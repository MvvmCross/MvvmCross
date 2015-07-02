// MvxPresenterHelpers.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// Cirrious.MvvmCross.Forms.Presenter is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com
﻿
﻿using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Cirrious.MvvmCross.Forms.Presenter.Core
{
    public static class MvxPresenterHelpers
    {
        public static IMvxViewModel LoadViewModel(MvxViewModelRequest request)
        {
            var viewModelLoader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = viewModelLoader.LoadViewModel(request, null);
            return viewModel;
        }

        public static Page CreatePage(MvxViewModelRequest request)
        {
            var viewModelName = request.ViewModelType.Name;
            var pageName = viewModelName.Replace("ViewModel", "Page");
            var pageType = request.ViewModelType.GetTypeInfo().Assembly.CreatableTypes()
                                  .FirstOrDefault(t => t.Name == pageName);
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
    }
}
