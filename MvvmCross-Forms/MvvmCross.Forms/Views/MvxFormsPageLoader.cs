using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public class MvxFormsPageLoader : IMvxFormsPageLoader
    {
		private IMvxViewsContainer _viewFinder;

        public Page LoadPage(MvxViewModelRequest request)
        {
			var pageName = GetPageName(request);
			var pageType = GetPageType(request);

            if (pageType == null)
            {
                Mvx.Trace("Page not found for {0}", pageName);
                return null;
            }

            var page = Activator.CreateInstance(pageType) as Page;
            if (page == null)
            {
                Mvx.Error("Failed to create ContentPage {0}", pageName);
            }
            return page;
        }

		protected virtual string GetPageName(MvxViewModelRequest request)
        {
            var viewModelName = request.ViewModelType.Name;
            return viewModelName.Replace("ViewModel", "Page");
        }

		protected virtual Type GetPageType(MvxViewModelRequest request)
        {
			if (_viewFinder == null)
				_viewFinder = Mvx.Resolve<IMvxViewsContainer>();

			try
			{
				return _viewFinder.GetViewType (request.ViewModelType);
			}
			catch (KeyNotFoundException) 
			{
				var pageName = GetPageName(request);
				return request.ViewModelType.GetTypeInfo().Assembly.CreatableTypes()
					.FirstOrDefault(t => t.Name == pageName);
			}
        }
    }
}