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
                    MvxTrace.Trace($"Current root is not a TabbedPage show your own first to use custom Host. Assuming we need to create one.");
                    masterDetailHost = new MvxMasterDetailPage();
                    navigationPage.PushAsync(masterDetailHost);
                }
            }
            else if (masterDetailHost == null)
            {
                MvxTrace.Trace($"Current root is not a TabbedPage show your own first to use custom Host. Assuming we need to create one.");
                masterDetailHost = new MvxMasterDetailPage();
                FormsApplication.MainPage = masterDetailHost;
            }

            var page = CreatePage(view, request);
            if (attribute.Position == MasterDetailPosition.Master)
            {
                if (masterDetailHost.Master is NavigationPage navigationMasterPage)
                    navigationMasterPage.PushAsync(page);
                else if (attribute.WrapInNavigationPage == true)
                    masterDetailHost.Master = new MvxNavigationPage(page);
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
