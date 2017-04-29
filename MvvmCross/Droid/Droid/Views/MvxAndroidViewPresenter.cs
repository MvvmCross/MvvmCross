// MvxAndroidViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Droid.Views
{
    public class MvxAndroidViewPresenter
        : MvxViewPresenter, IMvxAndroidViewPresenter
    {
        protected Activity Activity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public override void Show(MvxViewModelRequest request)
        {
            var intent = CreateIntentForRequest(request);
            Show(intent);
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            var presentationHint = hint as MvxClosePresentationHint;
            if (presentationHint != null)
            {
                Close(presentationHint.ViewModelToClose);
                return;
            }

            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }

        public override void Close(IMvxViewModel viewModel)
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
                Mvx.Warning(
                    "Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return;
            }

            activity.Finish();
        }

        protected virtual void Show(Intent intent)
        {
            var activity = Activity;
            if (activity == null)
            {
                MvxTrace.Warning("Cannot Resolve current top activity");
                return;
            }
            activity.StartActivity(intent);
        }

        protected virtual Intent CreateIntentForRequest(MvxViewModelRequest request)
        {
            var requestTranslator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();
            var intent = requestTranslator.GetIntentFor(request);
            return intent;
        }
    }
}