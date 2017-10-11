using System;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Views.Attributes;
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

        public virtual void ShowCarouselPage(
            Type view,
            MvxCarouselPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CreatePage(view, request) as CarouselPage;
            FormsApplication.MainPage = page;
        }

        public virtual bool CloseCarouselPage(IMvxViewModel viewModel, MvxCarouselPagePresentationAttribute attribute)
        {
            return false;
        }

        public virtual void ShowContentPage(
            Type view,
            MvxContentPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CreatePage(view, request);

            //TODO: Check ModalStack and push there if applied

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
            var masterDetailHost = FormsApplication.MainPage as MasterDetailPage;
            if (masterDetailHost == null && FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                masterDetailHost = navigationPage.CurrentPage as MasterDetailPage;
                if (masterDetailHost == null)
                {
                    MvxTrace.Trace($"Current root is not a MasterDetailPage show your own first to use custom Host. Assuming we need to create one.");
                    masterDetailHost = CreateMasterDetailHost(view, request);

                    // Crash here if you dont have master and detail property set for the masterDetailHost
                    navigationPage.PushAsync(masterDetailHost);
                }
            }
            else if (masterDetailHost == null)
            {
                MvxTrace.Trace($"Current root is not a MasterDetailPage show your own first to use custom Host. Assuming we need to create one.");
                masterDetailHost = CreateMasterDetailHost(view, request);
                FormsApplication.MainPage = masterDetailHost;
            }


            // Creating page here a second time when method CreateMasterDetailHost was executed already.
            // Maybe the better approach is to not push the masterDetailHost in line 103, do it later instead when everything is
            // setup correctly
            var page = CreatePage(view, request);

            if (attribute.Position == MasterDetailPosition.Master)
            {
                if (masterDetailHost.Master is NavigationPage navigationMasterPage)
                    navigationMasterPage.PushAsync(page);
                else if (attribute.WrapInNavigationPage == true) {

                    // In the beginning I have not set the WrapInNavigationPage to true in my master page, so this part got executed.
                    // It crashed saying "Title must be set in master" even tho it was set already. I think this happends because the new created
                    // Navigation page needs the title of page. like so: navPage.Title = page.Title before setting Master property of masterDetailHost
                    masterDetailHost.Master = new MvxNavigationPage(page);

                    // Another thing came into my mind while typing, who on earth wants to have navigation inside a master / navigation drawer. From
                    // my view thats odd. If you dont want to support this you could set the default of WrapInNavigationPage to false 
                    // inside of MvxMasterDetailPagePresentation

                }
                else
                    masterDetailHost.Master = page;
            }
            else if (attribute.Position == MasterDetailPosition.Detail)
            {
                if (masterDetailHost.Detail is NavigationPage navigationDetailPage)
                    navigationDetailPage.PushAsync(page);
                else if (attribute.WrapInNavigationPage == true)
                    masterDetailHost.Detail = new MvxNavigationPage(page);
                else
                    masterDetailHost.Detail = page;
            }
        }

        private MasterDetailPage CreateMasterDetailHost(Type view, MvxViewModelRequest request)
        {
            var masterDetailHost = new MvxMasterDetailPage
            {
                Master = CreatePage(view, request),

                // TODO: Good way to retrieve view and viewmodel type for detail here if possible. If not create content page
                Detail = new ContentPage()
            };

            return masterDetailHost;
        }

        public virtual bool CloseMasterDetailPage(IMvxViewModel viewModel, MvxMasterDetailPagePresentationAttribute attribute)
        {
            var masterDetailHost = FormsApplication.MainPage as MasterDetailPage;
            if (masterDetailHost == null && FormsApplication.MainPage is MvxNavigationPage navigationPage)
            {
                masterDetailHost = navigationPage.CurrentPage as MasterDetailPage;
            }
            if (attribute.Position == MasterDetailPosition.Master)
            {
                if (masterDetailHost.Master is NavigationPage navigationMasterPage)
                    navigationMasterPage.PopAsync();
            }
            else if (attribute.Position == MasterDetailPosition.Detail)
            {
                if (masterDetailHost.Detail is NavigationPage navigationDetailPage)
                    navigationDetailPage.PopAsync();
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
                if(attribute.WrapInNavigationPage == true)
                    navigationPage.CurrentPage.Navigation.PushModalAsync(new MvxNavigationPage(page));
                else
                    navigationPage.CurrentPage.Navigation.PushModalAsync(page);
            }
        }

        public virtual bool CloseModal(IMvxViewModel viewModel, MvxModalPresentationAttribute attribute)
        {
            if (FormsApplication.MainPage is MvxNavigationPage navigationPage)
                navigationPage.CurrentPage.Navigation.PopModalAsync();
            return true;
        }

        public virtual void ShowNavigationPage(
            Type view,
            MvxNavigationPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
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
            else if (tabHost == null)
            {
                MvxTrace.Trace($"Current root is not a TabbedPage show your own first to use custom Host. Assuming we need to create one.");
                tabHost = new MvxTabbedPage();
                FormsApplication.MainPage = tabHost;
            }

            var page = CreatePage(view, request);
            if (string.IsNullOrEmpty(page.Title))
                page.Title = attribute.Title;
            tabHost.Children.Add(page);
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
    }
}
