// MvxAndroidViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

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

        public bool RequestNavigate(MvxShowViewModelRequest request)
        {
            return RequestMainThreadAction(() => _presenter.Show(request));
        }
    }
}