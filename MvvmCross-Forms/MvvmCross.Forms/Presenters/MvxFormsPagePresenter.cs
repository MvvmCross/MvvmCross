﻿// MvxFormsPagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com

using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Core;
using MvvmCross.Platform;
using Xamarin.Forms;

namespace MvvmCross.Forms.Presenters
{
    public abstract class MvxFormsPagePresenter
        : MvxViewPresenter
    {
        public const string ModalPresentationParameter = "modal";

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

        private void SetupForBinding(Page page, IMvxViewModel viewModel, MvxViewModelRequest request)
        {
            var mvxContentPage = page as IMvxContentPage;
            if (mvxContentPage != null) {
                mvxContentPage.Request = request;
                mvxContentPage.ViewModel = viewModel;
            } else {
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

            var mainPage = _mvxFormsApp.MainPage as NavigationPage;

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
                    // check for modal presentation parameter
                    string modalParameter;
                    if (request.PresentationValues != null && request.PresentationValues.TryGetValue(ModalPresentationParameter, out modalParameter) && bool.Parse(modalParameter))
                        mainPage.Navigation.PushModalAsync(page);
                    else
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

        public override void Close(IMvxViewModel toClose)
        {
        }
    }
}