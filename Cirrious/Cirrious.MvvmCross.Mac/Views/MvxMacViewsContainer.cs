// <copyright file="MvxTouchViewsContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

using System;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Mac.Interfaces;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore.Exceptions;

namespace Cirrious.MvvmCross.Mac.Views
{
    public class MvxMacViewsContainer
        : MvxViewsContainer, IMvxMacViewCreator
    {
		#region IMvxMacViewCreator implementation

		public IMvxMacView CreateView (MvxShowViewModelRequest request)
		{
			var viewType = GetViewType(request.ViewModelType);
			if (viewType == null)
				throw new MvxException("View Type not found for " + request.ViewModelType);
			
			var view = Activator.CreateInstance(viewType) as IMvxMacView;
			if (view == null)
				throw new MvxException("View not loaded for " + viewType);

			var requestProperty = view.GetType().GetProperty("ViewModelRequest");
			if (requestProperty == null)
				throw new MvxException("ViewModelRequest Property missing for " + view.GetType());
			requestProperty.SetValue(view, request, null);

			return view;
		}

		public IMvxMacView CreateView (IMvxViewModel viewModel)
		{
			var viewModelType = viewModel.GetType();
			var request = MvxShowViewModelRequest.GetDefaultRequest(viewModelType);
			var view = CreateView(request);
			var viewModelProperty = view.GetType().GetProperty("ViewModel");
			if (viewModelProperty == null)
				throw new MvxException("ViewModel Property missing for " + view.GetType());
			
			if (!viewModelProperty.CanWrite)
				throw new MvxException("ViewModel Property readonly for " + view.GetType());
			
			viewModelProperty.SetValue(view, viewModel, null);
			return view;
		}

		#endregion
    }
}