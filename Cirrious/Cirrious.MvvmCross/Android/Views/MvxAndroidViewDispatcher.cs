#region Copyright
// <copyright file="MvxAndroidViewDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using Android.App;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;

#endregion

namespace Cirrious.MvvmCross.Android.Views
{
    public class MvxAndroidViewDispatcher
        : MvxMainThreadDispatcher
        , IMvxViewDispatcher
        , IMvxServiceConsumer<IMvxAndroidViewModelRequestTranslator>
    {
        private readonly Activity _activity;
        private readonly IMvxAndroidViewPresenter _presenter;

        public MvxAndroidViewDispatcher(Activity activity, IMvxAndroidViewPresenter presenter)
            : base(activity)
        {
            _activity = activity;
            _presenter = presenter;
        }

        #region IMvxViewDispatcher Members

        public bool RequestNavigate(MvxShowViewModelRequest request)
        {
            return RequestMainThreadAction(() => _presenter.Show(request));
        }

        public bool RequestClose(IMvxViewModel toClose)
        {
            return RequestMainThreadAction(() => _presenter.Close(toClose));
        }

        public bool RequestRemoveBackStep()
        {
            // not supported on Android? Not sure how to do this currently...
            return false;
        }

        #endregion
    }
}