using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.ViewModels
{
    public abstract class MvxMainThreadDispatchingObject 
        : IMvxServiceConsumer<IMvxViewDispatcherProvider>
    {
        protected IMvxViewDispatcher ViewDispatcher
        {
            get { return this.GetService<IMvxViewDispatcherProvider>().Dispatcher; }
        }

        protected void InvokeOnMainThread(Action action)
        {
            if (ViewDispatcher != null)
                ViewDispatcher.RequestMainThreadAction(action);
        }
    }
}