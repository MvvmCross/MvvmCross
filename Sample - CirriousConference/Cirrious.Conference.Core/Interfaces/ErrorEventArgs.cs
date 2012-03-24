using System;

namespace Cirrious.Conference.Core.Interfaces
{
    public class ErrorEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public ErrorEventArgs(string message)
        {
            Message = message;
        }
    }
}
