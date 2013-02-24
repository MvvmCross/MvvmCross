// MvxWpfViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Wpf.Interfaces;

namespace Cirrious.MvvmCross.Wpf.Views
{
    public abstract class MvxWpfViewPresenter
        : IMvxWpfViewPresenter
          , IMvxConsumer
    {
        public void Show(MvxShowViewModelRequest request)
        {
            try
            {
                var loader = this.GetService<IMvxSimpleWpfViewLoader>();
                var view = loader.CreateView(request);
                Present(view);
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error seen during navigation request to {0} - error {1}", request.ViewModelType.Name,
                               exception.ToLongString());
            }
        }

        public abstract void Present(FrameworkElement frameworkElement);

        public void Close(IMvxViewModel viewModel)
        {
            throw new NotImplementedException("Need to consider what to do here");
        }
    }
}