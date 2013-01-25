// MvxWinRTViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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
          , IMvxServiceConsumer
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
                MvxTrace.Trace("Error seen during navigation request to {0} - error {1}", request.ViewModelType.Name,
                               exception.ToLongString());
            }
        }

        public void Close(IMvxViewModel viewModel)
        {
#warning Should do more here - e.g. should check _rootFrame's current is a view for viewmodel
            _rootFrame.GoBack();
        }
    }
}