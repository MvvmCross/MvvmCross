// MvxAndroidViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxAndroidViewPresenter
        : IMvxAndroidViewPresenter
    {
        protected Activity Activity
        {
            get { return Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity; }
        }

        public virtual void Show(MvxViewModelRequest request)
        {
            var requestTranslator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();
            var intent = requestTranslator.GetIntentFor(request);
            Activity.StartActivity(intent);
        }

        public virtual void ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is MvxClosePresentationHint)
            {
                Close((hint as MvxClosePresentationHint).ViewModelToClose);
                return;
            }

            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }

        public virtual void Close(IMvxViewModel viewModel)
        {
            var activity = Activity;

            var currentView = activity as IMvxView;

            if (currentView == null)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe has no current page");
                return;
            }

            if (currentView.ViewModel != viewModel)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return;
            }

            activity.Finish();
        }
    }
}