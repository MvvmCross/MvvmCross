// MvxWpfViewsContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using System;
using System.Windows;

namespace Cirrious.MvvmCross.Wpf.Views
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

            var viewModelLoader = Mvx.Resolve<IMvxViewModelLoader>();
            wpfView.ViewModel = viewModelLoader.LoadViewModel(request, null);

            return viewControl;
        }
    }
}