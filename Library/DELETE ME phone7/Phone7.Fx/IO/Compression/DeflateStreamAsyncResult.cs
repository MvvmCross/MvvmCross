using System;
using System.Threading;

namespace Phone7.Fx.IO.Compression
{
    internal class DeflateStreamAsyncResult : IAsyncResult
    {
        public byte[] Buffer;
        public int Offset;
        public int Count;
        public bool IsWrite;

        private object _mAsyncObject;               // Caller's async object. 
        private readonly object _mAsyncState;                // Caller's state object.
        private readonly AsyncCallback _mAsyncCallback;      // Caller's callback method. 

        private object _mResult;                     // Final IO result to be returned byt the End*() method.
        internal bool MCompletedSynchronously;      // true if the operation completed synchronously.
        private int _mInvokedCallback;               // 0 is callback is not called 
        private int _mCompleted;                     // 0 if not completed >0 otherwise.
        private object _mEvent;                      // lazy allocated event to be returned in the IAsyncResult for the client to wait on 

        public DeflateStreamAsyncResult(object asyncObject, object asyncState,
                                   AsyncCallback asyncCallback,
                                   byte[] buffer, int offset, int count)
        {

            this.Buffer = buffer;
            this.Offset = offset;
            this.Count = count;
            MCompletedSynchronously = true;
            _mAsyncObject = asyncObject;
            _mAsyncState = asyncState;
            _mAsyncCallback = asyncCallback;
        }

        // Interface method to return the caller's state object.
        public object AsyncState
        {
            get
            {
                return _mAsyncState;
            }
        }

        // Interface property to return a WaitHandle that can be waited on for I/O completion.
        // This property implements lazy event creation.
        // the event object is only created when this property is accessed,
        // since we're internally only using callbacks, as long as the user is using 
        // callbacks as well we will not create an event at all.
        public WaitHandle AsyncWaitHandle
        {
            get
            {
                // save a copy of the completion status
                int savedCompleted = _mCompleted;
                if (_mEvent == null)
                {
                    // lazy allocation of the event:
                    // if this property is never accessed this object is never created
                    Interlocked.CompareExchange(ref _mEvent, new ManualResetEvent(savedCompleted != 0), null);
                }

                ManualResetEvent castedEvent = (ManualResetEvent)_mEvent;
                if (savedCompleted == 0 && _mCompleted != 0)
                {
                    // if, while the event was created in the reset state, 
                    // the IO operation completed, set the event here.
                    castedEvent.Set();
                }
                return castedEvent;
            }
        }

        // Interface property, returning synchronous completion status.
        public bool CompletedSynchronously
        {
            get
            {
                return MCompletedSynchronously;
            }
        }

        // Interface property, returning completion status. 
        public bool IsCompleted
        {
            get
            {
                return _mCompleted != 0;
            }
        }

        // Internal property for setting the IO result. 
        internal object Result
        {
            get
            {
                return _mResult;
            }
        }

        internal void Close()
        {
            if (_mEvent != null)
            {
                ((ManualResetEvent)_mEvent).Close();
            }
        }

        internal void InvokeCallback(bool completedSynchronously, object result)
        {
            Complete(completedSynchronously, result);
        }

        internal void InvokeCallback(object result)
        {
            Complete(result);
        }

        // Internal method for setting completion. 
        // As a side effect, we'll signal the WaitHandle event and clean up.
        private void Complete(bool completedSynchronously, object result)
        {
            MCompletedSynchronously = completedSynchronously;
            Complete(result);
        }

        private void Complete(object result)
        {
            _mResult = result;

            // Set IsCompleted and the event only after the usercallback method.
            Interlocked.Increment(ref _mCompleted);

            if (_mEvent != null)
            {
                ((ManualResetEvent)_mEvent).Set();
            }

            if (Interlocked.Increment(ref _mInvokedCallback) == 1)
            {
                if (_mAsyncCallback != null)
                {
                    _mAsyncCallback(this);
                }
            }
        }

    } 
}