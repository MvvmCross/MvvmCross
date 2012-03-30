#region Copyright
// <copyright file="MvxWinRTViewPresenter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WinRT.Interfaces;
using Windows.UI.Xaml.Controls;

namespace Cirrious.MvvmCross.WinRT.Views
{
    public class MvxWinRTViewPresenter
        : IMvxWinRTViewPresenter
          , IMvxServiceConsumer<IMvxViewsContainer>
    {
        private readonly Frame _rootFrame;

        public MvxWinRTViewPresenter(Frame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        public void Show(MvxShowViewModelRequest request)
        {
            try
            {
                var requestTranslator = this.GetService<IMvxViewsContainer>();
                var viewType = requestTranslator.GetViewType(request.ViewModelType);
                _rootFrame.Navigate(viewType, request);
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error seen during navigation request to {0} - error {1}", request.ViewModelType.Name, exception.ToLongString());
            }
        }

        public void Close(IMvxViewModel viewModel)
        {
#warning Should do more here - e.g. should check _rootFrame's current is a view for viewmodel
            _rootFrame.GoBack();
        }
    }
}