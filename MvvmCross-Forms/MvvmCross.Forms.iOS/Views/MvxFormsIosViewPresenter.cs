// MvxFormsIosPagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using UIKit;
using Xamarin.Forms;

namespace MvvmCross.Forms.iOS.Presenters
{
    public class MvxFormsIosViewPresenter
        : MvxIosViewPresenter
        , IMvxFormsViewPresenter
    {
        public MvxFormsIosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window, Application formsApplication) : base (applicationDelegate, window)
        {
            FormsApplication = formsApplication ?? throw new ArgumentNullException(nameof(formsApplication), "Application cannot be null");
        }
        public Xamarin.Forms.Application FormsApplication { get; set; }

        private IMvxFormsPagePresenter _formsPagePresenter;
        public virtual IMvxFormsPagePresenter FormsPagePresenter
        {
            get
            {
                if (_formsPagePresenter == null)
                {
                    _formsPagePresenter = new MvxFormsPagePresenter(FormsApplication, ViewsContainer, ViewModelTypeFinder, attributeTypesToActionsDictionary: AttributeTypesToActionsDictionary);
                    _formsPagePresenter.ClosePlatformViews = ClosePlatformViews;
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

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            var presentationAttribute = FormsPagePresenter.CreatePresentationAttribute(viewModelType, viewType);
            return presentationAttribute ?? base.CreatePresentationAttribute(viewModelType, viewType);
        }

        public virtual bool ClosePlatformViews()
        {
            CloseMasterNavigationController();
            CleanupModalViewControllers();
            CloseTabBarViewController();
            CloseSplitViewController();
            return true;
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            FormsPagePresenter.ChangePresentation(hint);
        }

        public override void Close(IMvxViewModel viewModel)
        {
            FormsPagePresenter.Close(viewModel);
        }
    }
}