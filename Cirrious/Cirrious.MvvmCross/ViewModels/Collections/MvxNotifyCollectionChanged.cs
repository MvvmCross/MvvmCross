#region Copyright
// <copyright file="MvxNotifyCollectionChanged.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels.Collections;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.ViewModels
{
    // TODO:
    // 1. Copy Mono's observable collection in here
    // 2. Copy Mono's IMvxNotifyCollectionChanged in here - as IMvxNotifyCollectionChanged
    // 3. Work out a way to do the ValueConverters for Win7
    // 4. Get the Json across
    // 5. Port all the samples across...
    // 6. Relax...

    public abstract class MvxNotifyCollectionChanged
        : IMvxNotifyCollectionChanged
        , IMvxServiceConsumer<IMvxViewDispatcherProvider>
    {
        protected IMvxViewDispatcher ViewDispatcher
        {
            get { return this.GetService<IMvxViewDispatcherProvider>().Dispatcher; }
        }

        #region Implementation of IMvxNotifyCollectionChanged

        public event MvxNotifyCollectionChangedEventHandler CollectionChanged;

        public object NativeCollection { get; set; }

        #endregion

        protected void FireCollectionReset()
        {
            FireCollectionChanged(new MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction.Reset));
        }

        protected void FireCollectionChanged(MvxNotifyCollectionChangedEventArgs args)
        {
            // check for subscription before going multithreaded
            if (CollectionChanged == null)
                return;

            if (ViewDispatcher != null)
                ViewDispatcher.RequestMainThreadAction(
                    () =>
                        {
                            // take a copy - see RoadWarrior's answer on http://stackoverflow.com/questions/282653/checking-for-null-before-event-dispatching-thread-safe/282741#282741
                            var handler = CollectionChanged;

                            if (handler != null)
                                handler(this, args);
                        });
        }
    }
}
