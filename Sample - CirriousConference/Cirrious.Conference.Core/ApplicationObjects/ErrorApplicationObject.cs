using System;
using Cirrious.Conference.Core.Interfaces;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.Conference.Core.ApplicationObjects
{
    public class ErrorApplicationObject
        : MvxApplicationObject
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