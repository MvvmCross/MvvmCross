using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Phone7.Fx.IO.Compression
{
    public class DeflateStream : Stream
    {
        private const int BufferSize = 4096;

        internal delegate void AsyncWriteDelegate(byte[] array, int offset, int count, bool isAsync);

        private Stream _stream;
        private readonly CompressionMode _mode;
        private readonly bool _leaveOpen;
        private readonly Inflater _inflater;
        private readonly Deflater _deflater;
        private readonly byte[] _buffer;

        private int _asyncOperations;
        private readonly AsyncCallback _mCallBack;
        private readonly AsyncWriteDelegate _mAsyncWriterDelegate;

        public DeflateStream(Stream stream, CompressionMode mode)
            : this(stream, mode, false, false)
        {
        }

        public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen)
            : this(stream, mode, leaveOpen, false)
        {
        }

        internal DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen, bool usingGZip)
        {
            _stream = stream;
            _mode = mode;
            _leaveOpen = leaveOpen;

            if (_stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            switch (_mode)
            {
                case CompressionMode.Decompress:
                    if (!(_stream.CanRead))
                    {
                        throw new ArgumentException("SR.NotReadableStream");
                    }
                    _inflater = new Inflater(usingGZip);
                    _mCallBack = new AsyncCallback(ReadCallback);
                    break;

                case CompressionMode.Compress:
                    if (!(_stream.CanWrite))
                    {
                        throw new ArgumentException("SR.NotWriteableStream");
                    }
                    _deflater = new Deflater(usingGZip);
                    _mAsyncWriterDelegate = new AsyncWriteDelegate(this.InternalWrite);
                    _mCallBack = new AsyncCallback(WriteCallback);
                    break;

                default:
                    throw new ArgumentException("SR.ArgumentOutOfRange_Enum");
            }
            _buffer = new byte[BufferSize];
        }

        public override bool CanRead
        {
            get
            {
                if (_stream == null)
                {
                    return false;
                }

                return (_mode == CompressionMode.Decompress && _stream.CanRead);
            }
        }

        public override bool CanWrite
        {
            get
            {
                if (_stream == null)
                {
                    return false;
                }

                return (_mode == CompressionMode.Compress && _stream.CanWrite);
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override long Length
        {
            get
            {
                throw new NotSupportedException("SR.NotSupported");
            }
        }

        public override long Position
        {
            get
            {
                throw new NotSupportedException("SR.NotSupported");
            }

            set
            {
                throw new NotSupportedException("SR.NotSupported");
            }
        }

        public override void Flush()
        {
            if (_stream == null)
            {
                throw new ObjectDisposedException(null, "SR.ObjectDisposed_StreamClosed");
            }
            return;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("SR.NotSupported");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("SR.NotSupported");
        }

        public override int Read(byte[] array, int offset, int count)
        {
            EnsureDecompressionMode();
            ValidateParameters(array, offset, count);

            int currentOffest = offset;
            int remainingCount = count;

            while (true)
            {
                int bytesRead = _inflater.Inflate(array, currentOffest, remainingCount);
                currentOffest += bytesRead;
                remainingCount -= bytesRead;

                if (remainingCount == 0)
                {
                    break;
                }

                if (_inflater.Finished())
                {
                    // if we finished decompressing, we can't have anything left in the outputwindow.
                    Debug.Assert(_inflater.AvailableOutput == 0, "We should have copied all stuff out!");
                    break;
                }

                Debug.Assert(_inflater.NeedsInput(), "We can only run into this case if we are short of input");

                int bytes = _stream.Read(_buffer, 0, _buffer.Length);
                if (bytes == 0)
                {
                    break;      //Do we want to throw an exception here?
                }

                _inflater.SetInput(_buffer, 0, bytes);
            }

            return count - remainingCount;
        }

        private void ValidateParameters(byte[] array, int offset, int count)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            if (array.Length - offset < count)
            {
                throw new ArgumentException("SR.InvalidArgumentOffsetCount");
            }

            if (_stream == null)
            {
                throw new ObjectDisposedException(null, "SR.ObjectDisposed_StreamClosed");
            }
        }

        private void EnsureDecompressionMode()
        {
            if (_mode != CompressionMode.Decompress)
            {
                throw new InvalidOperationException("SR.CannotReadFromDeflateStream");
            }
        }

        private void EnsureCompressionMode()
        {
            if (_mode != CompressionMode.Compress)
            {
                throw new InvalidOperationException("SR.CannotWriteToDeflateStream");
            }
        }

        //[HostProtection(ExternalThreading = true)]
        public override IAsyncResult BeginRead(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
        {
            EnsureDecompressionMode();
            if (_asyncOperations != 0)
            {
                throw new InvalidOperationException("SR.InvalidBeginCall");
            }
            Interlocked.Increment(ref _asyncOperations);

            try
            {
                ValidateParameters(array, offset, count);

                DeflateStreamAsyncResult userResult = new DeflateStreamAsyncResult(
                        this, asyncState, asyncCallback, array, offset, count) {IsWrite = false};

                // Try to read decompressed data in output buffer
                int bytesRead = _inflater.Inflate(array, offset, count);
                if (bytesRead != 0)
                {
                    // If decompression output buffer is not empty, return immediately.
                    // 'true' means we complete synchronously. 
                    userResult.InvokeCallback(true, (object)bytesRead);
                    return userResult;
                }

                if (_inflater.Finished())
                {
                    // end of compression stream 
                    userResult.InvokeCallback(true, (object)0);
                    return userResult;
                }

                // If there is no data on the output buffer and we are not at
                // the end of the stream, we need to get more data from the base stream
                _stream.BeginRead(_buffer, 0, _buffer.Length, _mCallBack, userResult);
                userResult.MCompletedSynchronously &= userResult.IsCompleted;

                return userResult;
            }
            catch
            {
                Interlocked.Decrement(ref _asyncOperations);
                throw;
            }
        }

        // callback function for asynchrous reading on base stream 
        private void ReadCallback(IAsyncResult baseStreamResult)
        {
            DeflateStreamAsyncResult outerResult = (DeflateStreamAsyncResult)baseStreamResult.AsyncState;
            outerResult.MCompletedSynchronously &= baseStreamResult.CompletedSynchronously;
            int bytesRead = 0;

            try
            {
                bytesRead = _stream.EndRead(baseStreamResult);
            }
            catch (Exception exc)
            {
                // Defer throwing this until EndXxx where we are ensured of user code on the stack. 
                outerResult.InvokeCallback(exc);
                return;
            }

            if (bytesRead <= 0)
            {
                // This indicates the base stream has received EOF 
                outerResult.InvokeCallback((object)0);
                return;
            }

            // Feed the data from base stream into decompression engine 
            _inflater.SetInput(_buffer, 0, bytesRead);
            bytesRead = _inflater.Inflate(outerResult.Buffer, outerResult.Offset, outerResult.Count);
            if (bytesRead == 0 && !_inflater.Finished())
            {
                // We could have read in head information and didn't get any data. 
                // Read from the base stream again.
                // Need to solve recusion. 
                _stream.BeginRead(_buffer, 0, _buffer.Length, _mCallBack, outerResult);
            }
            else
            {
                outerResult.InvokeCallback((object)bytesRead);
            }
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            EnsureDecompressionMode();

            if (_asyncOperations != 1)
            {
                throw new InvalidOperationException("SR.InvalidEndCall");
            }

            if (asyncResult == null)
            {
                throw new ArgumentNullException("asyncResult");
            }

            if (_stream == null)
            {
                throw new InvalidOperationException("SR.ObjectDisposed_StreamClosed");
            }

            DeflateStreamAsyncResult myResult = asyncResult as DeflateStreamAsyncResult;

            if (myResult == null)
            {
                throw new ArgumentNullException("asyncResult");
            }

            try
            {
                if (!myResult.IsCompleted)
                {
                    myResult.AsyncWaitHandle.WaitOne();
                }
            }
            finally
            {
                Interlocked.Decrement(ref _asyncOperations);
                // this will just close the wait handle 
                myResult.Close();
            }

            if (myResult.Result is Exception)
            {
                throw (Exception)(myResult.Result);
            }

            return (int)myResult.Result;
        }


        public override void Write(byte[] array, int offset, int count)
        {
            EnsureCompressionMode();
            ValidateParameters(array, offset, count);
            InternalWrite(array, offset, count, false);
        }

        internal void InternalWrite(byte[] array, int offset, int count, bool isAsync)
        {
            int bytesCompressed;

            // compressed the bytes we already passed to the deflater
            while (!_deflater.NeedsInput())
            {
                bytesCompressed = _deflater.GetDeflateOutput(_buffer);
                if (bytesCompressed != 0)
                {
                    if (isAsync)
                    {
                        IAsyncResult result = _stream.BeginWrite(_buffer, 0, bytesCompressed, null, null);
                        _stream.EndWrite(result);
                    }
                    else
                        _stream.Write(_buffer, 0, bytesCompressed);
                }
            }

            _deflater.SetInput(array, offset, count);

            // compressed the new input
            while (!_deflater.NeedsInput())
            {
                bytesCompressed = _deflater.GetDeflateOutput(_buffer);
                if (bytesCompressed != 0)
                {
                    if (isAsync)
                    {
                        IAsyncResult result = _stream.BeginWrite(_buffer, 0, bytesCompressed, null, null);
                        _stream.EndWrite(result);
                    }
                    else
                        _stream.Write(_buffer, 0, bytesCompressed);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                // Flush on the underlying stream can throw (ex., low disk space) 
                if (disposing && _stream != null)
                {
                    Flush();

                    // Need to do close the output stream in compression mode
                    if (_mode == CompressionMode.Compress && _stream != null)
                    {
                        int bytesCompressed;
                        // compress any bytes left. 
                        while (!_deflater.NeedsInput())
                        {
                            bytesCompressed = _deflater.GetDeflateOutput(_buffer);
                            if (bytesCompressed != 0)
                            {
                                _stream.Write(_buffer, 0, bytesCompressed);
                            }
                        }

                        // Write the end of compressed stream data.
                        // We can safely do this since the buffer is large enough. 
                        bytesCompressed = _deflater.Finish(_buffer);

                        if (bytesCompressed > 0)
                            _stream.Write(_buffer, 0, bytesCompressed);
                    }
                }
            }
            finally
            {
                try
                {
                    // Attempt to close the stream even if there was an IO error from Flushing.
                    // Note that Stream.Close() can potentially throw here (may or may not be 
                    // due to the same Flush error). In this case, we still need to ensure 
                    // cleaning up internal resources, hence the finally block.
                    if (disposing && !_leaveOpen && _stream != null)
                    {
                        _stream.Close();
                    }
                }
                finally
                {
                    _stream = null;
                    base.Dispose(disposing);
                }
            }
        }


        //[HostProtection(ExternalThreading = true)]
        public override IAsyncResult BeginWrite(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
        {
            EnsureCompressionMode();
            if (_asyncOperations != 0)
            {
                throw new InvalidOperationException("SR.InvalidBeginCall");
            }
            Interlocked.Increment(ref _asyncOperations);

            try
            {
                ValidateParameters(array, offset, count);

                DeflateStreamAsyncResult userResult = new DeflateStreamAsyncResult(
                        this, asyncState, asyncCallback, array, offset, count);
                userResult.IsWrite = true;

                _mAsyncWriterDelegate.BeginInvoke(array, offset, count, true, _mCallBack, userResult);
                userResult.MCompletedSynchronously &= userResult.IsCompleted;

                return userResult;
            }
            catch
            {
                Interlocked.Decrement(ref _asyncOperations);
                throw;
            }
        }

        // callback function for asynchrous reading on base stream
        private void WriteCallback(IAsyncResult asyncResult)
        {
            DeflateStreamAsyncResult outerResult = (DeflateStreamAsyncResult)asyncResult.AsyncState;
            outerResult.MCompletedSynchronously &= asyncResult.CompletedSynchronously;

            try
            {
                _mAsyncWriterDelegate.EndInvoke(asyncResult);
            }
            catch (Exception exc)
            {
                // Defer throwing this until EndXxx where we are ensured of user code on the stack.
                outerResult.InvokeCallback(exc);
                return;
            }
            outerResult.InvokeCallback(null);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            EnsureCompressionMode();

            if (_asyncOperations != 1)
            {
                throw new InvalidOperationException("SR.InvalidEndCall");
            }

            if (asyncResult == null)
            {
                throw new ArgumentNullException("asyncResult");
            }

            if (_stream == null)
            {
                throw new InvalidOperationException("SR.ObjectDisposed_StreamClosed");
            }

            DeflateStreamAsyncResult myResult = asyncResult as DeflateStreamAsyncResult;

            if (myResult == null)
            {
                throw new ArgumentNullException("asyncResult");
            }

            try
            {
                if (!myResult.IsCompleted)
                {
                    myResult.AsyncWaitHandle.WaitOne();
                }
            }
            finally
            {
                Interlocked.Decrement(ref _asyncOperations);
                // this will just close the wait handle
                myResult.Close();
            }

            if (myResult.Result is Exception)
            {
                throw (Exception)(myResult.Result);
            }
        }


        public Stream BaseStream
        {
            get
            {
                return _stream;
            }
        }
    } 
}