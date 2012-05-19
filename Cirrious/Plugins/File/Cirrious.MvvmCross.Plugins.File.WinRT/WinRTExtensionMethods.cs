#warning Copyright needed
using System;
using Windows.Foundation;

namespace Cirrious.MvvmCross.Plugins.File.WinRT
{
#warning THis should go to a shared dll?
    public static class WinRTExtensionMethods
    {
        public static void Await(this IAsyncAction operation)
        {
            var task = operation.AsTask();
            task.Wait();
            if (task.Exception != null)
            {
                // TODO - is this correct?
                throw task.Exception.InnerException;
            }
        }

        public static TResult Await<TResult>(this IAsyncOperation<TResult> operation)
        {
            var task = operation.AsTask<TResult>();
            task.Wait();
            if (task.Exception != null)
            {
                // TODO - is this correct?
                throw task.Exception.InnerException;
            }

            return task.Result;
        }
    }
}