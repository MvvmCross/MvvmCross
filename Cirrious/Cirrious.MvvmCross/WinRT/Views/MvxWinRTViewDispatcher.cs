#region Copyright
// <copyright file="MvxWinRTViewDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WinRT.Platform;
using Windows.UI.Xaml.Controls;

#endregion

namespace Cirrious.MvvmCross.WinRT.Views
{
    public class MvxWinRTViewDispatcher 
        : MvxMainThreadDispatcher
        , IMvxViewDispatcher
        , IMvxServiceConsumer<IMvxViewsContainer>
    {
        private readonly Frame _rootFrame;

        public MvxWinRTViewDispatcher(Frame rootFrame)
            : base(rootFrame.Dispatcher)
        {
            _rootFrame = rootFrame;
        }

        #region IMvxViewDispatcher Members

        public bool RequestNavigate(MvxShowViewModelRequest request)
        {
            var requestTranslator = this.GetService<IMvxViewsContainer>();
            var viewType = requestTranslator.GetViewType(request.ViewModelType);
            return RequestMainThreadAction(() =>
                                           {
                                               try
                                               {
                                                   _rootFrame.Navigate(viewType, request);
                                               }
                                               catch (Exception exception)
                                               {
                                                   MvxTrace.Trace("Error seen during navigation request to {0} - error {1}", request.ViewModelType.Name, exception.ToLongString());
                                               }
                                           });
        }

        public bool RequestNavigateBack()
        {
            return RequestMainThreadAction(() => _rootFrame.GoBack());
        }

        public bool RequestRemoveBackStep()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}