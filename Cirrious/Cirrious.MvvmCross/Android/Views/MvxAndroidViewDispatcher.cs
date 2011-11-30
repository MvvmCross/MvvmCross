#region Copyright

// <copyright file="MvxAndroidViewDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

#region using

using Android.App;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
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

        public MvxAndroidViewDispatcher(Activity activity)
            : base(activity)
        {
            _activity = activity;
        }

        #region IMvxViewDispatcher Members

        public bool RequestNavigate(MvxShowViewModelRequest request)
        {
            var requestTranslator = this.GetService<IMvxAndroidViewModelRequestTranslator>();
            var intent = requestTranslator.GetIntentFor(request);
            return InvokeOrBeginInvoke(() => _activity.StartActivity(intent));
        }

        public bool RequestNavigateBack()
        {
            return InvokeOrBeginInvoke(() => _activity.Finish());
        }

        public bool RequestRemoveBackStep()
        {
            // not supported on Android? Not sure how to do this currently...
            return false;
        }

        #endregion
    }
}