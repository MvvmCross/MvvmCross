// MvxAndroidViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxAndroidViewPresenter
        : IMvxAndroidViewPresenter
    {
        private Activity Activity
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
            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }
    }
}