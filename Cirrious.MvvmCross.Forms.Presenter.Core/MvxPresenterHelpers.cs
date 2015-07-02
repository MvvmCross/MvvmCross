// MvxPresenterHelpers.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// Cirrious.MvvmCross.Forms.Presenter is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com
﻿
﻿using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
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
			IMvxFormsPageLoader viewPageLoader;
			Mvx.TryResolve(out viewPageLoader);
			if (viewPageLoader == null)
			{
				// load default instead
				viewPageLoader = new MvxFormsPageLoader();
				Mvx.RegisterSingleton(viewPageLoader);
			}
			var page = viewPageLoader.LoadPage(request);
			return page;
		}
	}
}
