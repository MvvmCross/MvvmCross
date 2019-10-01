﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MvvmCross.Exceptions;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Logging;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Xamarin.Forms;

namespace MvvmCross.Forms.Presenters
{
    //Handles common Forms Presenter code
    public class MvxFormsPagePresenter :
        MvxAttributeViewPresenter, IMvxFormsPagePresenter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MvvmCross.Forms.Views.MvxFormsPagePresenter"/> class.
        /// </summary>
        /// <param name="platformPresenter">The native platform presenter from where the MvxFormsPagePresenter is created</param>
        public MvxFormsPagePresenter(IMvxFormsViewPresenter platformPresenter)
        {
            PlatformPresenter = platformPresenter;
        }

        protected IMvxFormsViewPresenter PlatformPresenter { get; }

        private Application _formsApplication;
        public Application FormsApplication
        {
            get
            {
                if (_formsApplication == null)
                    _formsApplication = PlatformPresenter.FormsApplication;
                return _formsApplication;
            }
            set { _formsApplication = value; }
        }

        private IMvxViewModelLoader _viewModelLoader;
        public IMvxViewModelLoader ViewModelLoader
        {
            get
            {
                if (_viewModelLoader == null)
                    _viewModelLoader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
                return _viewModelLoader;
            }
            set
            {
                _viewModelLoader = value;
            }
        }

        public override IMvxViewsContainer ViewsContainer
        {
            get
            {
                if (_viewsContainer == null)
                    _viewsContainer = PlatformPresenter.ViewsContainer;
                return base.ViewsContainer;
            }
            set => base.ViewsContainer = value;
        }

        public override IMvxViewModelTypeFinder ViewModelTypeFinder
        {
            get
            {
                if (_viewModelTypeFinder == null)
                    _viewModelTypeFinder = PlatformPresenter.ViewModelTypeFinder;
                return base.ViewModelTypeFinder;
            }
            set => base.ViewModelTypeFinder = value;
        }

        public override IDictionary<Type, MvxPresentationAttributeAction> AttributeTypesToActionsDictionary
        {
            get
            {
                if (_attributeTypesActionsDictionary == null)
                {
                    _attributeTypesActionsDictionary = PlatformPresenter.AttributeTypesToActionsDictionary;
                }
                return _attributeTypesActionsDictionary;
            }
            set => base.AttributeTypesToActionsDictionary = value;
        }

        public virtual Page CreatePage(Type viewType, MvxViewModelRequest request, MvxBasePresentationAttribute attribute)
        {
            var page = Activator.CreateInstance(viewType) as Page;

            if (page is IMvxPage contentPage)
            {
                if (request is MvxViewModelInstanceRequest instanceRequest)
                    contentPage.ViewModel = instanceRequest.ViewModelInstance;
                else
                    contentPage.ViewModel = ViewModelLoader.LoadViewModel(request, null);
            }

            if (attribute is MvxPagePresentationAttribute pageAttribute)
            {
                if (string.IsNullOrEmpty(page.Title) && !string.IsNullOrEmpty(pageAttribute.Title))
                    page.Title = pageAttribute.Title;
                if (string.IsNullOrEmpty(page.Icon) && !string.IsNullOrEmpty(pageAttribute.Icon))
                    page.Icon = pageAttribute.Icon;
            }

            return page;
        }

        protected virtual async Task<Page> CloseAndCreatePage(Type view,
            MvxViewModelRequest request,
            MvxPagePresentationAttribute attribute,
            bool closeModal = true,
            bool closePlatformViews = true,
            bool showPlatformViews = true)
        {
            if (closeModal)
                await CloseAllModals();

            if (closePlatformViews)
                PlatformPresenter.ClosePlatformViews();

            if (showPlatformViews)
                PlatformPresenter.ShowPlatformHost(attribute.HostViewModelType);

            return CreatePage(view, request, attribute);
        }

        protected virtual ContentPage CreateContentPage()
        {
            return new ContentPage();
        }

        protected virtual NavigationPage CreateNavigationPage(Page pageRoot = null)
        {
            return new NavigationPage(pageRoot);
        }

        protected virtual MasterDetailPage CreateMasterDetailPage()
        {
            return new MasterDetailPage();
        }

        protected virtual TabbedPage CreateTabbedPage()
        {
            return new TabbedPage();
        }

        protected virtual CarouselPage CreateCarouselPage()
        {
            return new CarouselPage();
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Register<MvxCarouselPagePresentationAttribute>(ShowCarouselPage, CloseCarouselPage);
            AttributeTypesToActionsDictionary.Register<MvxContentPagePresentationAttribute>(ShowContentPage, CloseContentPage);
            AttributeTypesToActionsDictionary.Register<MvxMasterDetailPagePresentationAttribute>(ShowMasterDetailPage, CloseMasterDetailPage);
            AttributeTypesToActionsDictionary.Register<MvxModalPresentationAttribute>(ShowModal, CloseModal);
            AttributeTypesToActionsDictionary.Register<MvxNavigationPagePresentationAttribute>(ShowNavigationPage, CloseNavigationPage);
            AttributeTypesToActionsDictionary.Register<MvxTabbedPagePresentationAttribute>(ShowTabbedPage, CloseTabbedPage);
        }

        public async override Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
#if DEBUG // Only wrap in try-finally when in debug
            try
            {
#endif
                var navigation = GetPageOfType<NavigationPage>()?.Navigation;
                if (hint is MvxPopToRootPresentationHint popToRootHint)
                {
                    // Make sure all modals are closed
                    await CloseAllModals(popToRootHint.Animated);

                    // Double check we have a navigation page, otherwise
                    // we can just return as we must be already at the root page
                    if (navigation == null)
                        return true;

                    // Close all pages back to the root
                    await navigation.PopToRootAsync(popToRootHint.Animated);
                    return true;
                }
                if (hint is MvxPopPresentationHint popHint)
                {
                    var matched = await PopModalToViewModel(navigation, popHint);
                    if (matched) return true;


                    await PopToViewModel(navigation, popHint.ViewModelToPopTo, popHint.Animated);
                    return true;
                }
                if (hint is MvxRemovePresentationHint removeHint)
                {
                    foreach (var modal in navigation.ModalStack)
                    {
                        var removed = RemoveByViewModel(modal.Navigation, removeHint.ViewModelToRemove);
                        if (removed)
                            return true;
                    }

                    RemoveByViewModel(navigation, removeHint.ViewModelToRemove);
                    return true;
                }
                if (hint is MvxPagePresentationHint pageHint)
                {
                    var pageType = ViewsContainer.GetViewType(pageHint.ViewModel);
                    if (GetPageOfTypeByType(pageType) is Page page)
                    {
                        if (page.Parent is TabbedPage tabbedPage)
                            tabbedPage.CurrentPage = page;
                        else if (page.Parent is CarouselPage carouselPage && page is ContentPage contentPage)
                            carouselPage.CurrentPage = contentPage;
                    }
                    return true;
                }
                if (hint is MvxPopRecursivePresentationHint popRecursiveHint)
                {
                    var levels = popRecursiveHint.LevelsDeep;
                    if (levels > navigation.NavigationStack.Count())
                        levels = navigation.NavigationStack.Count();
                    for (int i = 0; i < levels; i++)
                    {
                        await navigation.PopAsync(popRecursiveHint.Animated);
                    }

                    return true;
                }

                return true;
#if DEBUG // Only showing this when debugging MVX
            }
            finally
            {
                MvxFormsLog.Instance.Trace(FormsApplication.Hierarchy());
            }
#endif
        }

        protected virtual bool RemoveByViewModel(INavigation navigation, Type viewModelToRemove)
        {
            var page = navigation.NavigationStack
                             .OfType<IMvxPage>()
                             .FirstOrDefault(view => view.ViewModel.GetType() == viewModelToRemove) as Page;
            if (page != null)
            {
                navigation.RemovePage(page);
                return true;
            }

            return false;
        }

        protected virtual async Task<bool> PopModalToViewModel(INavigation navigation, MvxPopPresentationHint popHint)
        {
            // Need to check the modal stack first
            while (navigation.ModalStack.Any())
            {
                var modalPage = navigation.ModalStack.Last();
                if (modalPage.IsViewModelTypeOf(popHint.ViewModelToPopTo))
                    return true;

                var modalNavPage = GetPageOfType<NavigationPage>(modalPage);
                if (modalNavPage != null)
                {
                    var matched = await PopToViewModel(modalNavPage.Navigation, popHint.ViewModelToPopTo, popHint.Animated);
                    if (matched)
                        return true;
                }

                await navigation.PopModalAsync();
            }

            return false;
        }

        protected virtual async Task<bool> PopToViewModel(INavigation navigation, Type viewModelType, bool animate = false)
        {
            while (navigation.NavigationStack.Any())
            {
                var page = navigation.NavigationStack.Last();
                if ((page as IMvxPage)?.ViewModel.GetType() == viewModelType)
                    return true;

                if (navigation.NavigationStack.Count > 1)
                {
                    await navigation.PopAsync(animate);
                }
                else
                {
                    // Don't try to pop the last page - this shouldn't be done! Instead
                    // break out of the while, and the whole modal will be popped.
                    break;
                }
            }

            return false;
        }

        public override Task<bool> Show(MvxViewModelRequest request)
        {
#if DEBUG // Only wrap in try-finally when in debug
            try
            {
#endif
                return base.Show(request);
#if DEBUG // Only showing this when debugging MVX
            }
            finally
            {
                MvxFormsLog.Instance.Trace(FormsApplication.Hierarchy());
            }
#endif
        }

        public virtual async Task<bool> ShowCarouselPage(
            Type view,
            MvxCarouselPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = await CloseAndCreatePage(view, request, attribute);

            if (attribute.Position == CarouselPosition.Root)
            {
                if (!(page is MvxCarouselPage carouselPageRoot))
                {
                    throw new MvxException($"A root page should be of type {nameof(MvxCarouselPage)}");
                }

                await PushOrReplacePage(FormsApplication.MainPage, page, attribute);
            }
            else
            {
                var carouselHost = GetPageOfType<CarouselPage>();
                if (carouselHost == null)
                {
                    MvxFormsLog.Instance.Trace($"Current root is not a CarouselPage show your own first to use custom Host. Assuming we need to create one.");
                    carouselHost = new CarouselPage();
                    await PushOrReplacePage(FormsApplication.MainPage, carouselHost, attribute);
                }
                carouselHost.Children.Add(page as ContentPage);
            }
            return true;
        }

        public virtual Task<bool> CloseCarouselPage(IMvxViewModel viewModel, MvxCarouselPagePresentationAttribute attribute)
        {
            if (attribute.Position == CarouselPosition.Root)
            {
                return FindAndCloseViewFromViewModel(viewModel, FormsApplication.MainPage, attribute);
            }
            else
            {
                var carouselHost = GetPageOfType<MvxCarouselPage>();
                var page = carouselHost.Children.OfType<IMvxPage>().FirstOrDefault(x => x.ViewModel == viewModel);
                if (page is ContentPage carouselPage)
                    carouselHost.Children.Remove(carouselPage);
                else
                    return Task.FromResult(false);
                return Task.FromResult(true);
            }
        }

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (viewType.GetTypeInfo().IsSubclassOf(typeof(ContentPage)))
            {
                MvxFormsLog.Instance.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming ContentPage presentation");
                return new MvxContentPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.GetTypeInfo().IsSubclassOf(typeof(CarouselPage)))
            {
                MvxFormsLog.Instance.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming CarouselPage presentation");
                return new MvxCarouselPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.GetTypeInfo().IsSubclassOf(typeof(TabbedPage)))
            {
                MvxFormsLog.Instance.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming TabbedPage presentation");
                return new MvxTabbedPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.GetTypeInfo().IsSubclassOf(typeof(MasterDetailPage)))
            {
                MvxFormsLog.Instance.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming MasterDetailPage presentation");
                return new MvxMasterDetailPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.GetTypeInfo().IsSubclassOf(typeof(NavigationPage)))
            {
                MvxFormsLog.Instance.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming NavigationPage presentation");
                return new MvxNavigationPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }

            return PlatformPresenter.CreatePresentationAttribute(viewModelType, viewType);
        }

        public virtual async Task<bool> ShowContentPage(
            Type view,
            MvxContentPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = await CloseAndCreatePage(view, request, attribute);
            await PushOrReplacePage(FormsApplication.MainPage, page, attribute);
            return true;
        }

        public virtual Task<bool> CloseContentPage(IMvxViewModel viewModel, MvxContentPagePresentationAttribute attribute)
        {
            return FindAndCloseViewFromViewModel(viewModel, FormsApplication.MainPage, attribute);
        }

        public virtual async Task<bool> ShowMasterDetailPage(
            Type view,
            MvxMasterDetailPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = await CloseAndCreatePage(view, request, attribute);

            if (attribute.Position == MasterDetailPosition.Root)
            {
                if (page is MasterDetailPage masterDetailRoot)
                {
                    if (masterDetailRoot.Master == null)
                        masterDetailRoot.Master = CreateContentPage().Build(cp => cp.Title = !string.IsNullOrEmpty(attribute.Title) ? attribute.Title : nameof(MvxMasterDetailPage));
                    if (masterDetailRoot.Detail == null)
                        masterDetailRoot.Detail = CreateContentPage().Build(cp => cp.Title = !string.IsNullOrEmpty(attribute.Title) ? attribute.Title : nameof(MvxMasterDetailPage));

                    await PushOrReplacePage(FormsApplication.MainPage, page, attribute);
                }
                else
                    throw new MvxException($"A root page should be of type {nameof(MasterDetailPage)}");
            }
            else
            {
                var masterDetailHost = GetPageOfType<MasterDetailPage>();
                if (masterDetailHost == null)
                {
                    //Assume we have to create the host
                    masterDetailHost = CreateMasterDetailPage();

                    if (!string.IsNullOrWhiteSpace(attribute.Icon))
                    {
                        masterDetailHost.Icon = attribute.Icon;
                    }

                    masterDetailHost.Master = CreateContentPage().Build(cp => cp.Title = !string.IsNullOrWhiteSpace(attribute.Title) ? attribute.Title : nameof(MvxMasterDetailPage));
                    masterDetailHost.Detail = CreateContentPage();

                    var masterDetailRootAttribute = new MvxMasterDetailPagePresentationAttribute { Position = MasterDetailPosition.Root, WrapInNavigationPage = attribute.WrapInNavigationPage, NoHistory = attribute.NoHistory };

                    await PushOrReplacePage(FormsApplication.MainPage, masterDetailHost, masterDetailRootAttribute);
                }

                if (attribute.Position == MasterDetailPosition.Master)
                {
                    await PushOrReplacePage(masterDetailHost.Master, page, attribute);
                }
                else
                {
                    await PushOrReplacePage(masterDetailHost.Detail, page, attribute);
                }
            }
            return true;
        }

        public virtual Task<bool> CloseMasterDetailPage(IMvxViewModel viewModel, MvxMasterDetailPagePresentationAttribute attribute)
        {
            var masterDetailHost = GetPageOfType<MasterDetailPage>();
            switch (attribute.Position)
            {
                case MasterDetailPosition.Root:
                    return ClosePage(FormsApplication.MainPage, null, attribute);
                case MasterDetailPosition.Master:
                    return ClosePage(masterDetailHost.Master, null, attribute);
                case MasterDetailPosition.Detail:
                    return FindAndCloseViewFromViewModel(viewModel, masterDetailHost.Detail, attribute);
            }
            return Task.FromResult(true);
        }

        public virtual async Task<bool> ShowModal(
            Type view,
            MvxModalPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = await CloseAndCreatePage(view, request, attribute, closeModal: false);

            if (FormsApplication.MainPage == null)
                FormsApplication.MainPage = CreateNavigationPage(CreateContentPage().Build(cp => cp.Title = nameof(ContentPage)));

            if (attribute.WrapInNavigationPage)
            {
                if (GetModalPageOfType<NavigationPage>() is NavigationPage modalNavigation)
                {
                    // There's already a navigation page, so use existing logic
                    // to work out whether the nav stack should be cleared (eg No History)
                    await PushOrReplacePage(modalNavigation, page, attribute);
                }
                else
                {
                    // Either last isn't a nav page, or there is no last page
                    // So, wrap the current page in a nav page and push onto stack
                    await FormsApplication.MainPage.Navigation.PushModalAsync(new NavigationPage(page));
                }
            }
            else
            {
                // No navigation page required, so just push onto modal stack
                await FormsApplication.MainPage.Navigation.PushModalAsync(page, attribute.Animated);
            }
            return true;
        }

        public virtual async Task<bool> CloseModal(IMvxViewModel viewModel, MvxModalPresentationAttribute attribute)
        {
            var last = FormsApplication.MainPage.Navigation.ModalStack.LastOrDefault();
            if (last is NavigationPage navPage && navPage.Navigation.NavigationStack.Count > 1)
            {
                await navPage.Navigation.PopAsync();
            }
            else
            {
                await FormsApplication.MainPage.Navigation.PopModalAsync(attribute.Animated);
            }

            return true;
        }

        public virtual async Task<bool> ShowNavigationPage(
            Type view,
            MvxNavigationPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = await CloseAndCreatePage(view, request, attribute);

            if (attribute.NoHistory)
                FormsApplication.MainPage = page;
            else
                await PushOrReplacePage(FormsApplication.MainPage, page, attribute);
            return true;
        }

        public virtual Task<bool> CloseNavigationPage(IMvxViewModel viewModel, MvxNavigationPagePresentationAttribute attribute)
        {
            return FindAndCloseViewFromViewModel(viewModel, FormsApplication.MainPage, attribute);
        }

        public virtual async Task<bool> ShowTabbedPage(
            Type view,
            MvxTabbedPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = await CloseAndCreatePage(view, request, attribute);

            if (attribute.Position == TabbedPosition.Root)
            {
                if (page is TabbedPage tabbedPageRoot)
                {
                    await PushOrReplacePage(FormsApplication.MainPage, page, attribute);
                }
                else
                    throw new MvxException($"A root page should be of type {nameof(TabbedPage)}");
            }
            else
            {
                var tabHost = GetPageOfType<TabbedPage>();
                if (tabHost == null)
                {
                    MvxFormsLog.Instance.Trace($"Current root is not a TabbedPage show your own first to use custom Host. Assuming we need to create one.");
                    tabHost = new TabbedPage();
                    await PushOrReplacePage(FormsApplication.MainPage, tabHost, attribute);
                }

                tabHost.Children.Add(page);
            }
            return true;
        }

        public virtual Task<bool> CloseTabbedPage(IMvxViewModel viewModel, MvxTabbedPagePresentationAttribute attribute)
        {
            if (attribute.Position == TabbedPosition.Root)
            {
                return FindAndCloseViewFromViewModel(viewModel, FormsApplication.MainPage, attribute);
            }
            else
            {
                var tabHost = GetPageOfType<MvxTabbedPage>();
                var page = tabHost.Children.OfType<IMvxPage>().FirstOrDefault(x => x.ViewModel == viewModel);
                if (page is Page tabPage)
                    tabHost.Children.Remove(tabPage);
                else
                    return Task.FromResult(false);

                return Task.FromResult(true);
            }
        }

        public virtual async Task CloseAllModals(bool animate = false)
        {
            var navigation = FormsApplication.MainPage?.Navigation;
            while (navigation?.ModalStack.Any() ?? false)
            {
                await navigation.PopModalAsync(animate);
            }
        }

        protected virtual async Task<bool> ClosePage(Page rootPage, Page page, MvxPagePresentationAttribute attribute)
        {
            var root = TopNavigationPage();

            if (page != null)
            {
                root?.Navigation?.RemovePage(page);
            }
            else
            {
                var nav = root?.Navigation;
                if (nav != null)
                {
                    await nav.PopAsync(attribute.Animated);
                }
            }
            return true;
        }

        public virtual async Task PushOrReplacePage(Page rootPage, Page page, MvxPagePresentationAttribute attribute)
        {
            // Make sure we always have a rootPage
            if (rootPage == null)
            {
                rootPage = FormsApplication.MainPage;
            }

            var navigationRootPage = GetPageOfType<NavigationPage>(rootPage);

            // Step down through any nested navigation pages to make sure we're pushing to the
            // most nested navigation page
            if (attribute.WrapInNavigationPage &&
                navigationRootPage?.CurrentPage is NavigationPage navigationNestedPage)
            {
                await PushOrReplacePage(navigationNestedPage, page, attribute);
                return;
            }

            // Handle the case where the page should be wrapped in a navigation page
            if (attribute.WrapInNavigationPage)
            {
                // Look at parent and see whether it's a navigation page,
                // if it is, then use it to navigate to the new page
                if (navigationRootPage == null && rootPage?.Parent is NavigationPage parentNavigation)
                {
                    navigationRootPage = parentNavigation;
                }

                // If the root isn't a navigation page, we need to wrap the new page
                // in a navigation wrapper.
                if (navigationRootPage == null || attribute.NoHistory)
                {
                    var navpage = CreateNavigationPage(page);
                    ReplacePageRoot(rootPage, navpage, attribute);
                }
                else
                {
                    await navigationRootPage.PushAsync(page, attribute.Animated);
                }
            }
            else
            {
                ReplacePageRoot(rootPage, page, attribute);
            }
        }

        public virtual Page[] CurrentPageTree
        {
            get
            {
                var pages = new List<Page>();
                var builder = BuildPageTree();
                builder(pages);
                return pages.ToArray();
            }
        }

        protected virtual Action<List<Page>> BuildPageTree(Page rootPage = null)
        {
            if (rootPage == null)
                rootPage = FormsApplication.MainPage;

            Func<List<Page>, List<Page>> pageListBuilder = (list) =>
            {
                list.Add(rootPage);
                return list;
            };

            if (rootPage is NavigationPage navigationPage)
            {
                // Check for modals
                var topMostModal = navigationPage?.Navigation?.ModalStack?.LastOrDefault();
                if (topMostModal != null && topMostModal != navigationPage)
                {
                    var currentModalNav = BuildPageTree(topMostModal);
                    if (currentModalNav != null) return (List<Page> list) => currentModalNav(pageListBuilder(list));
                }

                if (navigationPage.CurrentPage != null)
                {
                    // Check if there's a nested navigation
                    var navPage = BuildPageTree(navigationPage.CurrentPage);
                    if (navPage != null) return (List<Page> list) => navPage(pageListBuilder(list));
                }
            }

            // The page isn't a navigation page, so check
            // to see if it's a master detail, and if so, check
            // the Detail and Master pages for a navigation page
            if (rootPage is MasterDetailPage masterDetailsPage)
            {
                if (masterDetailsPage.Detail != null)
                {
                    var navDetailPage = BuildPageTree(masterDetailsPage.Detail);
                    if (navDetailPage != null) return (List<Page> list) => navDetailPage(pageListBuilder(list));
                }

                if (masterDetailsPage.Master != null)
                {
                    var navMasterPage = BuildPageTree(masterDetailsPage.Master);
                    if (navMasterPage != null) return (List<Page> list) => navMasterPage(pageListBuilder(list));
                }
            }

            if (rootPage is MultiPage<Page> multiPage)
            {
                var currentTab = multiPage.CurrentPage;
                if (currentTab != null)
                {
                    var currentTabPage = BuildPageTree(currentTab);

                    return (List<Page> list) => currentTabPage(pageListBuilder(list));
                }
            }

            // Nothing, all out of luck!
            return (List<Page> list) => pageListBuilder(list);
        }

        public virtual NavigationPage TopNavigationPage(Page rootPage = null)
        {
            if (rootPage == null)
                rootPage = FormsApplication.MainPage;

            if (rootPage is NavigationPage navigationPage)
            {
                if (navigationPage.CurrentPage != null)
                {
                    // Check if there's a nested navigation
                    var navPage = TopNavigationPage(navigationPage.CurrentPage);
                    if (navPage != null) return navPage;
                }

                // Check for modals
                var topMostModal = navigationPage?.Navigation?.ModalStack?.LastOrDefault();
                if (topMostModal != null && topMostModal != navigationPage)
                {
                    var currentModalNav = TopNavigationPage(topMostModal);
                    if (currentModalNav != null) return currentModalNav;
                }

                return navigationPage;
            }

            // The page isn't a navigation page, so check
            // to see if it's a master detail, and if so, check
            // the Detail and Master pages for a navigation page
            if (rootPage is MasterDetailPage masterDetailsPage)
            {
                if (masterDetailsPage.Detail != null)
                {
                    var navDetailPage = TopNavigationPage(masterDetailsPage.Detail);
                    if (navDetailPage != null) return navDetailPage;
                }

                if (masterDetailsPage.Master != null)
                {
                    var navMasterPage = TopNavigationPage(masterDetailsPage.Master);
                    if (navMasterPage != null) return navMasterPage;
                }
            }

            // Nothing, all out of luck!
            return null;
        }

        public virtual TPage GetModalPageOfType<TPage>(Page rootPage = null) where TPage : Page
        {
            if (rootPage == null)
                rootPage = FormsApplication.MainPage;

            if (rootPage?.Navigation?.ModalStack?.Count > 0)
            {
                foreach (var item in rootPage.Navigation.ModalStack)
                {
                    var modalPage = GetPageOfType<TPage>(item);
                    if (modalPage is TPage)
                        return modalPage;
                }
            }
            return null;
        }

        //Needs to have a different method name
        public virtual Page GetPageOfTypeByType(Type viewType, Page rootPage = null)
        {
            if (rootPage == null)
                rootPage = FormsApplication.MainPage;

            return GetType().GetMethod(nameof(GetPageOfType)).MakeGenericMethod(viewType).Invoke(this, new object[] { rootPage }) as Page;
        }

        public virtual TPage GetPageOfType<TPage>(Page rootPage = null) where TPage : Page
        {
            if (rootPage == null)
                rootPage = FormsApplication.MainPage;

            if (rootPage is TPage)
                return rootPage as TPage;
            else if (rootPage is NavigationPage navigationRootPage)
            {
                if (navigationRootPage.CurrentPage is NavigationPage navigationNestedPage)
                    return GetPageOfType<TPage>(navigationNestedPage);
                else
                    return GetPageOfType<TPage>(navigationRootPage.CurrentPage);
            }
            else if (rootPage is MasterDetailPage masterDetailRoot)
            {
                var detailHost = GetPageOfType<TPage>(masterDetailRoot.Detail);
                if (detailHost is TPage)
                    return detailHost;
                else
                    return GetPageOfType<TPage>(masterDetailRoot.Master);
            }
            else if (rootPage is CarouselPage carouselPage)
            {
                foreach (var item in carouselPage.Children)
                {
                    var nestedPage = GetPageOfType<TPage>(item);
                    if (nestedPage is TPage)
                        return nestedPage;
                }
                return rootPage as TPage;
            }
            else if (rootPage is TabbedPage tabbedPage)
            {
                foreach (var item in tabbedPage.Children)
                {
                    var nestedPage = GetPageOfType<TPage>(item);
                    if (nestedPage is TPage)
                        return nestedPage;
                }
                return rootPage as TPage;
            }
            else
                return rootPage as TPage;
        }

        public virtual void ReplacePageRoot(Page existingPage, Page page, MvxPagePresentationAttribute attribute)
        {
            try
            {
                // If the existing page is the current main page of the forms
                // app, then simply replace it
                if (existingPage == FormsApplication.MainPage)
                {
                    FormsApplication.MainPage = page;
                    return;
                }

                var parent = existingPage.Parent;
                if (parent is MasterDetailPage rootMasterDetail)
                {
                    if (attribute is MvxMasterDetailPagePresentationAttribute masterDetailAttribute)
                    {
                        if (masterDetailAttribute.Position == MasterDetailPosition.Master)
                        {
                            rootMasterDetail.Master = page;
                        }
                        else if (masterDetailAttribute.Position == MasterDetailPosition.Detail)
                        {
                            rootMasterDetail.Detail = page;
                        }
                    }
                }

                // Handle updating pages within either carousel or tabbed pages
                if (parent is MultiPage<Page> carousel)
                {
                    var cp = page as Page;
                    var idx = carousel.Children.IndexOf(cp);
                    carousel.Children[idx] = cp;
                }
            }
            catch (Exception ex)
            {
                throw new MvxException(ex, "Cannot replace MainPage root");
            }
        }

        protected virtual Task<bool> FindAndCloseViewFromViewModel(IMvxViewModel mvxViewModel, Page rootPage, MvxPagePresentationAttribute attribute)
        {
            var root = TopNavigationPage();
            Page pageToClose = null;

            // finding the view from viewmodel in navigation stack
            pageToClose = root?.Navigation?.NavigationStack?
                .OfType<IMvxPage>()
                .FirstOrDefault(x => x.ViewModel == mvxViewModel) as Page;

            if (pageToClose == null)
            {
                MvxFormsLog.Instance.Warn("Ignoring close for ViewModel - Matching View for ViewModel instance failed");
                return Task.FromResult(false);
            }

            return ClosePage(rootPage, pageToClose, attribute);
        }
    }
}
