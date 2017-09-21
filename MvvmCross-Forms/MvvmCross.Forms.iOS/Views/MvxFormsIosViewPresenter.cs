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
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform.Platform;
using UIKit;
using Xamarin.Forms;

namespace MvvmCross.Forms.iOS.Presenters
{
    public class MvxFormsIosViewPresenter
        : MvxIosViewPresenter
        , IMvxFormsViewPresenter
    {
        public MvxFormsIosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate, window)
        {
        }

        public MvxFormsIosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window, MvxFormsApplication formsApplication) : this(applicationDelegate, window)
        {
            FormsApplication = formsApplication ?? throw new ArgumentNullException(nameof(formsApplication), "MvxFormsApplication cannot be null");
            FormsApplication.MainPage = new MvxNavigationPage();
        }

        private MvxFormsApplication _formsApplication;
        public MvxFormsApplication FormsApplication
        {
            get { return _formsApplication; }
            set { _formsApplication = value; }
        }

        protected override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();

            AttributeTypesToShowMethodDictionary.Add(
                typeof(MvxCarouselPagePresentationAttribute),
                (vc, attribute, request) => ShowCarouselPage(vc.GetType(), (MvxCarouselPagePresentationAttribute)attribute, request));

            AttributeTypesToShowMethodDictionary.Add(
                typeof(MvxContentPagePresentationAttribute),
                (vc, attribute, request) => ShowContentPage(vc.GetType(), (MvxContentPagePresentationAttribute)attribute, request));

            AttributeTypesToShowMethodDictionary.Add(
                typeof(MvxMasterDetailPagePresentationAttribute),
                (vc, attribute, request) => ShowMasterDetailPage(vc.GetType(), (MvxMasterDetailPagePresentationAttribute)attribute, request));

            AttributeTypesToShowMethodDictionary.Add(
                typeof(MvxModalPresentationAttribute),
                (vc, attribute, request) => ShowModal(vc.GetType(), (MvxModalPresentationAttribute)attribute, request));


            AttributeTypesToShowMethodDictionary.Add(
                typeof(MvxNavigationPagePresentationAttribute),
                (vc, attribute, request) => ShowNavigationPage(vc.GetType(), (MvxNavigationPagePresentationAttribute)attribute, request));

            AttributeTypesToShowMethodDictionary.Add(
                typeof(MvxTabbedPagePresentationAttribute),
                (vc, attribute, request) => ShowTabbedPage(vc.GetType(), (MvxTabbedPagePresentationAttribute)attribute, request));
        }

        protected override MvxBasePresentationAttribute GetPresentationAttributes(UIViewController viewController)
        {
            return base.GetPresentationAttributes(viewController);
        }

        public override void Show(MvxViewModelRequest request)
        {
            //TODO: Find a way to show Forms views

            base.Show(request);
        }

        protected virtual Page CreatePage(Type viewType, MvxViewModelRequest request)
        {
            var page = Activator.CreateInstance(viewType) as Page;

            if (page is IMvxPage contentPage)
            {
                if (request is MvxViewModelInstanceRequest instanceRequest)
                {
                    contentPage.ViewModel = instanceRequest.ViewModelInstance;
                }
                else
                {
                    contentPage.ViewModel = MvxPresenterHelpers.LoadViewModel(request);
                }
            }
            return page;
        }

        protected virtual void ShowCarouselPage(
            Type view,
            MvxCarouselPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CreatePage(view, request) as CarouselPage;
            FormsApplication.MainPage = page;
        }

        protected virtual bool CloseCarouselPage(IMvxViewModel viewModel, MvxCarouselPagePresentationAttribute attribute)
        {
            return false;
        }

        protected virtual void ShowContentPage(
            Type view,
            MvxContentPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CreatePage(view, request);

            if (attribute.WrapInNavigationPage && (FormsApplication.MainPage == null || FormsApplication.MainPage.GetType() != typeof(MvxNavigationPage)))
            {
                FormsApplication.MainPage = new MvxNavigationPage(page);
            }
            else if (attribute.WrapInNavigationPage && FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                navigationPage.PushAsync(page, attribute.Animated);
            }
            else
            {
                FormsApplication.MainPage = page;
            }
        }

        protected virtual bool CloseContentPage(IMvxViewModel viewModel, MvxContentPagePresentationAttribute attribute)
        {
            if (FormsApplication.MainPage is MvxNavigationPage navigationPage)
                navigationPage.PopAsync(attribute.Animated);
            return true;
        }

        protected virtual void ShowMasterDetailPage(
            Type view,
            MvxMasterDetailPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CreatePage(view, request) as MasterDetailPage;
            FormsApplication.MainPage = page;
        }

        protected virtual bool CloseMasterDetailPage(IMvxViewModel viewModel, MvxMasterDetailPagePresentationAttribute attribute)
        {
            if (FormsApplication.MainPage is MvxNavigationPage navigationPage)
                navigationPage.PopAsync();
            return true;
        }

        protected virtual void ShowModal(
            Type view,
            MvxModalPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CreatePage(view, request);

            if (FormsApplication.MainPage is MvxNavigationPage navigationPage)
                navigationPage.CurrentPage.Navigation.PushModalAsync(page);
        }

        protected virtual bool CloseModal(IMvxViewModel viewModel, MvxModalPresentationAttribute attribute)
        {
            if (FormsApplication.MainPage is MvxNavigationPage navigationPage)
                navigationPage.CurrentPage.Navigation.PopModalAsync();
            return true;
        }

        protected virtual void ShowNavigationPage(
            Type view,
            MvxNavigationPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CreatePage(view, request);
            FormsApplication.MainPage = page;
        }

        protected virtual bool CloseNavigationPage(IMvxViewModel viewModel, MvxNavigationPagePresentationAttribute attribute)
        {
            return true;
        }

        protected virtual void ShowTabbedPage(
            Type view,
            MvxTabbedPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CreatePage(view, request) as TabbedPage;
            FormsApplication.MainPage = page;
        }

        protected virtual bool CloseTabbedPage(IMvxViewModel viewModel, MvxTabbedPagePresentationAttribute attribute)
        {
            return true;
        }
    }
}