#region Copyright
// <copyright file="MvxWpfViewPresenter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
using System;
using System.Windows;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Wpf.Interfaces;

namespace Cirrious.MvvmCross.Wpf.Views
{
    public abstract class MvxWpfViewPresenter
        : IMvxWpfViewPresenter
        , IMvxServiceConsumer<IMvxViewsContainer>
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
                MvxTrace.Trace("Error seen during navigation request to {0} - error {1}", request.ViewModelType.Name, exception.ToLongString());
            }
        }

        public abstract void Present(FrameworkElement frameworkElement);

        public void Close(IMvxViewModel viewModel)
        {
            throw new NotImplementedException("Need to consider what to do here");
        }
    }
}