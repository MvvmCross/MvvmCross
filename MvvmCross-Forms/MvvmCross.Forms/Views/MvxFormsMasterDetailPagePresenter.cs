using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.ViewModels;
using MvvmCross.Platform;
using Xamarin.Forms;

/*
namespace MvvmCross.Forms.Views
{
    /// <summary>
    /// Presenter provinding MasterDetailPage functionality for the MainView in a MvxForms App.
    /// </summary>
    public abstract class MvxFormsMasterDetailPagePresenter : MvxViewPresenter, IMvxFormsPagePresenter
    {
        private MvxFormsApplication _formsApplication;
        public MvxFormsApplication FormsApplication
        {
            get { return _formsApplication; }
            set { _formsApplication = value; }
        }

        protected MvxFormsMasterDetailPagePresenter()
        {
        }

        protected MvxFormsMasterDetailPagePresenter(MvxFormsApplication formsApplication)
        {
            FormsApplication = formsApplication ?? throw new ArgumentNullException(nameof(formsApplication), "MvxFormsApp cannot be null");
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint)
            {
                var mainPage = FormsApplication.MainPage as MasterDetailPage;

                if (mainPage == null)
                {
                    Mvx.TaggedTrace("MvxFormsPresenter:ChangePresentation()", "Oops! Don't know what to do");
                }
                else
                {
                    // Perform pop on the Detail Page and launch RootContentPageActivated if root has been reached
                    var navPage = mainPage.Detail as NavigationPage;
                    navPage.PopAsync();
                    if (navPage.Navigation.NavigationStack.Count == 1)
                        RootContentPageActivated();
                }
            }
        }

        public override void Show(MvxViewModelRequest request)
        {
			if (TryShowPage(request))
                return;

            Mvx.Error("Skipping request for {0}", request.ViewModelType.Name);
        }

        protected virtual void CustomPlatformInitialization(MasterDetailPage mainPage)
        {
        }

        private void SetupForBinding(Page page, IMvxViewModel viewModel, MvxViewModelRequest request)
        {
            var contentPage = page as IMvxContentPage;
            if (contentPage != null) {
                contentPage.Request = request;
                contentPage.ViewModel = viewModel;                
            } 
            else {
                page.BindingContext = viewModel;
            }
        }

        private bool TryShowPage(MvxViewModelRequest request)
        {
            var page = MvxPresenterHelpers.CreatePage(request);
            if (page == null)
                return false;

            var viewModel = MvxPresenterHelpers.LoadViewModel(request);            

            SetupForBinding(page, viewModel, request);
            
            var mainPage = FormsApplication.MainPage as MasterDetailPage;

            // Initialize the MasterDetailPage container            
            if (mainPage == null)
            {
                // The ViewModel used should inherit from MvxMasterDetailViewModel, so we can create a new ContentPage for use in the Detail page
                var masterDetailViewModel = viewModel as MvxMasterDetailViewModel;
                if (masterDetailViewModel == null)
                    throw new InvalidOperationException("ViewModel should inherit from MvxMasterDetailViewModel<T>");

                Page rootContentPage = null;
                if (masterDetailViewModel.RootContentPageViewModelType != null)
                {
                    var rootContentRequest = new MvxViewModelRequest(masterDetailViewModel.RootContentPageViewModelType, null, null);

                    var rootContentViewModel = MvxPresenterHelpers.LoadViewModel(rootContentRequest);
                    rootContentPage = MvxPresenterHelpers.CreatePage(rootContentRequest);
                    SetupForBinding(rootContentPage, rootContentViewModel, rootContentRequest);
                }
                else
                    rootContentPage = new ContentPage();
                
                var navPage = new NavigationPage(rootContentPage);

                //Hook to Popped event to launch RootContentPageActivated if proceeds
                navPage.Popped += (sender, e) =>
                {
                    if (navPage.Navigation.NavigationStack.Count == 1)
                        RootContentPageActivated();
                };

                mainPage = new MasterDetailPage
                {
                    Master = page,
                    Detail = navPage 
                };                

                FormsApplication.MainPage = mainPage;
                CustomPlatformInitialization(mainPage);
            }
            else
            {
                // Functionality for clearing the navigation stack before pushing to new Page (for example in a menu with multiple options)
                if (request.PresentationValues != null)
                {
                    if (request.PresentationValues.ContainsKey("NavigationMode") && request.PresentationValues["NavigationMode"] == "ClearStack")
                    {
                        mainPage.Detail.Navigation.PopToRootAsync();
                        if (Device.Idiom == TargetIdiom.Phone)
                            mainPage.IsPresented = false;
                    }
                }

                try
                {
                    var nav = mainPage.Detail as NavigationPage;

                    // calling this sync blocks UI and never navigates hence code continues regardless here
                    nav.PushAsync(page);
                }
                catch (Exception e)
                {
                    Mvx.Error("Exception pushing {0}: {1}\n{2}", page.GetType(), e.Message, e.StackTrace);
                    return false;
                }
            }

            return true;
        }

        private void RootContentPageActivated()
        {
            var mainPage = Application.Current.MainPage as MasterDetailPage;            
            (mainPage.Master.BindingContext as MvxMasterDetailViewModel)?.RootContentPageActivated();
        }

        public override void Close(IMvxViewModel toClose)
        {
        }
    }
}*/
