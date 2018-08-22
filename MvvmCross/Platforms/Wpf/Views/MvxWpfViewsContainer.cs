// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using MvvmCross.Exceptions;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Wpf.Views
{
    public class MvxWpfViewsContainer
        : MvxViewsContainer
        , IMvxWpfViewsContainer
    {
        public virtual FrameworkElement CreateView(MvxViewModelRequest request)
        {
            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
                throw new MvxException("View Type not found for " + request.ViewModelType);

            var wpfView = CreateView(viewType) as IMvxWpfView;        

            if (request is MvxViewModelInstanceRequest instanceRequest)
            {
                wpfView.ViewModel = instanceRequest.ViewModelInstance;
            }
            else
            {
                var viewModelLoader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
                wpfView.ViewModel = viewModelLoader.LoadViewModel(request, null);
            }

            return wpfView as FrameworkElement;
        }

        public FrameworkElement CreateView(Type viewType)
        {
            var viewObject = Activator.CreateInstance(viewType);
            if (viewObject == null)
                throw new MvxException("View not loaded for " + viewType);

            var wpfView = viewObject as IMvxWpfView;
            if (wpfView == null)
                throw new MvxException("Loaded View does not have IMvxWpfView interface " + viewType);

            var viewControl = viewObject as FrameworkElement;
            if (viewControl == null)
                throw new MvxException("Loaded View is not a FrameworkElement " + viewType);

            return viewControl;
        }
    }
}
