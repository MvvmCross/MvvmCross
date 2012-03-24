using System;

namespace Cirrious.Conference.Core.Interfaces
{
    public interface IErrorSource
    {
        event EventHandler<ErrorEventArgs> ErrorReported;
    }
}