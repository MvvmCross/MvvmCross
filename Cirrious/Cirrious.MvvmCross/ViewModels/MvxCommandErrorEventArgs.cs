using System;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxCommandErrorEventArgs : EventArgs
    {
        public MvxCommandErrorEventArgs(Exception error)
        {
            Error = error;
        }

        public Exception Error { get; private set; }
    }
}
