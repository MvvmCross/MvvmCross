// MvxFormsDroidPagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobe�a Mori�n, @CobenaMarcos, marcoscm@me.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using Xamarin.Forms;

namespace MvvmCross.Forms.Droid.Views
{
    public class MvxFormsAndroidViewPresenter
        : MvxAppCompatViewPresenter, IMvxFormsViewPresenter
    {
		public MvxFormsAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
		{
		}

        public MvxFormsAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies, MvxFormsApplication formsApplication) : this(androidViewAssemblies)
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

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();

            FormsPagePresenter.RegisterAttributeTypes(AttributeTypesToActionsDictionary);
        }

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (viewType.IsSubclassOf(typeof(ContentPage)))
            {
                MvxLog.Instance.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming ContentPage presentation");
                return new MvxContentPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.IsSubclassOf(typeof(CarouselPage)))
            {
                MvxLog.Instance.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming CarouselPage presentation");
                return new MvxCarouselPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.IsSubclassOf(typeof(TabbedPage)))
            {
                MvxLog.Instance.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming TabbedPage presentation");
                return new MvxTabbedPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.IsSubclassOf(typeof(MasterDetailPage)))
            {
                MvxLog.Instance.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming MasterDetailPage presentation");
                return new MvxMasterDetailPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.IsSubclassOf(typeof(NavigationPage)))
            {
                MvxLog.Instance.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                               $"Assuming NavigationPage presentation");
                return new MvxNavigationPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }

            return base.CreatePresentationAttribute(viewModelType, viewType);
        }

        public virtual bool ClosePlatformViews()
        {
            CloseFragments();
            if (!(CurrentActivity is MvxFormsAppCompatActivity || CurrentActivity is MvxFormsApplicationActivity) && 
                !(CurrentActivity is MvxSplashScreenActivity || CurrentActivity is MvxSplashScreenAppCompatActivity))
                CurrentActivity.Finish();
            return true;
        }
    }
}
