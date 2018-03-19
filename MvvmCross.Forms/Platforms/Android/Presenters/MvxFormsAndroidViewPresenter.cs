// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Forms.Presenters;
using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Forms.Core;
using MvvmCross.Platform.Android.Views;
using MvvmCross.ViewModels;
using MvvmCross.Forms.Platform.Android.Views;
using Xamarin.Forms;

namespace MvvmCross.Forms.Platform.Android.Presenters
{
    public class MvxFormsAndroidViewPresenter
        : MvxAppCompatViewPresenter, IMvxFormsViewPresenter
    {
        public MvxFormsAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies, Application formsApplication) : base(androidViewAssemblies)
        {
            FormsApplication = formsApplication ?? throw new ArgumentNullException(nameof(formsApplication), "MvxFormsApplication cannot be null");
        }

        private Application _formsApplication;
        public Application FormsApplication
        {
            get { return _formsApplication; }
            set { _formsApplication = value; }
        }

        private IMvxFormsPagePresenter _formsPagePresenter;
        public virtual IMvxFormsPagePresenter FormsPagePresenter
        {
            get
            {
                if (_formsPagePresenter == null)
                {
                    _formsPagePresenter = new MvxFormsPagePresenter(this);
                    Mvx.RegisterSingleton(_formsPagePresenter);
                }
                return _formsPagePresenter;
            }
            set
            {
                _formsPagePresenter = value;
            }
        }

        public override void Show(MvxViewModelRequest request)
        {
            FormsPagePresenter.Show(request);
        }

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();
            FormsPagePresenter.RegisterAttributeTypes();
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            FormsPagePresenter.ChangePresentation(hint);
            base.ChangePresentation(hint);
        }

        public override void Close(IMvxViewModel viewModel)
        {
            FormsPagePresenter.Close(viewModel);
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
