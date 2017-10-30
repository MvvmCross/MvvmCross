// MvxFormsWindowsPhonePagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com

using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Uwp.Views;
using System;
using Xamarin.Forms;

namespace MvvmCross.Forms.Uwp.Presenters
{
    public class MvxFormsUwpViewPresenter
        : MvxWindowsViewPresenter
        , IMvxFormsViewPresenter
    {
        public MvxFormsUwpViewPresenter(IMvxWindowsFrame rootFrame) : base(rootFrame)
        {
        }

        public MvxFormsUwpViewPresenter(IMvxWindowsFrame rootFrame, MvxFormsApplication formsApplication) : this(rootFrame)
        {
            FormsApplication = formsApplication ?? throw new ArgumentNullException(nameof(formsApplication), "MvxFormsApplication cannot be null");
        }

        private MvxFormsApplication _formsApplication;
        public MvxFormsApplication FormsApplication
        {
            get { return _formsApplication; }
            set { _formsApplication = value; }
        }

        private MvxFormsPagePresenter _formsPagePresenter;
        public virtual MvxFormsPagePresenter FormsPagePresenter
        {
            get
            {
                if (_formsPagePresenter == null)
                {
                    _formsPagePresenter = new MvxFormsPagePresenter(FormsApplication, ViewsContainer, ViewModelTypeFinder);
                    _formsPagePresenter.ClosePlatformViews = ClosePlatformViews;
                    Mvx.RegisterSingleton<IMvxFormsPagePresenter>(_formsPagePresenter);
                }
                return _formsPagePresenter;
            }
            set
            {
                _formsPagePresenter = value;
            }
        }

        //TODO: Refactor to new presenter code
        public const string ModalPresentationParameter = "modal";

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint)
            {
                var mainPage = FormsApplication.MainPage as NavigationPage;

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
            var viewType = ViewsContainer.GetViewType(request.ViewModelType);
            var page = FormsPagePresenter.CreatePage(viewType, request, null);
            if (page == null)
                return false;

            var mainPage = _formsApplication.MainPage as NavigationPage;

            if (mainPage == null)
            {
                _formsApplication.MainPage = new NavigationPage(page);
                mainPage = FormsApplication.MainPage as NavigationPage;
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

        public virtual bool ClosePlatformViews()
        {
            MvxTrace.Trace($"Closing of native Views in Forms is not supported on UWP.");
            return false;
        }
    }
}