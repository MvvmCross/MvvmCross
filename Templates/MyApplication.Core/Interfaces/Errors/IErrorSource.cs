using System;

namespace MyApplication.Core.Interfaces.Errors
{
    public interface IErrorSource
    {
        event EventHandler<ErrorEventArgs> ErrorReported;
    }
}