using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Views.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    //Handles common Forms Presenter code
    public class MvxFormsPagePresenter : IMvxFormsPagePresenter
    {
        public MvxFormsPagePresenter(MvxFormsApplication formsApplication, IMvxViewsContainer viewsContainer = null, IMvxViewModelTypeFinder viewModelTypeFinder = null, IMvxViewModelLoader viewModelLoader = null)
        {
            FormsApplication = formsApplication;
            ViewModelLoader = viewModelLoader;
            ViewsContainer = viewsContainer;
            ViewModelTypeFinder = viewModelTypeFinder;
        }

        private MvxFormsApplication _formsApplication;
        public MvxFormsApplication FormsApplication
        {
            get { return _formsApplication; }
            set { _formsApplication = value; }
        }

        private IMvxViewModelLoader _viewModelLoader;
        public IMvxViewModelLoader ViewModelLoader
        {
            get
            {
                if (_viewModelLoader == null)
                    _viewModelLoader = Mvx.Resolve<IMvxViewModelLoader>();
                return _viewModelLoader;
            }
            set
            {
                _viewModelLoader = value;
            }
        }

        private IMvxViewsContainer _viewsContainer;
        public IMvxViewsContainer ViewsContainer
        {
            get
            {
                if (_viewsContainer == null)
                    _viewsContainer = Mvx.Resolve<IMvxViewsContainer>();
                return _viewsContainer;
            }
            set
            {
                _viewsContainer = value;
            }
        }

        private IMvxViewModelTypeFinder _viewModelTypeFinder;
        public IMvxViewModelTypeFinder ViewModelTypeFinder
        {
            get
            {
                if (_viewModelTypeFinder == null)
                    _viewModelTypeFinder = Mvx.Resolve<IMvxViewModelTypeFinder>();
                return _viewModelTypeFinder;
            }
            set
            {
                _viewModelTypeFinder = value;
            }
        }

        public virtual Func<bool> ClosePlatformViews { get; set; }

        public virtual Page CreatePage(Type viewType, MvxViewModelRequest request, MvxBasePresentationAttribute attribute)
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
                    contentPage.ViewModel = ViewModelLoader.LoadViewModel(request, null);
                }
            }

            if(attribute is MvxPagePresentationAttribute pageAttribute)
            {
                if (string.IsNullOrEmpty(page.Title) && !string.IsNullOrEmpty(pageAttribute.Title))
                    page.Title = pageAttribute.Title;
                if (string.IsNullOrEmpty(page.Icon) && !string.IsNullOrEmpty(pageAttribute.Icon))
                    page.Icon = pageAttribute.Icon;
            }

            return page;
        }

        public virtual void RegisterAttributeTypes(Dictionary<Type, MvxPresentationAttributeAction> AttributeTypesToActionsDictionary)
        {
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

        public virtual void ShowCarouselPage(
            Type view,
            MvxCarouselPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            CloseAllModals();
            ClosePlatformViews?.Invoke();

            var page = CreatePage(view, request, attribute);

            if (attribute.Position == CarouselPosition.Root)
            {
                if (page is CarouselPage carouselPageRoot)
                {
                    if (attribute.WrapInNavigationPage && FormsApplication.MainPage is MvxNavigationPage currentPage)
                    {
                        if (attribute.NoHistory)
                            currentPage.PopToRootAsync(attribute.Animated);
                        currentPage.PushAsync(page, attribute.Animated);
                    }
                    else if (attribute.WrapInNavigationPage)
                        ReplaceRoot(new MvxNavigationPage(page));
                    else
                        ReplaceRoot(page);
                }
                else
                    throw new MvxException($"A root page should be of type {nameof(CarouselPage)}");
            }
            else
            {
                var carouselHost = FormsApplication.MainPage as CarouselPage;
                if (carouselHost == null && FormsApplication.MainPage is MvxNavigationPage navigationPage)
                {
                    carouselHost = navigationPage.CurrentPage as CarouselPage;
                    if (carouselHost == null)
                    {
                        MvxTrace.Trace($"Current root is not a CarouselPage show your own first to use custom Host. Assuming we need to create one.");
                        carouselHost = new MvxCarouselPage();
                        navigationPage.PushAsync(carouselHost, attribute.Animated);
                    }
                }
                else if (carouselHost == null)
                {
                    MvxTrace.Trace($"Current root is not a CarouselPage show your own first to use custom Host. Assuming we need to create one.");
                    carouselHost = new MvxCarouselPage();
                    FormsApplication.MainPage = carouselHost;
                }

                carouselHost.Children.Add(page as ContentPage);
            }
        }

        public virtual bool CloseCarouselPage(IMvxViewModel viewModel, MvxCarouselPagePresentationAttribute attribute)
        {
            if (attribute.Position == CarouselPosition.Root)
            {
                if (FormsApplication.MainPage is MvxNavigationPage rootNavigationPage)
                    rootNavigationPage.PopAsync(attribute.Animated);
                return true;
            }
            else
            {
                var carouselHost = FormsApplication.MainPage as CarouselPage;
                if (carouselHost == null && FormsApplication.MainPage is MvxNavigationPage navigationPage)
                {
                    carouselHost = navigationPage.CurrentPage as CarouselPage;
                }
                var page = carouselHost.Children.OfType<IMvxPage>().FirstOrDefault(x => x.ViewModel == viewModel);
                if (page is ContentPage carouselPage)
                    carouselHost.Children.Remove(carouselPage);
                else
                    return false;
                return true;
            }
        }

        public virtual void ShowContentPage(
            Type view,
            MvxContentPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            CloseAllModals();
            ClosePlatformViews?.Invoke();

            var page = CreatePage(view, request, attribute);

            if (attribute.WrapInNavigationPage && FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                if (attribute.NoHistory)
                    navigationPage.PopToRootAsync(attribute.Animated);
                navigationPage.PushAsync(page, attribute.Animated);
            }
            else if (attribute.WrapInNavigationPage)
            {
                ReplaceRoot(new MvxNavigationPage(page));
            }
            else
            {
                ReplaceRoot(page);
            }
        }

        public virtual bool CloseContentPage(IMvxViewModel viewModel, MvxContentPagePresentationAttribute attribute)
        {
            if (FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                if (navigationPage.CurrentPage is MvxNavigationPage currentNavigationPage)
                {
                    currentNavigationPage.PopAsync(attribute.Animated);
                }
                else
                {
                    navigationPage.PopAsync(attribute.Animated);
                }
            }
            return true;
        }

        public virtual void ShowMasterDetailPage(
            Type view,
            MvxMasterDetailPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            CloseAllModals();
            ClosePlatformViews?.Invoke();

            var page = CreatePage(view, request, attribute);

            if(attribute.Position == MasterDetailPosition.Root)
            {
                if (page is MasterDetailPage masterDetailRoot)
                {
                    if (masterDetailRoot.Master == null)
                        masterDetailRoot.Master = new MvxContentPage() { Title = !string.IsNullOrEmpty(attribute.Title) ? attribute.Title : "MvvmCross"  };
                    if (masterDetailRoot.Detail == null)
                        masterDetailRoot.Detail = new MvxContentPage() { Title = !string.IsNullOrEmpty(attribute.Title) ? attribute.Title : "MvvmCross" };

                    if (attribute.WrapInNavigationPage && FormsApplication.MainPage is MvxNavigationPage currentNavigationPage)
                    {
                        if (attribute.NoHistory)
                            currentNavigationPage.PopToRootAsync(attribute.Animated);
                        currentNavigationPage.PushAsync(page, attribute.Animated);
                    }
                    else if (attribute.WrapInNavigationPage)
                        ReplaceRoot(new MvxNavigationPage(page));
                    else
                        ReplaceRoot(page);
                }
                else
                    throw new MvxException($"A root page should be of type {nameof(MasterDetailPage)}");
            }
            else
            {
                var masterDetailHost = FormsApplication.MainPage as MasterDetailPage;
                if (masterDetailHost == null && FormsApplication.MainPage is MvxNavigationPage navigationPage)
                {
                    masterDetailHost = navigationPage.CurrentPage as MasterDetailPage;
                    if (masterDetailHost == null)
                    {
                        masterDetailHost = new MasterDetailPage();
                        masterDetailHost.Master = new MvxContentPage() { Title = !string.IsNullOrEmpty(attribute.Title) ? attribute.Title : "MvvmCross" };
                        masterDetailHost.Detail = new MvxContentPage() { Title = !string.IsNullOrEmpty(attribute.Title) ? attribute.Title : "MvvmCross" };
                        if (attribute.NoHistory)
                            navigationPage.PopToRootAsync(attribute.Animated);
                        navigationPage.PushAsync(masterDetailHost, attribute.Animated);
                    }
                }
                else if (masterDetailHost == null)
                {
                    //Assume we have to create the host
                    masterDetailHost = new MasterDetailPage();
                    masterDetailHost.Master = new MvxContentPage() { Title = !string.IsNullOrEmpty(attribute.Title) ? attribute.Title : "MvvmCross" };
                    masterDetailHost.Detail = new MvxContentPage() { Title = !string.IsNullOrEmpty(attribute.Title) ? attribute.Title : "MvvmCross" };
                    FormsApplication.MainPage = masterDetailHost;
                }
                if(attribute.Position == MasterDetailPosition.Master)
                {
                    if (attribute.WrapInNavigationPage && masterDetailHost.Master is MvxNavigationPage navigationMasterPage)
                    {
                        if (attribute.NoHistory)
                            navigationMasterPage.PopToRootAsync(attribute.Animated);
                        navigationMasterPage.PushAsync(page, attribute.Animated);
                    }
                    else if (attribute.WrapInNavigationPage)
                        masterDetailHost.Master = new MvxNavigationPage(page) { Title = !string.IsNullOrEmpty(attribute.Title) ? attribute.Title : "MvvmCross" };
                    else
                        masterDetailHost.Master = page;
                }
                else
                {
                    if (attribute.WrapInNavigationPage && masterDetailHost.Detail is MvxNavigationPage navigationDetailPage)
                    {
                        if (attribute.NoHistory)
                            navigationDetailPage.PopToRootAsync(attribute.Animated);
                        navigationDetailPage.PushAsync(page, attribute.Animated);
                    }
                    else if (attribute.WrapInNavigationPage)
                        masterDetailHost.Detail = new MvxNavigationPage(page);
                    else
                        masterDetailHost.Detail = page;
                }
            }
        }

        public virtual bool CloseMasterDetailPage(IMvxViewModel viewModel, MvxMasterDetailPagePresentationAttribute attribute)
        {
            var masterDetailHost = FormsApplication.MainPage as MasterDetailPage;
            if (masterDetailHost == null && FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                masterDetailHost = navigationPage.CurrentPage as MasterDetailPage;
            }

            switch (attribute.Position)
            {
                case MasterDetailPosition.Root:
                    if (FormsApplication.MainPage is MvxNavigationPage rootNavigationPage)
                        rootNavigationPage.PopAsync(attribute.Animated);
                    break;
                case MasterDetailPosition.Master:
                    if (masterDetailHost.Master is NavigationPage navigationMasterPage)
                        navigationMasterPage.PopAsync(attribute.Animated);
                    break;
                case MasterDetailPosition.Detail:
                    if (masterDetailHost.Detail is NavigationPage navigationDetailPage)
                        navigationDetailPage.PopAsync(attribute.Animated);
                    break;
                default:
                    break;
            }
            return true;
        }

        public virtual void ShowModal(
            Type view,
            MvxModalPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ClosePlatformViews?.Invoke();

            var page = CreatePage(view, request, attribute);

            if (FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                if (attribute.WrapInNavigationPage && navigationPage?.Navigation?.ModalStack?.LastOrDefault() is MvxNavigationPage modalNavigationPage)
                {
                    if (attribute.NoHistory)
                        modalNavigationPage.PopToRootAsync(attribute.Animated);
                    modalNavigationPage.PushAsync(page, attribute.Animated);
                }
                else if (attribute.WrapInNavigationPage)
                    navigationPage.Navigation.PushModalAsync(new MvxNavigationPage(page), attribute.Animated);
                else
                    navigationPage.Navigation.PushModalAsync(page, attribute.Animated);
            }
            else
            {
                if (attribute.WrapInNavigationPage && FormsApplication.MainPage?.Navigation?.ModalStack?.LastOrDefault() is MvxNavigationPage modalNavigationPage)
                    modalNavigationPage.PushAsync(page, attribute.Animated);
                else if (attribute.WrapInNavigationPage && FormsApplication.MainPage?.Navigation != null)
                    FormsApplication.MainPage.Navigation.PushModalAsync(new MvxNavigationPage(page), attribute.Animated);
                else
                {
                    if (FormsApplication.MainPage?.Navigation == null)
                        FormsApplication.MainPage = new MvxNavigationPage(new MvxPage());
                    FormsApplication.MainPage.Navigation.PushModalAsync(page, attribute.Animated);
                }
            }
        }

        public virtual bool CloseModal(IMvxViewModel viewModel, MvxModalPresentationAttribute attribute)
        {
            if (FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                if (attribute.WrapInNavigationPage && navigationPage?.Navigation?.ModalStack?.LastOrDefault() is MvxNavigationPage modalNavigationPage && modalNavigationPage?.Navigation?.NavigationStack?.Count > 1)
                    modalNavigationPage.PopAsync(attribute.Animated);
                else
                    navigationPage.Navigation.PopModalAsync(attribute.Animated);
            }
            else
            {
                if (attribute.WrapInNavigationPage && FormsApplication.MainPage?.Navigation?.ModalStack?.LastOrDefault() is MvxNavigationPage modalNavigationPage)
                    modalNavigationPage.PopAsync(attribute.Animated);
                else
                    FormsApplication.MainPage?.Navigation?.PopModalAsync(attribute.Animated);
            }
            return true;
        }

        public virtual void ShowNavigationPage(
            Type view,
            MvxNavigationPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            CloseAllModals();
            ClosePlatformViews?.Invoke();

            var page = CreatePage(view, request, attribute);

            if(attribute.NoHistory)
                FormsApplication.MainPage = page;
            else if (attribute.WrapInNavigationPage && FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                if (navigationPage.CurrentPage is MvxNavigationPage currentNavigationPage)
                {
                    currentNavigationPage.PushAsync(page, attribute.Animated);
                }
                else
                    navigationPage.PushAsync(page, attribute.Animated);
                    
            }
        }

        public virtual bool CloseNavigationPage(IMvxViewModel viewModel, MvxNavigationPagePresentationAttribute attribute)
        {
            if (FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                if (navigationPage.CurrentPage is MvxNavigationPage currentNavigationPage)
                {
                    currentNavigationPage.PopAsync(attribute.Animated);
                }
                else
                {
                    navigationPage.PopAsync(attribute.Animated);
                }
            }
            return true;
        }

        public virtual void ShowTabbedPage(
            Type view,
            MvxTabbedPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            CloseAllModals();
            ClosePlatformViews?.Invoke();

            var page = CreatePage(view, request, attribute);

            if(attribute.Position == TabbedPosition.Root)
            {
                if (page is TabbedPage tabbedPageRoot)
                {
                    if (attribute.WrapInNavigationPage && FormsApplication.MainPage is MvxNavigationPage currentPage)
                    {
                        if (attribute.NoHistory)
                            currentPage.PopToRootAsync(attribute.Animated);
                        currentPage.PushAsync(page, attribute.Animated);
                    }
                    else if (attribute.WrapInNavigationPage)
                        ReplaceRoot(new MvxNavigationPage(page));
                    else
                        ReplaceRoot(page);
                }
                else
                    throw new MvxException($"A root page should be of type {nameof(TabbedPage)}");
            }
            else
            {
                var tabHost = FormsApplication.MainPage as TabbedPage;
                if (tabHost == null && FormsApplication.MainPage is MvxNavigationPage navigationPage)
                {
                    tabHost = navigationPage.CurrentPage as TabbedPage;
                    if (tabHost == null)
                    {
                        MvxTrace.Trace($"Current root is not a TabbedPage show your own first to use custom Host. Assuming we need to create one.");
                        tabHost = new MvxTabbedPage();
                        navigationPage.PushAsync(tabHost, attribute.Animated);
                    }
                }
                else if (FormsApplication.MainPage is MasterDetailPage masterDetailPage)
                {
                    tabHost = masterDetailPage.Detail as TabbedPage;
                    if (tabHost == null && masterDetailPage.Detail is MvxNavigationPage detailNavigationPage)
                    {
                        tabHost = detailNavigationPage.CurrentPage as TabbedPage;
                    }
                }
                else if (tabHost == null)
                {
                    MvxTrace.Trace($"Current root is not a TabbedPage show your own first to use custom Host. Assuming we need to create one.");
                    tabHost = new MvxTabbedPage();
                    FormsApplication.MainPage = tabHost;
                }

                tabHost.Children.Add(page);
            }
        }

        public virtual bool CloseTabbedPage(IMvxViewModel viewModel, MvxTabbedPagePresentationAttribute attribute)
        {
            var tabHost = FormsApplication.MainPage as TabbedPage;
            if (tabHost == null && FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                tabHost = navigationPage.CurrentPage as TabbedPage;
            }
            var page = tabHost.Children.OfType<IMvxPage>().FirstOrDefault(x => x.ViewModel == viewModel);
            if (page is Page tabPage)
                tabHost.Children.Remove(tabPage);
            else
                return false;
            return true;
        }

        public virtual void CloseAllModals()
        {
            if (FormsApplication.MainPage != null)
            {
                if (FormsApplication.MainPage is MvxNavigationPage rootNavigationPage && rootNavigationPage.CurrentPage is MvxNavigationPage nestedNavigationPage)
                {
                    CloseModalStack(nestedNavigationPage.Navigation.ModalStack);
                }
                else if (FormsApplication.MainPage is MvxNavigationPage navigationPage)
                {
                    CloseModalStack(navigationPage.Navigation.ModalStack);
                }
                else if (FormsApplication.MainPage.Navigation?.ModalStack?.Count > 0)
                {
                    CloseModalStack(FormsApplication.MainPage.Navigation.ModalStack);
                }
            }
        }
       
        protected virtual void CloseModalStack(IReadOnlyList<Page> modals)
        {
            var modalList = modals.ToList();
            if (modalList != null && modalList.Count > 0)
            {
                foreach (var modal in modalList)
                {
                    modal.Navigation.PopModalAsync();
                }
            }
        }

        public virtual void ReplaceRoot(Page page)
        {
            if(FormsApplication.MainPage == null)
                FormsApplication.MainPage = page;
            else if(FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                navigationPage.Navigation.InsertPageBefore(page, navigationPage.CurrentPage);
                navigationPage.Navigation.PopToRootAsync();
            }
            else
            {
                //This may fail
                FormsApplication.MainPage = page;
            }
        }
    }
}
