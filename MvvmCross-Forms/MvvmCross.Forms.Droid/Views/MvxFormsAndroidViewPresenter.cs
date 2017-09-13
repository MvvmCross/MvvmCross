// MvxFormsDroidPagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com

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

        protected virtual NavigationPage MainPage => _formsApplication.MainPage as NavigationPage;

		private IMvxFormsPageLoader _formsPageLoader;
		protected IMvxFormsPageLoader FormsPageLoader
		{
			get
			{
				if (_formsPageLoader == null)
					_formsPageLoader = Mvx.Resolve<IMvxFormsPageLoader>();
				return _formsPageLoader;
			}
		}

		protected override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();

			AttributeTypesToActionsDictionary.Add(
                typeof(MvxFormsPagePresentationAttribute),
				new MvxPresentationAttributeAction
				{
					ShowAction = (view, attribute, request) => ShowFormsPage(view, (MvxFormsPagePresentationAttribute)attribute, request),
					CloseAction = (viewModel, attribute) => CloseFormsPage(viewModel, (MvxFormsPagePresentationAttribute)attribute)
				});
        }

        protected override MvvmCross.Core.Views.MvxBasePresentationAttribute GetAttributeForViewModel(Type viewModelType)
        {
			IList<MvxBasePresentationAttribute> attributes;
            if (ViewModelToPresentationAttributeMap.TryGetValue(viewModelType, out attributes))
            {
                MvxBasePresentationAttribute attribute = attributes.FirstOrDefault();
				if (attribute.ViewType?.GetInterfaces().OfType<IMvxOverridePresentationAttribute>().FirstOrDefault() is IMvxOverridePresentationAttribute view)
				{
					var presentationAttribute = view.PresentationAttribute();

					if (presentationAttribute != null)
						return presentationAttribute;
				}
				return attribute;
            }
            var viewType = ViewsContainer.GetViewType(viewModelType);

			if (viewType.IsSubclassOf(typeof(Page)))
			{
				MvxTrace.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
					$"Assuming Page presentation");
                return new MvxFormsPagePresentationAttribute(){ ViewType = viewType };
			}

            return base.GetAttributeForViewModel(viewModelType);
        }

		protected virtual void ShowFormsPage(
			Type view,
			MvxFormsPagePresentationAttribute attribute,
			MvxViewModelRequest request)
		{
            var page = Activator.CreateInstance(view) as Page;

			if (MainPage == null)
			{
				_formsApplication.MainPage = new NavigationPage(page);
			}

            if(page is IMvxContentPage contentPage)
			if (request is MvxViewModelInstanceRequest instanceRequest)
			{
				contentPage.ViewModel = instanceRequest.ViewModelInstance;
			}
			else
			{
                contentPage.ViewModel = MvxPresenterHelpers.LoadViewModel(request);
			}

            MainPage.PushAsync(page);
		}

		protected virtual bool CloseFormsPage(IMvxViewModel viewModel, MvxFormsPagePresentationAttribute attribute)
		{
            return true;
		}
    }
}
