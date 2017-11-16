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

        private IMvxFormsPagePresenter _formsPagePresenter;
        public virtual IMvxFormsPagePresenter FormsPagePresenter
        {
            get
            {
                if (_formsPagePresenter == null)
                {
                    _formsPagePresenter = new MvxFormsPagePresenter(FormsApplication, ViewsContainer, ViewModelTypeFinder, attributeTypesToActionsDictionary: AttributeTypesToActionsDictionary);
                    _formsPagePresenter.ClosePlatformViews = ClosePlatformViews;
                    Mvx.RegisterSingleton(_formsPagePresenter);
                }
                return _formsPagePresenter;
            }
            set
            {
                _formsPagePresenter = value;
            }
        }

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();

            FormsPagePresenter.RegisterAttributeTypes();
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint closeHint)
            {
                FormsPagePresenter.Close(closeHint.ViewModelToClose);

                //var mainPage = FormsApplication.MainPage as NavigationPage;

                //if (mainPage == null)
                //{
                //    Mvx.TaggedTrace("MvxFormsPresenter:ChangePresentation()", "Oops! Don't know what to do");
                //}
                //else
                //{
                //    mainPage.PopAsync();
                //}
            }
        }

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            var presentationAttribute = FormsPagePresenter.CreatePresentationAttribute(viewModelType, viewType);
            return presentationAttribute ?? base.CreatePresentationAttribute(viewModelType, viewType);
        }


        protected virtual void CustomPlatformInitialization(NavigationPage mainPage)
        {
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