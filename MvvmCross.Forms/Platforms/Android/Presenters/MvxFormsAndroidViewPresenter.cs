// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Forms.Presenters;
using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Forms.Core;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using MvvmCross.Forms.Platforms.Android.Views;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace MvvmCross.Forms.Platforms.Android.Presenters
{
    public class MvxFormsAndroidViewPresenter
        : MvxAppCompatViewPresenter, IMvxFormsViewPresenter
    {
        public MvxFormsAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies, Application formsApplication) : base(androidViewAssemblies)
        {
            FormsApplication = formsApplication ?? throw new ArgumentNullException(nameof(formsApplication), "MvxFormsApplication cannot be null");
        }
        public Application FormsApplication { get; set; }

        private IMvxFormsPagePresenter _formsPagePresenter;
        public virtual IMvxFormsPagePresenter FormsPagePresenter
        {
            get
            {
                if (_formsPagePresenter == null)
                    throw new ArgumentNullException(nameof(FormsPagePresenter), "IMvxFormsPagePresenter cannot be null. Set the value in CreateViewPresenter in the setup.");
                return _formsPagePresenter;
            }
            set { _formsPagePresenter = value; }
        }

        public override Task<bool> Show(MvxViewModelRequest request)
        {
            return FormsPagePresenter.Show(request);
        }

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();
            FormsPagePresenter.RegisterAttributeTypes();
        }

        public override async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (!await FormsPagePresenter.ChangePresentation(hint)) return false;
            return await base.ChangePresentation(hint);
        }

        public override Task<bool> Close(IMvxViewModel viewModel)
        {
            return FormsPagePresenter.Close(viewModel);
        }

        public virtual bool ShowPlatformHost(Type hostViewModel = null)
        {
            // if there is no Actitivty host associated, assume is the current activity
            if (hostViewModel == null)
                hostViewModel = GetCurrentActivityViewModelType();

            var currentHostViewModelType = GetCurrentActivityViewModelType();
            if (hostViewModel != currentHostViewModelType)
            {
                var hostViewModelRequest = MvxViewModelRequest.GetDefaultRequest(hostViewModel);
                Show(hostViewModelRequest);
            }
            return true;
        }

        public virtual bool ClosePlatformViews()
        {
            CloseFragments();
            if (!(CurrentActivity is MvxFormsAppCompatActivity || CurrentActivity is MvxFormsApplicationActivity) &&
                !(CurrentActivity is MvxSplashScreenActivity || CurrentActivity is MvxSplashScreenAppCompatActivity))
                CurrentActivity?.Finish();
            return true;
        }
    }
}
