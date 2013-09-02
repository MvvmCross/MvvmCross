// <copyright file="MvxTouchViewsContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

using System;
using Cirrious.MvvmCross.Mac.Interfaces;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore.Exceptions;

namespace Cirrious.MvvmCross.Mac.Views
{
    public class MvxMacViewsContainer
        : MvxViewsContainer, IMvxMacViewCreator
    {
		public MvxViewModelRequest CurrentRequest { get; private set; }

		public virtual IMvxMacView CreateView(MvxViewModelRequest request)
		{
			try
			{
				CurrentRequest = request;
				var viewType = GetViewType(request.ViewModelType);
				if (viewType == null)
					throw new MvxException("View Type not found for " + request.ViewModelType);

				var view = Activator.CreateInstance(viewType) as IMvxMacView;
				if (view == null)
					throw new MvxException("View not loaded for " + viewType);
				view.Request = request;
				return view;
			}
			finally
			{
				CurrentRequest = null;
			}
		}

		public virtual IMvxMacView CreateView(IMvxViewModel viewModel)
		{
			var request = new MvxViewModelInstanceRequest(viewModel);
			var view = CreateView(request);
			return view;
		}    }
}