#region Copyright

// <copyright file="MvxNotifyProperty.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.ComponentModel;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.ViewModel
{
    public abstract class MvxNotifyProperty 
        : INotifyPropertyChanged
        , IMvxServiceConsumer<IMvxViewDispatcherProvider>
    {
        protected IMvxViewDispatcher ViewDispatcher
        {
            get { return this.GetService<IMvxViewDispatcherProvider>().Dispatcher; }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected void FirePropertyChanged(string whichProperty)
        {
            // check for subscription before going multithreaded
            if (PropertyChanged == null)
                return;

            if (ViewDispatcher != null)
                ViewDispatcher.RequestMainThreadAction(
                    () =>
                        {
                            // take a copy - see RoadWarrior's answer on http://stackoverflow.com/questions/282653/checking-for-null-before-event-dispatching-thread-safe/282741#282741
                            var handler = PropertyChanged;

                            if (handler != null)
                                handler(this, new PropertyChangedEventArgs(whichProperty));
                        });
        }
    }
}