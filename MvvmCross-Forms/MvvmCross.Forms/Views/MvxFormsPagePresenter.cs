using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Views.Attributes;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    //Handles common Forms Presenter code
    public class MvxFormsPagePresenter
    {
        public MvxFormsPagePresenter(MvxFormsApplication formsApplication)
        {
            FormsApplication = formsApplication;
        }

        private MvxFormsApplication _formsApplication;
        public MvxFormsApplication FormsApplication
        {
            get { return _formsApplication; }
            set { _formsApplication = value; }
        }

        public virtual Page CreatePage(Type viewType, MvxViewModelRequest request)
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

            var page = CreatePage(view, request);

            if (attribute.Position == CarouselPosition.Root)
            {
                if (page is CarouselPage carouselPageRoot)
                {
                    if (attribute.WrapInNavigationPage && FormsApplication.MainPage is MvxNavigationPage currentPage)
                        currentPage.PushAsync(page);
                    else if (attribute.WrapInNavigationPage)
                        FormsApplication.MainPage = new MvxNavigationPage(page);
                    else
                        FormsApplication.MainPage = page;
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
                        navigationPage.PushAsync(carouselHost);
                    }
                }
                else if (carouselHost == null)
                {
                    MvxTrace.Trace($"Current root is not a CarouselPage show your own first to use custom Host. Assuming we need to create one.");
                    carouselHost = new MvxCarouselPage();
                    FormsApplication.MainPage = carouselHost;
                }

                if (string.IsNullOrEmpty(page.Title))
                    page.Title = attribute.Title;
                carouselHost.Children.Add(page as ContentPage);
            }
        }

        public virtual bool CloseCarouselPage(IMvxViewModel viewModel, MvxCarouselPagePresentationAttribute attribute)
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

        public virtual void ShowContentPage(
            Type view,
            MvxContentPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            CloseAllModals();

            var page = CreatePage(view, request);

            if (attribute.WrapInNavigationPage && FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                navigationPage.PushAsync(page, attribute.Animated);
            }
            else if (attribute.WrapInNavigationPage)
            {
                FormsApplication.MainPage = new MvxNavigationPage(page);
            }
            else
            {
                //TODO: Crashes on iOS
                FormsApplication.MainPage = page;
            }
        }

        public virtual bool CloseContentPage(IMvxViewModel viewModel, MvxContentPagePresentationAttribute attribute)
        {
            if (FormsApplication.MainPage is MvxNavigationPage navigationPage)
                navigationPage.PopAsync(attribute.Animated);
            return true;
        }

        public virtual void ShowMasterDetailPage(
            Type view,
            MvxMasterDetailPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            CloseAllModals();

            var page = CreatePage(view, request);

            if(attribute.Position == MasterDetailPosition.Root)
            {
                if (page is MasterDetailPage masterDetailRoot)
                {
                    if (masterDetailRoot.Master == null)
                        masterDetailRoot.Master = new MvxContentPage() { Title = "MvvmCross" };
                    if (masterDetailRoot.Detail == null)
                        masterDetailRoot.Detail = new MvxContentPage() { Title = "MvvmCross" };

                    if (attribute.WrapInNavigationPage && FormsApplication.MainPage is MvxNavigationPage currentPage)
                        currentPage.PushAsync(page);
                    else if (attribute.WrapInNavigationPage)
                        FormsApplication.MainPage = new MvxNavigationPage(page);
                    else
                        FormsApplication.MainPage = page;
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
                        masterDetailHost.Master = new MvxContentPage() { Title = "MvvmCross" };
                        masterDetailHost.Detail = new MvxContentPage() { Title = "MvvmCross" };
                        navigationPage.PushAsync(masterDetailHost);
                    }
                }
                else if (masterDetailHost == null)
                {
                    //Assume we have to create the host
                    masterDetailHost = new MasterDetailPage();
                    masterDetailHost.Master = new MvxContentPage() { Title = "MvvmCross" };
                    masterDetailHost.Detail = new MvxContentPage() { Title = "MvvmCross" };
                    FormsApplication.MainPage = masterDetailHost;
                }
                if(attribute.Position == MasterDetailPosition.Master)
                {
                    if (attribute.WrapInNavigationPage && masterDetailHost.Master is MvxNavigationPage navigationMasterPage)
                        navigationMasterPage.PushAsync(page);
                    else if (attribute.WrapInNavigationPage)
                        masterDetailHost.Master = new MvxNavigationPage(page);
                    else
                        masterDetailHost.Master = page;
                }
                else
                {
                    if (attribute.WrapInNavigationPage && masterDetailHost.Detail is MvxNavigationPage navigationDetailPage)
                        navigationDetailPage.PushAsync(page);
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
                        rootNavigationPage.PopAsync();
                    break;
                case MasterDetailPosition.Master:
                    if (masterDetailHost.Master is NavigationPage navigationMasterPage)
                        navigationMasterPage.PopAsync();
                    break;
                case MasterDetailPosition.Detail:
                    if (masterDetailHost.Detail is NavigationPage navigationDetailPage)
                        navigationDetailPage.PopAsync();
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
            var page = CreatePage(view, request);

            if (FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                if (attribute.WrapInNavigationPage && navigationPage.Navigation.ModalStack.LastOrDefault() is MvxNavigationPage modalNavigationPage)
                    modalNavigationPage.PushAsync(page);
                else if (attribute.WrapInNavigationPage)
                    navigationPage.Navigation.PushModalAsync(new MvxNavigationPage(page));
                else
                    navigationPage.Navigation.PushModalAsync(page);
            }
            else
            {
                if (attribute.WrapInNavigationPage && FormsApplication.MainPage.Navigation.ModalStack.LastOrDefault() is MvxNavigationPage modalNavigationPage)
                    modalNavigationPage.PushAsync(page);
                else if (attribute.WrapInNavigationPage)
                    FormsApplication.MainPage.Navigation.PushModalAsync(new MvxNavigationPage(page));
                else
                    FormsApplication.MainPage.Navigation.PushModalAsync(page);
            }
        }

        public virtual bool CloseModal(IMvxViewModel viewModel, MvxModalPresentationAttribute attribute)
        {
            if (FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                if (attribute.WrapInNavigationPage && navigationPage.Navigation.ModalStack.LastOrDefault() is MvxNavigationPage modalNavigationPage && modalNavigationPage.Navigation.NavigationStack.Count > 1)
                    modalNavigationPage.PopAsync();
                else
                    navigationPage.Navigation.PopModalAsync();
            }
            else
            {
                if (attribute.WrapInNavigationPage && FormsApplication.MainPage.Navigation.ModalStack.LastOrDefault() is MvxNavigationPage modalNavigationPage)
                    modalNavigationPage.PopAsync();
                else
                    FormsApplication.MainPage.Navigation.PopModalAsync();
            }
            return true;
        }

        public virtual void ShowNavigationPage(
            Type view,
            MvxNavigationPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            CloseAllModals();

            var page = CreatePage(view, request);
            FormsApplication.MainPage = page;
        }

        public virtual bool CloseNavigationPage(IMvxViewModel viewModel, MvxNavigationPagePresentationAttribute attribute)
        {
            return true;
        }

        public virtual void ShowTabbedPage(
            Type view,
            MvxTabbedPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            CloseAllModals();

            var page = CreatePage(view, request);

            if(attribute.Position == TabbedPosition.Root)
            {
                if (page is TabbedPage tabbedPageRoot)
                {
                    if (attribute.WrapInNavigationPage && FormsApplication.MainPage is MvxNavigationPage currentPage)
                        currentPage.PushAsync(page);
                    else if (attribute.WrapInNavigationPage)
                        FormsApplication.MainPage = new MvxNavigationPage(page);
                    else
                        FormsApplication.MainPage = page;
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
                        navigationPage.PushAsync(tabHost);
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

                if (string.IsNullOrEmpty(page.Title))
                    page.Title = attribute.Title;
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
                else if (FormsApplication.MainPage.Navigation.ModalStack.Count > 0)
                {
                    CloseModalStack(FormsApplication.MainPage.Navigation.ModalStack);
                }
            }
        }

        protected virtual void CloseModalStack(IReadOnlyList<Page> modals)
        {
            if (modals != null && modals.Count > 0)
            {
                foreach (var modal in modals)
                {
                    modal.Navigation.PopModalAsync();
                }
            }
        }
    }
}
