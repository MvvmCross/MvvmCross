// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Views;
using MvvmCross.Logging;
using MvvmCross.Platform.Ios.Presenters;
using MvvmCross.ViewModels;
using UIKit;
using Xamarin.Forms;

namespace MvvmCross.Forms.Platform.iOS.Views
{
    public class MvxFormsIosViewPresenter
        : MvxIosViewPresenter
        , IMvxFormsViewPresenter
    {
        public MvxFormsIosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window, MvxFormsApplication formsApplication) : base (applicationDelegate, window)
        {
            FormsApplication = formsApplication ?? throw new ArgumentNullException(nameof(formsApplication), "MvxFormsApplication cannot be null");
        }

        private MvxFormsApplication _formsApplication;
        public MvxFormsApplication FormsApplication
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

            if (_window.RootViewController == null)
                SetWindowRootViewController(FormsApplication.MainPage.CreateViewController());
        }

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();
            FormsPagePresenter.RegisterAttributeTypes();
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            FormsPagePresenter.ChangePresentation(hint);
        }

        public override void Close(IMvxViewModel viewModel)
        {
            FormsPagePresenter.Close(viewModel);
        }

        public virtual bool ShowPlatformHost(Type hostViewModel = null)
        {
            MvxFormsLog.Instance.Trace($"Showing of native host View in Forms is not supported.");
            return false;
        }

        public virtual bool ClosePlatformViews()
        {
            CloseMasterNavigationController();
            CloseModalViewControllers();
            CloseTabBarViewController();
            CloseSplitViewController();
            return true;
        }
    }
}
