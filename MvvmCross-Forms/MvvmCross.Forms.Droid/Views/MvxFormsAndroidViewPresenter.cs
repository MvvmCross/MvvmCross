// MvxFormsDroidPagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using Xamarin.Forms;

namespace MvvmCross.Forms.Droid.Views
{
    public class MvxFormsAndroidViewPresenter
        : MvxAppCompatViewPresenter, IMvxFormsViewPresenter
    {
		public MvxFormsAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
		{
		}

        public MvxFormsAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies, MvxFormsApplication formsApplication) : this(androidViewAssemblies)
        {
            FormsApplication = formsApplication ?? throw new ArgumentNullException(nameof(formsApplication), "MvxFormsApplication cannot be null");
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

			AttributeTypesToActionsDictionary.Add(
                typeof(MvxCarouselPagePresentationAttribute),
				new MvxPresentationAttributeAction
				{
                ShowAction = (view, attribute, request) => ShowCarouselPage(view, (MvxCarouselPagePresentationAttribute)attribute, request),
                CloseAction = (viewModel, attribute) => CloseCarouselPage(viewModel, (MvxCarouselPagePresentationAttribute)attribute)
				});

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxContentPagePresentationAttribute),
                new MvxPresentationAttributeAction
                {
                ShowAction = (view, attribute, request) => ShowContentPage(view, (MvxContentPagePresentationAttribute)attribute, request),
                CloseAction = (viewModel, attribute) => CloseContentPage(viewModel, (MvxContentPagePresentationAttribute)attribute)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxMasterDetailPagePresentationAttribute),
                new MvxPresentationAttributeAction
                {
                ShowAction = (view, attribute, request) => ShowMasterDetailPage(view, (MvxMasterDetailPagePresentationAttribute)attribute, request),
                CloseAction = (viewModel, attribute) => CloseMasterDetailPage(viewModel, (MvxMasterDetailPagePresentationAttribute)attribute)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxModalPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                ShowAction = (view, attribute, request) => ShowModal(view, (MvxModalPresentationAttribute)attribute, request),
                CloseAction = (viewModel, attribute) => CloseModal(viewModel, (MvxModalPresentationAttribute)attribute)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxNavigationPagePresentationAttribute),
                new MvxPresentationAttributeAction
                {
                ShowAction = (view, attribute, request) => ShowNavigationPage(view, (MvxNavigationPagePresentationAttribute)attribute, request),
                CloseAction = (viewModel, attribute) => CloseNavigationPage(viewModel, (MvxNavigationPagePresentationAttribute)attribute)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxTabbedPagePresentationAttribute),
                new MvxPresentationAttributeAction
                {
                ShowAction = (view, attribute, request) => ShowTabbedPage(view, (MvxTabbedPagePresentationAttribute)attribute, request),
                CloseAction = (viewModel, attribute) => CloseTabbedPage(viewModel, (MvxTabbedPagePresentationAttribute)attribute)
                });
        }

        protected override MvxBasePresentationAttribute GetAttributeForViewModel(Type viewModelType)
        {
			IList<MvxBasePresentationAttribute> attributes;
            if (ViewModelToPresentationAttributeMap.TryGetValue(viewModelType, out attributes))
            {
                MvxBasePresentationAttribute attribute = attributes.FirstOrDefault();
				if (attribute.ViewType?.GetInterfaces().OfType<IMvxOverridePresentationAttribute>().FirstOrDefault() is IMvxOverridePresentationAttribute view)
				{
					var presentationAttribute = view.PresentationAttribute();

					if (presentationAttribute != null)
						return presentationAttribute;
				}
				return attribute;
            }

            return CreateAttributeForViewModel(viewModelType);
        }

        protected override MvxBasePresentationAttribute CreateAttributeForViewModel(Type viewModelType)
        {
            var viewType = ViewsContainer.GetViewType(viewModelType);
            if (viewType.IsSubclassOf(typeof(ContentPage)))
            {
                MvxTrace.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming ContentPage presentation");
                return new MvxContentPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.IsSubclassOf(typeof(CarouselPage)))
            {
                MvxTrace.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming CarouselPage presentation");
                return new MvxCarouselPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.IsSubclassOf(typeof(TabbedPage)))
            {
                MvxTrace.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming TabbedPage presentation");
                return new MvxTabbedPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.IsSubclassOf(typeof(MasterDetailPage)))
            {
                MvxTrace.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming MasterDetailPage presentation");
                return new MvxMasterDetailPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.IsSubclassOf(typeof(NavigationPage)))
            {
                MvxTrace.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming NavigationPage presentation");
                return new MvxNavigationPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }

            return base.CreateAttributeForViewModel(viewModelType);
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

            if(attribute.WrapInNavigationPage && (FormsApplication.MainPage == null || FormsApplication.MainPage.GetType() != typeof(MvxNavigationPage)))
            {
                FormsApplication.MainPage = new MvxNavigationPage(page);
            }
            else if(attribute.WrapInNavigationPage && FormsApplication.MainPage is MvxNavigationPage navigationPage)
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
