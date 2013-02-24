// MvxMacViewsContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region Copyright

// <copyright file="MvxTouchViewsContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

using System;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

#endregion

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxMacViewsContainer
        : MvxViewsContainer
          , IMvxMacViewCreator
          
    {
        #region IMvxTouchViewCreator Members

        public virtual IMvxMacView CreateView(MvxShowViewModelRequest request)
        {
            var view = ConstructView(request);
            var viewModel = ConstructViewModel(request);
            view.ViewModel = viewModel;
            return view;
        }

        protected virtual IMvxMacView ConstructView(MvxShowViewModelRequest request)
        {
            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
                throw new MvxException("View Type not found for " + request.ViewModelType);
            var view = Activator.CreateInstance(viewType, request) as IMvxMacView;
            if (view == null)
                throw new MvxException("View not loaded for " + viewType);
            return view;
        }

        protected virtual IMvxViewModel ConstructViewModel(MvxShowViewModelRequest request)
        {
            var loader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(request);
        }

        /*
		protected virtual IMvxMacView CreateGenericView (Type viewType, MvxShowViewModelRequest request)
		{
			MvxTrace.Trace("Creating generic view - note that these are now not recommended");
			var view = Activator.CreateInstance(viewType, request) as IMvxMacView;
			if (view == null)
				throw new MvxException("View not loaded for " + viewType);
			return view;
		}
		*/

        public virtual IMvxMacView CreateView(IMvxViewModel viewModel)
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