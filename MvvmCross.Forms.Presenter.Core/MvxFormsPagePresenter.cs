// MvxFormsPagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms.Presenter is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com

using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using System;
using Xamarin.Forms;

namespace MvvmCross.Forms.Presenter.Core
{
    public abstract class MvxFormsPagePresenter
        : MvxViewPresenter
    {
        private Application _mvxFormsApp;

        public Application MvxFormsApp
        {
            get { return _mvxFormsApp; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("MvxFormsApp cannot be null");
                }

                _mvxFormsApp = value;
            }
        }

        protected MvxFormsPagePresenter()
        {
        }

        protected MvxFormsPagePresenter(Application mvxFormsApp)
        {
            MvxFormsApp = mvxFormsApp;
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint)
            {
                var mainPage = MvxFormsApp.MainPage as NavigationPage;

                if (mainPage == null)
                {
                    Mvx.TaggedTrace("MvxFormsPresenter:ChangePresentation()", "Oops! Don't know what to do");
                }
                else
                {
                    mainPage.PopAsync();
                }
            }
        }

        public override void Show(MvxViewModelRequest request)
        {
			if (TryShowPage(request))
                return;

            Mvx.Error("Skipping request for {0}", request.ViewModelType.Name);
        }

        protected virtual void CustomPlatformInitialization(NavigationPage mainPage)
        {
        }

        private bool TryShowPage(MvxViewModelRequest request)
        {
            var page = MvxPresenterHelpers.CreatePage(request);
            if (page == null)
                return false;

            var viewModel = MvxPresenterHelpers.LoadViewModel(request);

            var mainPage = _mvxFormsApp.MainPage as NavigationPage;
            page.BindingContext = viewModel;

            if (mainPage == null)
            {
                _mvxFormsApp.MainPage = new NavigationPage(page);
                mainPage = MvxFormsApp.MainPage as NavigationPage;
                CustomPlatformInitialization(mainPage);
            }
            else
            {
                try
                {
					// calling this sync blocks UI and never navigates hence code continues regardless here
					mainPage.PushAsync(page);
                }
                catch (Exception e)
                {
                    Mvx.Error("Exception pushing {0}: {1}\n{2}", page.GetType(), e.Message, e.StackTrace);
                    return false;
                }
            }

            return true;
        }
    }
}