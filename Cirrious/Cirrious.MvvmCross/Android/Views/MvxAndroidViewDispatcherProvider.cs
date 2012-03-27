#region Copyright
// <copyright file="MvxAndroidViewDispatcherProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Android.Views
{
    public class MvxAndroidViewDispatcherProvider
        : IMvxViewDispatcherProvider
          , IMvxServiceConsumer<IMvxAndroidCurrentTopActivity>
    {
        private readonly IMvxAndroidViewPresenter _presenter;

        public MvxAndroidViewDispatcherProvider(IMvxAndroidViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public IMvxViewDispatcher Dispatcher
        {
            get { return new MvxAndroidViewDispatcher(this.GetService<IMvxAndroidCurrentTopActivity>().Activity, _presenter); }
        }
    }
}