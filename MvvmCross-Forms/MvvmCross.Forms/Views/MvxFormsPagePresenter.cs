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
using System.Reflection;

namespace MvvmCross.Forms.Views
{
    //Handles common Forms Presenter code
    public class MvxFormsPagePresenter :
        MvxAttributeViewPresenter, IMvxFormsPagePresenter
    {
        public MvxFormsPagePresenter(
            MvxFormsApplication formsApplication,
            IMvxViewsContainer viewsContainer = null,
            IMvxViewModelTypeFinder viewModelTypeFinder = null,
            IMvxViewModelLoader viewModelLoader = null,
            Dictionary<Type, MvxPresentationAttributeAction> attributeTypesToActionsDictionary = null)
        {
            FormsApplication = formsApplication;
            ViewsContainer = viewsContainer;
            ViewModelTypeFinder = viewModelTypeFinder;
            ViewModelLoader = viewModelLoader;
            AttributeTypesToActionsDictionary = attributeTypesToActionsDictionary;
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

        public virtual Func<Type, bool> ShowPlatformHost { get; set; }
        public virtual Func<bool> ClosePlatformViews { get; set; }

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

        private Page CloseAndCreatePage(Type view,
            MvxViewModelRequest request,
            MvxPagePresentationAttribute attribute,
            bool closeModal = true,
            bool closePlatformViews = true,
            bool showPlatformViews = true)
        {
            if (closeModal)
            {
                CloseAllModals();
            }

            if (closePlatformViews)
            {
                ClosePlatformViews?.Invoke();
            }

            if (showPlatformViews)
            {
                ShowPlatformHost?.Invoke(attribute.HostViewModelType);
            }

            var page = CreatePage(view, request, attribute);
            return page;
        }

        public override void RegisterAttributeTypes()
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

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint)
            {
                Close((hint as MvxClosePresentationHint).ViewModelToClose);
                return;
            }

            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }

        public virtual void ShowCarouselPage(
            Type view,
            MvxCarouselPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CloseAndCreatePage(view, request, attribute);

            if (attribute.Position == CarouselPosition.Root)
            {
                if (!(page is MvxCarouselPage carouselPageRoot))
                {
                    throw new MvxException($"A root page should be of type {nameof(MvxCarouselPage)}");
                }

                PushOrReplacePage(FormsApplication.MainPage, page, attribute);
            }
            else
            {
                var carouselHost = GetHostPageOfType<MvxCarouselPage>();
                if (carouselHost == null)
                {
                    MvxTrace.Trace($"Current root is not a CarouselPage show your own first to use custom Host. Assuming we need to create one.");
                    carouselHost = new MvxCarouselPage();
                    PushOrReplacePage(FormsApplication.MainPage, carouselHost, attribute);
                }
                carouselHost.Children.Add(page as ContentPage);
            }
        }

        public virtual bool CloseCarouselPage(IMvxViewModel viewModel, MvxCarouselPagePresentationAttribute attribute)
        {
            if (attribute.Position == CarouselPosition.Root)
                return ClosePage(FormsApplication.MainPage, null, attribute);
            else
            {
                var carouselHost = GetHostPageOfType<MvxCarouselPage>();
                var page = carouselHost.Children.OfType<IMvxPage>().FirstOrDefault(x => x.ViewModel == viewModel);
                if (page is ContentPage carouselPage)
                    carouselHost.Children.Remove(carouselPage);
                else
                    return false;
                return true;
            }
        }

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (viewType.GetTypeInfo().IsSubclassOf(typeof(ContentPage)))
            {
                MvxTrace.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming ContentPage presentation");
                return new MvxContentPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.GetTypeInfo().IsSubclassOf(typeof(CarouselPage)))
            {
                MvxTrace.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming CarouselPage presentation");
                return new MvxCarouselPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.GetTypeInfo().IsSubclassOf(typeof(TabbedPage)))
            {
                MvxTrace.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming TabbedPage presentation");
                return new MvxTabbedPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.GetTypeInfo().IsSubclassOf(typeof(MasterDetailPage)))
            {
                MvxTrace.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming MasterDetailPage presentation");
                return new MvxMasterDetailPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.GetTypeInfo().IsSubclassOf(typeof(NavigationPage)))
            {
                MvxTrace.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming NavigationPage presentation");
                return new MvxNavigationPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }

            return null;
        }

        public virtual void ShowContentPage(
            Type view,
            MvxContentPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CloseAndCreatePage(view, request, attribute);

            PushOrReplacePage(FormsApplication.MainPage, page, attribute);
        }

        public virtual bool CloseContentPage(IMvxViewModel viewModel, MvxContentPagePresentationAttribute attribute)
        {
            return ClosePage(FormsApplication.MainPage, null, attribute);
        }

        public virtual void ShowMasterDetailPage(
            Type view,
            MvxMasterDetailPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CloseAndCreatePage(view, request, attribute);

            if (attribute.Position == MasterDetailPosition.Root)
            {
                if (page is MasterDetailPage masterDetailRoot)
                {
                    if (masterDetailRoot.Master == null)
                        masterDetailRoot.Master = new ContentPage() { Title = !string.IsNullOrEmpty(attribute.Title) ? attribute.Title : nameof(MvxMasterDetailPage) };
                    if (masterDetailRoot.Detail == null)
                        masterDetailRoot.Detail = new ContentPage() { Title = !string.IsNullOrEmpty(attribute.Title) ? attribute.Title : nameof(MvxMasterDetailPage) };

                    PushOrReplacePage(FormsApplication.MainPage, page, attribute);
                }
                else
                    throw new MvxException($"A root page should be of type {nameof(MasterDetailPage)}");
            }
            else
            {
                var masterDetailHost = GetHostPageOfType<MvxMasterDetailPage>();
                if (masterDetailHost == null)
                {
                    //Assume we have to create the host
                    masterDetailHost = new MvxMasterDetailPage();

                    if (!string.IsNullOrWhiteSpace(attribute.Icon))
                    {
                        masterDetailHost.Icon = attribute.Icon;
                    }

                    masterDetailHost.Master = new ContentPage() { Title = !string.IsNullOrWhiteSpace(attribute.Title) ? attribute.Title : nameof(MvxMasterDetailPage) };
                    masterDetailHost.Detail = new ContentPage();

                    var masterDetailRootAttribute = new MvxMasterDetailPagePresentationAttribute { Position = MasterDetailPosition.Root, WrapInNavigationPage = attribute.WrapInNavigationPage, NoHistory = attribute.NoHistory };

                    PushOrReplacePage(FormsApplication.MainPage, masterDetailHost, masterDetailRootAttribute);
                }

                if (attribute.Position == MasterDetailPosition.Master)
                {
                    PushOrReplacePage(masterDetailHost.Master, page, attribute);
                }
                else
                {
                    PushOrReplacePage(masterDetailHost.Detail, page, attribute);
                }
            }
        }

        public virtual bool CloseMasterDetailPage(IMvxViewModel viewModel, MvxMasterDetailPagePresentationAttribute attribute)
        {
            var masterDetailHost = GetHostPageOfType<MvxMasterDetailPage>();
            switch (attribute.Position)
            {
                case MasterDetailPosition.Root:
                    return ClosePage(FormsApplication.MainPage, null, attribute);
                case MasterDetailPosition.Master:
                    return ClosePage(masterDetailHost.Master, null, attribute);
                case MasterDetailPosition.Detail:
                    return ClosePage(masterDetailHost.Detail, null, attribute);
            }
            return true;
        }

        public virtual void ShowModal(
            Type view,
            MvxModalPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CloseAndCreatePage(view, request, attribute, closeModal: false);

            if (FormsApplication.MainPage == null)
                FormsApplication.MainPage = new NavigationPage(new ContentPage() { Title = nameof(ContentPage) });

            var last = FormsApplication.MainPage.Navigation.ModalStack.LastOrDefault();

            if (attribute.WrapInNavigationPage)
            {
                if (last is NavigationPage navPage)
                {
                    // There's already a navigation page, so use existing logic
                    // to work out whether the nav stack should be cleared (eg No History)
                    PushOrReplacePage(navPage, page, attribute);
                }
                else
                {
                    // Either last isn't a nav page, or there is no last page
                    // So, wrap the current page in a nav page and push onto stack
                    FormsApplication.MainPage.Navigation.PushModalAsync(new NavigationPage(page));
                }
            }
            else
            {
                // No navigation page required, so just push onto modal stack
                FormsApplication.MainPage.Navigation.PushModalAsync(page, attribute.Animated);
            }
        }

        public virtual bool CloseModal(IMvxViewModel viewModel, MvxModalPresentationAttribute attribute)
        {
            return PopModal(attribute);
        }

        private NavigationPage TopNavigationPage()
        {
            return TopNavigationPage(FormsApplication.MainPage);
        }

        private NavigationPage TopNavigationPage(Page rootPage)
        {
            if (rootPage == null)
            {
                return TopNavigationPage(FormsApplication.MainPage);
            }

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

        private bool PopModal(MvxPagePresentationAttribute attribute)
        {
            var last = FormsApplication.MainPage.Navigation.ModalStack.LastOrDefault();
            if (last is NavigationPage navPage && navPage.Navigation.NavigationStack.Count > 1)
            {
                navPage.Navigation.PopAsync();
            }
            else
            {
                FormsApplication.MainPage.Navigation.PopModalAsync(attribute.Animated);
            }

            return true;
        }

        public virtual void ShowNavigationPage(
            Type view,
            MvxNavigationPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CloseAndCreatePage(view, request, attribute);

            if (attribute.NoHistory)
                FormsApplication.MainPage = page;
            else
                PushOrReplacePage(FormsApplication.MainPage, page, attribute);
        }

        public virtual bool CloseNavigationPage(IMvxViewModel viewModel, MvxNavigationPagePresentationAttribute attribute)
        {
            return ClosePage(FormsApplication.MainPage, null, attribute);
        }

        public virtual void ShowTabbedPage(
            Type view,
            MvxTabbedPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CloseAndCreatePage(view, request, attribute);

            if (attribute.Position == TabbedPosition.Root)
            {
                if (page is TabbedPage tabbedPageRoot)
                {
                    PushOrReplacePage(FormsApplication.MainPage, page, attribute);
                }
                else
                    throw new MvxException($"A root page should be of type {nameof(TabbedPage)}");
            }
            else
            {
                var tabHost = GetHostPageOfType<MvxTabbedPage>();
                if (tabHost == null)
                {
                    MvxTrace.Trace($"Current root is not a TabbedPage show your own first to use custom Host. Assuming we need to create one.");
                    tabHost = new MvxTabbedPage();
                    PushOrReplacePage(FormsApplication.MainPage, tabHost, attribute);
                }

                tabHost.Children.Add(page);
            }
        }

        public virtual bool CloseTabbedPage(IMvxViewModel viewModel, MvxTabbedPagePresentationAttribute attribute)
        {
            if (attribute.Position == TabbedPosition.Root)
                return ClosePage(FormsApplication.MainPage, null, attribute);
            else
            {
                var tabHost = GetHostPageOfType<MvxTabbedPage>();
                var page = tabHost.Children.OfType<IMvxPage>().FirstOrDefault(x => x.ViewModel == viewModel);
                if (page is Page tabPage)
                    tabHost.Children.Remove(tabPage);
                else
                    return false;
                return true;
            }
        }

        public virtual void CloseAllModals(Page rootPage = null)
        {
            if (rootPage == null)
                rootPage = FormsApplication.MainPage;

            if (rootPage is NavigationPage rootNavigationPage && rootNavigationPage.CurrentPage is NavigationPage nestedNavigationPage)
            {
                CloseAllModals(nestedNavigationPage);
            }
            else if (rootPage is NavigationPage navigationPage)
            {
                CloseModalStack(navigationPage.Navigation.ModalStack);
            }
            else if (rootPage?.Navigation?.ModalStack?.Count > 0)
            {
                CloseModalStack(FormsApplication.MainPage.Navigation.ModalStack);
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

        public virtual void PushOrReplacePage(Page rootPage, Page page, MvxPagePresentationAttribute attribute)
        {
            // Make sure we always have a rootPage
            if (rootPage == null)
            {
                rootPage = FormsApplication.MainPage;
            }

            var navigationRootPage = rootPage as NavigationPage;

            // Step down through any nested navigation pages to make sure we're pushing to the
            // most nested navigation page
            if (attribute.WrapInNavigationPage &&
                navigationRootPage?.CurrentPage is NavigationPage navigationNestedPage)
            {
                PushOrReplacePage(navigationNestedPage, page, attribute);
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
                if (navigationRootPage == null)
                {
                    // NR: This is a really hacky solution to a bug where if "NavigationPage.HasNavigationBar="False"
                    // is set in the page XAML, the navigation bar is still shown. Looks like after first navigation
                    // this is resolved
                    var navpage = new NavigationPage(new ContentPage());
                    ReplacePageRoot(rootPage, navpage, attribute);
                    navpage.Navigation.InsertPageBefore(page, navpage.RootPage);
                    navpage.Navigation.PopToRootAsync(attribute.Animated);
                }
                else
                {
                    if (attribute.NoHistory)
                    {
                        navigationRootPage.Navigation.InsertPageBefore(page, navigationRootPage.RootPage);
                        navigationRootPage.Navigation.PopToRootAsync(attribute.Animated);
                    }
                    else
                    {
                        navigationRootPage.PushAsync(page, attribute.Animated);
                    }
                }
            }
            else
            {
                ReplacePageRoot(rootPage, page, attribute);
            }

            //if (attribute.NoHistory)
            //{
            //    if (attribute.WrapInNavigationPage && rootPage is NavigationPage navigationRootPage && navigationRootPage.CurrentPage is NavigationPage navigationNestedPage)
            //    {
            //        PushOrReplacePage(navigationNestedPage, page, attribute);
            //    }
            //    else if (attribute is MvxMasterDetailPagePresentationAttribute masterDetailAttribute && masterDetailAttribute.Position != MasterDetailPosition.Root)
            //    {
            //        if (attribute.WrapInNavigationPage)
            //        {
            //            page = new NavigationPage(page);
            //        }

            //        ReplaceMasterDetailRoot(rootPage, page, masterDetailAttribute);
            //    }
            //    else if (attribute.WrapInNavigationPage && rootPage is NavigationPage navigationPage)
            //    {
            //        navigationPage.Navigation.InsertPageBefore(page, navigationPage.RootPage);
            //        navigationPage.Navigation.PopToRootAsync(attribute.Animated);
            //    }
            //    else
            //    {
            //        ReplaceRoot(page);
            //    }
            //}
            //else
            //{
            //    if (rootPage is NavigationPage navigationRootPage && navigationRootPage.CurrentPage is NavigationPage navigationNestedPage)
            //        PushOrReplacePage(navigationNestedPage, page, attribute);
            //    else if (attribute is MvxMasterDetailPagePresentationAttribute masterDetailAttribute && masterDetailAttribute.Position != MasterDetailPosition.Root)
            //    {
            //        // TODO: Need to adjust this to handle WrapInNavigationPage set to true
            //        ReplaceMasterDetailRoot(rootPage, page, masterDetailAttribute);
            //    }
            //    else if (attribute.WrapInNavigationPage && rootPage is NavigationPage navigationPage)
            //        navigationPage.PushAsync(page, attribute.Animated);
            //    else if (attribute.WrapInNavigationPage)
            //        ReplaceRoot(new NavigationPage(page));
            //    else
            //        ReplaceRoot(page);
            //}
        }

        public virtual bool ClosePage(Page rootPage, Page page, MvxPagePresentationAttribute attribute)
        {
            //var modal = rootPage?.Navigation?.ModalStack?.LastOrDefault();
            //if (modal.)

            var root = TopNavigationPage();


            if (page != null)
            {
                root?.Navigation?.RemovePage(page);

                //if (rootPage is NavigationPage navigationRootPage)
                //{
                //    if (navigationRootPage.CurrentPage is NavigationPage navigationNestedPage)
                //        ClosePage(navigationNestedPage, page, attribute);
                //    else
                //        navigationRootPage.Navigation.RemovePage(page);
                //}
                //else if (rootPage.Navigation != null)
                //{
                //    rootPage.Navigation.RemovePage(page);
                //}
                //else
                //{
                //    MvxTrace.Trace($"Cannot remove page: {page.Title}");
                //    return false;
                //}
            }
            else
            {
                root?.Navigation?.PopAsync(attribute.Animated);

                //if (rootPage is NavigationPage navigationRootPage)
                //{
                //    if (navigationRootPage.CurrentPage is NavigationPage navigationNestedPage)
                //        ClosePage(navigationNestedPage, page, attribute);
                //    else
                //        navigationRootPage.PopAsync(attribute.Animated);
                //}
                //else if (rootPage.Navigation != null)
                //{
                //    rootPage.Navigation.PopAsync(attribute.Animated);
                //}
                //else
                //{
                //    MvxTrace.Trace($"Cannot pop page: {page.Title}");
                //    return false;
                //}
            }
            return true;
        }

        protected TPage GetHostPageOfType<TPage>(Page rootPage = null) where TPage : Page
        {
            if (rootPage == null)
                rootPage = FormsApplication.MainPage;

            if (rootPage is TPage)
                return rootPage as TPage;
            else if (rootPage is NavigationPage navigationRootPage)
            {
                if (navigationRootPage.CurrentPage is NavigationPage navigationNestedPage)
                    return GetHostPageOfType<TPage>(navigationNestedPage);
                else
                    return GetHostPageOfType<TPage>(navigationRootPage.CurrentPage);
            }
            else if (rootPage is MvxMasterDetailPage masterDetailRoot)
            {
                var detailHost = GetHostPageOfType<TPage>(masterDetailRoot.Detail);
                if (detailHost is TPage)
                    return detailHost;
                else
                    return GetHostPageOfType<TPage>(masterDetailRoot.Master);
            }
            else if (rootPage is MvxCarouselPage carouselPage)
            {
                foreach (var item in carouselPage.Children)
                {
                    var nestedPage = GetHostPageOfType<TPage>(item);
                    if (nestedPage is TPage)
                        return nestedPage;
                }
                return rootPage as TPage;
            }
            else if (rootPage is MvxTabbedPage tabbedPage)
            {
                foreach (var item in tabbedPage.Children)
                {
                    var nestedPage = GetHostPageOfType<TPage>(item);
                    if (nestedPage is TPage)
                        return nestedPage;
                }
                return rootPage as TPage;
            }
            else
                return rootPage as TPage;
        }

        //public virtual void ReplaceMasterDetailRoot(Page existingMasterDetailPage, Page newRootPage, MvxMasterDetailPagePresentationAttribute masterDetailAttribute)
        //{
        //    try
        //    {
        //        var rootMasterDetail = existingMasterDetailPage.Parent as MasterDetailPage;
        //        if (rootMasterDetail == null)
        //        {
        //            return;
        //        }



        //        if (masterDetailAttribute.Position == MasterDetailPosition.Master)
        //        {
        //            if (masterDetailAttribute.WrapInNavigationPage)
        //            {
        //                if (rootMasterDetail.Master is NavigationPage navPage)
        //                {
        //                    navPage.PushAsync(newRootPage);
        //                }
        //                else
        //                {
        //                    rootMasterDetail.Master = new NavigationPage(newRootPage);
        //                }
        //            }
        //            else
        //            {
        //                rootMasterDetail.Master = newRootPage;
        //            }
        //        }
        //        else if (masterDetailAttribute.Position == MasterDetailPosition.Detail)
        //        {
        //            if (masterDetailAttribute.WrapInNavigationPage)
        //            {
        //                if (rootMasterDetail.Detail is NavigationPage navPage)
        //                {
        //                    navPage.PushAsync(newRootPage);
        //                }
        //                else
        //                {
        //                    rootMasterDetail.Detail = new NavigationPage(newRootPage);
        //                }
        //            }
        //            else
        //            {
        //                rootMasterDetail.Detail = newRootPage;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new MvxException("Cannot replace MainPage root", ex);
        //    }
        //}

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
                if (parent is MultiPage<ContentPage> carousel)
                {
                    var cp = page as ContentPage;
                    var idx = carousel.Children.IndexOf(cp);
                    carousel.Children[idx] = cp;
                }

            }
            catch (Exception ex)
            {
                throw new MvxException("Cannot replace MainPage root", ex);
            }
        }

        public virtual void ReplaceRoot(Page page)
        {
            try
            {
                FormsApplication.MainPage = page;
            }
            catch (Exception ex)
            {
                throw new MvxException("Cannot replace MainPage root", ex);
            }
        }
    }
}