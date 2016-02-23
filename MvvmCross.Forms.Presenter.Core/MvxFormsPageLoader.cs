using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using System;
using Xamarin.Forms;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Presenter.Core
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
				_viewFinder = Mvx.Resolve<IMvxViewsContainer> ();

			return _viewFinder.GetViewType (request.ViewModelType);
        }
    }
}