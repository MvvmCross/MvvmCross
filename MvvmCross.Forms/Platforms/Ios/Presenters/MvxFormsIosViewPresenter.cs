// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Presenters;
using MvvmCross.Logging;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.ViewModels;
using UIKit;
using Xamarin.Forms;

namespace MvvmCross.Forms.Platforms.Ios.Presenters
{
    public class MvxFormsIosViewPresenter
        : MvxIosViewPresenter
        , IMvxFormsViewPresenter
    {
        public MvxFormsIosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window, Application formsApplication) : base (applicationDelegate, window)
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

        public override async Task<bool> Show(MvxViewModelRequest request)
        {
            if (!await FormsPagePresenter.Show(request)) return false;

            if (_window.RootViewController == null)
                SetWindowRootViewController(FormsApplication.MainPage.CreateViewController());
            return true;
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
