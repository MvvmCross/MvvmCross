#region Copyright

// <copyright file="MvxAndroidViewsContainer.cs" company="Cirrious">
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
using Android.Content;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;
using Newtonsoft.Json;

#endregion

namespace Cirrious.MvvmCross.Android.Views
{
    public class MvxAndroidViewsContainer
        : MvxViewsContainer
          , IMvxAndroidViewModelRequestTranslator
          , IMvxAndroidActivityTracker
          , IMvxAndroidSubViewServices
    {
        private const string ExtrasKey = "MvxLaunchData";

        private readonly Context _applicationContext;
        private Activity _lastSeenTopLevelViewActivity;
        private IMvxViewModel _lastSeenTopLevelViewModel;

        public MvxAndroidViewsContainer(Context applicationContext)
        {
            _applicationContext = applicationContext;
        }

        #region IMvxViewDispatcherProvider Members

        public override IMvxViewDispatcher Dispatcher
        {
            get { return new MvxAndroidViewDispatcher(_lastSeenTopLevelViewActivity); }
        }

        #endregion

        #region Implementation of IMvxAndroidViewModelRequestTranslator

        public MvxShowViewModelRequest GetRequestFromIntent(Intent intent)
        {
            var extraData = intent.Extras.GetString(ExtrasKey);
            if (extraData == null)
                return null;

            return JsonConvert.DeserializeObject<MvxShowViewModelRequest>(extraData);
        }

        public Intent GetIntentFor(MvxShowViewModelRequest request)
        {
            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
            {
                throw new MvxException("View Type not found for " + request.ViewModelType);
            }

            var requestText = JsonConvert.SerializeObject(request);

#warning - not entirely happy with this _lastSeenTopLevelViewActivity here - is this correct for subviews?
            var intent = new Intent(_lastSeenTopLevelViewActivity, viewType);
            intent.PutExtra(ExtrasKey, requestText);
            intent.SetFlags(ActivityFlags.NewTask);
            return intent;
        }

        #endregion

        #region Implementation of IMvxAndroidActivityTracker

        public Activity CurrentTopLevelActivity
        {
            get { return _lastSeenTopLevelViewActivity; }
        }

        public void OnTopLevelAndroidActivity(Activity activity, IMvxViewModel viewModel)
        {
            _lastSeenTopLevelViewActivity = activity;
            _lastSeenTopLevelViewModel = viewModel;
        }

        public void OnSubViewAndroidActivity(Activity activity)
        {
            // currently ignored!
        }

        public void SetInitialAndroidActivity(Activity activity)
        {
            _lastSeenTopLevelViewActivity = activity;
            _lastSeenTopLevelViewModel = null;
        }

        #endregion

        #region Implementation of IMvxAndroidSubViewServices

        public IMvxViewModel CurrentTopLevelViewModel
        {
            get { return _lastSeenTopLevelViewModel; }
        }

        #endregion
    }
}