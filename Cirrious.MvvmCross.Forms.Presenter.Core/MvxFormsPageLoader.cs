using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Cirrious.MvvmCross.Forms.Presenter.Core
{
    public class MvxFormsPageLoader : IMvxFormsPageLoader
    {
        private MvxViewModelRequest _request;

        public Page LoadPage(MvxViewModelRequest request)
        {
            _request = request;

            var pageName = GetPageName();
            var pageType = GetPageType(pageName);

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

        protected virtual string GetPageName()
        {
            var viewModelName = _request.ViewModelType.Name;
            return viewModelName.Replace("ViewModel", "Page");
        }

        protected virtual Type GetPageType(string pageName)
        {
            return _request.ViewModelType.GetTypeInfo().Assembly.CreatableTypes()
                .FirstOrDefault(t => t.Name == pageName);
        }
    }
}