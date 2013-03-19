﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.ViewModels;

namespace BestSellers
{
    public interface IErrorReporter
    {
        void ReportError(string error);
    }

    public class ErrorEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public ErrorEventArgs(string message)
        {
            Message = message;
        }
    }

    public interface IErrorSource
    {
        event EventHandler<ErrorEventArgs> ErrorReported;
    }

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

    public class App 
        : MvxApplication
        
    {
        public App()
        {
            Mvx.RegisterSingleton<IMvxStartNavigation>(new StartApplicationObject());

            var errorApplicationObject = new ErrorApplicationObject();
            Mvx.RegisterSingleton<IErrorReporter>(errorApplicationObject);
            Mvx.RegisterSingleton<IErrorSource>(errorApplicationObject);
        }
    }
}
