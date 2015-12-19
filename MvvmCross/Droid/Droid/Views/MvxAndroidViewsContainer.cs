// MvxAndroidViewsContainer.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Views
{
    using System;

    using Android.Content;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    public class MvxAndroidViewsContainer
        : MvxViewsContainer
         , IMvxAndroidViewsContainer
    {
        private const string ExtrasKey = "MvxLaunchData";
        private const string SubViewModelKey = "MvxSubViewModelKey";

        private readonly Context _applicationContext;

        public MvxAndroidViewsContainer(Context applicationContext)
        {
            this._applicationContext = applicationContext;
        }

        #region Implementation of IMvxAndroidViewModelRequestTranslator

        public virtual IMvxViewModel Load(Intent intent, IMvxBundle savedState)
        {
            return Load(intent, null, null);
        }

        public virtual IMvxViewModel Load(Intent intent, IMvxBundle savedState, Type viewModelTypeHint)
        {
            if (intent == null)
            {
                MvxTrace.Error("Null Intent seen when creating ViewModel");
                return null;
            }

            if (intent.Action == Intent.ActionMain)
            {
                MvxTrace.Trace("Creating ViewModel for ActionMain");
                return this.DirectLoad(savedState, viewModelTypeHint);
            }

            if (intent.Extras == null)
            {
                MvxTrace.Trace("Null Extras seen on Intent when creating ViewModel - have you tried to navigate to an MvvmCross View directly? Will try direct load");
                return this.DirectLoad(savedState, viewModelTypeHint);
            }

            IMvxViewModel mvxViewModel;
            if (this.TryGetEmbeddedViewModel(intent, out mvxViewModel))
            {
                MvxTrace.Trace("Embedded ViewModel used");
                return mvxViewModel;
            }

            MvxTrace.Trace("Attempting to load new ViewModel from Intent with Extras");
            var toReturn = this.CreateViewModelFromIntent(intent, savedState);
            if (toReturn != null)
                return toReturn;

            MvxTrace.Trace("ViewModel not loaded from Extras - will try DirectLoad");
            return this.DirectLoad(savedState, viewModelTypeHint);
        }

        protected virtual IMvxViewModel DirectLoad(IMvxBundle savedState, Type viewModelTypeHint)
        {
            if (viewModelTypeHint == null)
            {
                Mvx.Error("Unable to load viewmodel - no type hint provided");
                return null;
            }

            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModelRequest = MvxViewModelRequest.GetDefaultRequest(viewModelTypeHint);
            var viewModel = loaderService.LoadViewModel(viewModelRequest, savedState);
            return viewModel;
        }

        protected virtual IMvxViewModel CreateViewModelFromIntent(Intent intent, IMvxBundle savedState)
        {
            var extraData = intent.Extras.GetString(ExtrasKey);
            if (extraData == null)
                return null;

            var converter = Mvx.Resolve<IMvxNavigationSerializer>();
            var viewModelRequest = converter.Serializer.DeserializeObject<MvxViewModelRequest>(extraData);

            return this.ViewModelFromRequest(viewModelRequest, savedState);
        }

        protected virtual IMvxViewModel ViewModelFromRequest(MvxViewModelRequest viewModelRequest, IMvxBundle savedState)
        {
            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(viewModelRequest, savedState);
            return viewModel;
        }

        protected virtual bool TryGetEmbeddedViewModel(Intent intent, out IMvxViewModel mvxViewModel)
        {
            var embeddedViewModelKey = intent.Extras.GetInt(SubViewModelKey);
            if (embeddedViewModelKey != 0)
            {
                {
                    mvxViewModel = Mvx.Resolve<IMvxChildViewModelCache>().Get(embeddedViewModelKey);
                    return true;
                }
            }
            mvxViewModel = null;
            return false;
        }

        public virtual Intent GetIntentFor(MvxViewModelRequest request)
        {
            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
            {
                throw new MvxException("View Type not found for " + request.ViewModelType);
            }

            var converter = Mvx.Resolve<IMvxNavigationSerializer>();
            var requestText = converter.Serializer.SerializeObject(request);

            var intent = new Intent(this._applicationContext, viewType);
            intent.PutExtra(ExtrasKey, requestText);

            this.AdjustIntentForPresentation(intent, request);

            return intent;
        }

        protected virtual void AdjustIntentForPresentation(Intent intent, MvxViewModelRequest request)
        {
            intent.AddFlags(ActivityFlags.NewTask);

#warning we want to do things here... clear top, remove history item, etc
            //#warning ClearTop is not enough :/ Need to work on an Intent based scheme like http://stackoverflow.com/questions/3007998/on-logout-clear-activity-history-stack-preventing-back-button-from-opening-l
            //            if (request.ClearTop)
            //                intent.AddFlags(ActivityFlags.ClearTop);
        }

        public virtual Tuple<Intent, int> GetIntentWithKeyFor(IMvxViewModel viewModel)
        {
            var request = MvxViewModelRequest.GetDefaultRequest(viewModel.GetType());
            var intent = this.GetIntentFor(request);

            var key = Mvx.Resolve<IMvxChildViewModelCache>().Cache(viewModel);
            intent.PutExtra(SubViewModelKey, key);

            return new Tuple<Intent, int>(intent, key);
        }

        public void RemoveSubViewModelWithKey(int key)
        {
            Mvx.Resolve<IMvxChildViewModelCache>().Remove(key);
        }

        #endregion Implementation of IMvxAndroidViewModelRequestTranslator
    }
}