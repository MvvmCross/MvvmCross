using System;
using System.IO;

namespace Phone7.Fx.IO.Compression
{
    public class GZipStream : Stream
    {
        private DeflateStream _deflateStream;

        public GZipStream(Stream stream, CompressionMode mode)
            : this(stream, mode, false)
        {
        }

        public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen)
        {
            _deflateStream = new DeflateStream(stream, mode, leaveOpen, true);
        }

        public override bool CanRead
        {
            get
            {
                if (_deflateStream == null)
                {
                    return false;
                }

                return _deflateStream.CanRead;
            }
        }

        public override bool CanWrite
        {
            get
            {
                if (_deflateStream == null)
                {
                    return false;
                }

                return _deflateStream.CanWrite;
            }
        }

        public override bool CanSeek
        {
            get
            {
                if (_deflateStream == null)
                {
                    return false;
                }

                return _deflateStream.CanSeek;
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
            if (_deflateStream == null)
            {
                throw new ObjectDisposedException(null, "SR.ObjectDisposed_StreamClosed");
            }
            _deflateStream.Flush();
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

       
        public override IAsyncResult BeginRead(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
        {
            if (_deflateStream == null)
            {
                throw new InvalidOperationException("SR.ObjectDisposed_StreamClosed");
            }
            return _deflateStream.BeginRead(array, offset, count, asyncCallback, asyncState);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            if (_deflateStream == null)
            {
                throw new InvalidOperationException("SR.ObjectDisposed_StreamClosed");
            }
            return _deflateStream.EndRead(asyncResult);
        }

      
        public override IAsyncResult BeginWrite(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
        {
            if (_deflateStream == null)
            {
                throw new InvalidOperationException("SR.ObjectDisposed_StreamClosed");
            }
            return _deflateStream.BeginWrite(array, offset, count, asyncCallback, asyncState);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            if (_deflateStream == null)
            {
                throw new InvalidOperationException("SR.ObjectDisposed_StreamClosed");
            }
            _deflateStream.EndWrite(asyncResult);
        }
        public override int Read(byte[] array, int offset, int count)
        {
            if (_deflateStream == null)
            {
                throw new ObjectDisposedException(null, "SR.ObjectDisposed_StreamClosed");
            }

            return _deflateStream.Read(array, offset, count);
        }

        public override void Write(byte[] array, int offset, int count)
        {
            if (_deflateStream == null)
            {
                throw new ObjectDisposedException(null, "SR.ObjectDisposed_StreamClosed");
            }

            _deflateStream.Write(array, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && _deflateStream != null)
                {
                    _deflateStream.Close();
                }
                _deflateStream = null;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public Stream BaseStream
        {
            get
            {
                if (_deflateStream != null)
                {
                    return _deflateStream.BaseStream;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}