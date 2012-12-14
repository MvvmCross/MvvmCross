#region Copyright
// <copyright file="MvxWpfViewsContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Windows;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Wpf.Interfaces;

namespace Cirrious.MvvmCross.Wpf.Views
{
    public class MvxWpfViewsContainer
        : MvxViewsContainer 
        , IMvxServiceConsumer<IMvxViewModelLoader>, IMvxSimpleWpfViewLoader
    {
        public FrameworkElement CreateView(MvxShowViewModelRequest request)
        {
            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
                throw new MvxException("View Type not found for " + request.ViewModelType);

            // , request
            var viewObject = Activator.CreateInstance(viewType);
            if (viewObject == null)
                throw new MvxException("View not loaded for " + viewType);

            var wpfView = viewObject as IMvxWpfView;
            if (wpfView == null)
                throw new MvxException("Loaded View does not have IMvxWpfView interface " + viewType);

            var viewControl = viewObject as FrameworkElement;
            if (viewControl == null)
                throw new MvxException("Loaded View is not a FrameworkElement " + viewType);

            var viewModelLoader = this.GetService<IMvxViewModelLoader>();
            wpfView.ViewModel = viewModelLoader.LoadViewModel(request);

            return viewControl;
        }

        /*
        #region IMvxTouchViewCreator Members

        public virtual IMvxTouchView CreateView(MvxShowViewModelRequest request)
        {
            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
                throw new MvxException("View Type not found for " + request.ViewModelType);

            var view = Activator.CreateInstance(viewType, request) as IMvxTouchView;
            if (view == null)
                throw new MvxException("View not loaded for " + viewType);
            return view;
        }

        public virtual IMvxTouchView CreateView(IMvxViewModel viewModel)
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
         */
    }
}