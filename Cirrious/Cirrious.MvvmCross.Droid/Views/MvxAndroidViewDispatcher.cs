// MvxAndroidViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxAndroidViewDispatcher
        : MvxMainThreadDispatcher
          , IMvxViewDispatcher
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