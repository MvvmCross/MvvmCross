#region Copyright
// <copyright file="MvxAndroidViewsContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using Android.Content;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;
using Newtonsoft.Json;

#endregion

namespace Cirrious.MvvmCross.Android.Views
{
    public class MvxAndroidViewsContainer
        : MvxViewsContainer
        , IMvxAndroidViewModelRequestTranslator
        , IMvxServiceConsumer<IMvxAndroidCurrentTopActivity>
    {
        private const string ExtrasKey = "MvxLaunchData";

        private readonly Context _applicationContext;

        public MvxAndroidViewsContainer(Context applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public override IMvxViewDispatcher Dispatcher
        {
            get
            {
                return new MvxAndroidViewDispatcher(this.GetService<IMvxAndroidCurrentTopActivity>().Activity);
            }
        }

        #region Implementation of IMvxAndroidViewModelRequestTranslator

        public virtual MvxShowViewModelRequest GetRequestFromIntent(Intent intent)
        {
            if (intent == null || intent.Extras == null)
                return null;

            var extraData = intent.Extras.GetString(ExtrasKey);
            if (extraData == null)
                return null;

            return JsonConvert.DeserializeObject<MvxShowViewModelRequest>(extraData);
        }

        public virtual Intent GetIntentFor(MvxShowViewModelRequest request)
        {
            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
            {
                throw new MvxException("View Type not found for " + request.ViewModelType);
            }

            var requestText = JsonConvert.SerializeObject(request);

            var intent = new Intent(_applicationContext, viewType);
            intent.PutExtra(ExtrasKey, requestText);
#warning ClearTop is not enough :/ Need to work on an Intent based scheme like http://stackoverflow.com/questions/3007998/on-logout-clear-activity-history-stack-preventing-back-button-from-opening-l
            if (request.ClearTop)
                intent.AddFlags(ActivityFlags.ClearTop);
            intent.AddFlags(ActivityFlags.NewTask);
            return intent;
        }

        #endregion
    }
}