#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MvxViewModel.cs" company="Cirrious">
// //     (c) Copyright Cirrious. http://www.cirrious.com
// //     This source is subject to the Microsoft Public License (Ms-PL)
// //     Please see license.txt on http://opensource.org/licenses/ms-pl.html
// //     All other rights reserved.
// // </copyright>
// // 
// // Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// // ------------------------------------------------------------------------

#endregion

#region using

using System.ComponentModel;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;

#endregion

namespace Cirrious.MvvmCross.ViewModel
{
    public abstract class MvxPropertyNotify : INotifyPropertyChanged, IMvxServiceConsumer<IMvxViewDispatcherProvider>
    {
        protected IMvxViewDispatcher ViewDispatcher
        {
            get { return this.GetService<IMvxViewDispatcherProvider>().Dispatcher; }
        }

        protected void FirePropertyChanged(string whichProperty)
        {
            if (ViewDispatcher != null)
                ViewDispatcher.RequestMainThreadAction(
                    () =>
                        {
                            if (PropertyChanged != null)
                                PropertyChanged(this, new PropertyChangedEventArgs(whichProperty));
                        });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}