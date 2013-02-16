using System;
using Cirrious.MvvmCross.ViewModels;
using MyApplication.Core.Interfaces.Errors;

namespace MyApplication.Core.ApplicationObjects
{
    public class ErrorApplicationObject
        : MvxNavigatingObject
          , IErrorReporter
          , IErrorSource
    {
        public void ReportError(string error)
        {
            if (ErrorReported == null)
                return;

            InvokeOnMainThread(() =>
                {
                    var handler = ErrorReported;
                    if (handler != null)
                        handler(this, new ErrorEventArgs(error));
                });
        }

        public event EventHandler<ErrorEventArgs> ErrorReported;
    }
}