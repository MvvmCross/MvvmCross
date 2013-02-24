// MvxAndroidViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Cirrious.CrossCore.Droid.Interfaces;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxAndroidViewPresenter
        : IMvxAndroidViewPresenter
          , IMvxConsumer
    {
        private Activity Activity
        {
            get { return this.Resolve<IMvxAndroidCurrentTopActivity>().Activity; }
        }

        public virtual void Show(MvxShowViewModelRequest request)
        {
            var requestTranslator = this.Resolve<IMvxAndroidViewModelRequestTranslator>();
            var intent = requestTranslator.GetIntentFor(request);
            Activity.StartActivity(intent);
        }

        public virtual void Close(IMvxViewModel toClose)
        {
            toClose.ActOnRegisteredViews(view =>
                {
                    var activity = view as Activity;
                    if (activity != null)
                        activity.Finish();
                });
        }
    }
}