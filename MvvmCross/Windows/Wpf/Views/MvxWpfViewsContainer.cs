// MvxWpfViewsContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Wpf.Views
{
    using System;
    using System.Windows;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;

    public class MvxWpfViewsContainer
        : MvxViewsContainer
        , IMvxWpfViewsContainer
    {
        public virtual FrameworkElement CreateView(MvxViewModelRequest request)
        {
            var viewType = this.GetViewType(request.ViewModelType);
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