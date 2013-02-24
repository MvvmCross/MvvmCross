// MvxWinRTViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WinRT.Interfaces;
using Windows.UI.Xaml.Controls;

namespace Cirrious.MvvmCross.WinRT.Views
{
    public class MvxWinRTViewPresenter
        : IMvxWinRTViewPresenter
          , IMvxConsumer
    {
        private readonly Frame _rootFrame;

        public MvxWinRTViewPresenter(Frame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        public virtual void Show(MvxShowViewModelRequest request)
        {
            try
            {
                var requestTranslator = this.Resolve<IMvxViewsContainer>();
                var viewType = requestTranslator.GetViewType(request.ViewModelType);
                _rootFrame.Navigate(viewType, request);
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error seen during navigation request to {0} - error {1}", request.ViewModelType.Name,
                               exception.ToLongString());
            }
        }

        public virtual void Close(IMvxViewModel viewModel)
        {
#warning Should do more here - e.g. should check _rootFrame's current is a view for viewmodel
            _rootFrame.GoBack();
        }
    }
}