// MvxAndroidViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxAndroidViewPresenter
        : IMvxAndroidViewPresenter
          , IMvxServiceConsumer
    {
        private Activity Activity
        {
            get { return this.GetService<IMvxAndroidCurrentTopActivity>().Activity; }
        }

        public virtual void Show(MvxShowViewModelRequest request)
        {
            var requestTranslator = this.GetService<IMvxAndroidViewModelRequestTranslator>();
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