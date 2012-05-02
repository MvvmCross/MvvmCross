#region Copyright
// <copyright file="MvxPhoneViewDispatcher.cs" company="Cirrious">
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
using System.Linq;
using System.Threading;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;
using Microsoft.Phone.Controls;

#endregion

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public class MvxPhoneViewDispatcher 
        : MvxMainThreadDispatcher
        , IMvxViewDispatcher
        , IMvxServiceConsumer<IMvxWindowsPhoneViewModelRequestTranslator>
    {
        private readonly PhoneApplicationFrame _rootFrame;

        public MvxPhoneViewDispatcher(PhoneApplicationFrame rootFrame)
            : base(rootFrame.Dispatcher)
        {
            _rootFrame = rootFrame;
        }

        #region IMvxViewDispatcher Members

        public bool RequestNavigate(MvxShowViewModelRequest request)
        {
            var requestTranslator = this.GetService<IMvxWindowsPhoneViewModelRequestTranslator>();
            var xamlUri = requestTranslator.GetXamlUriFor(request);
            return RequestMainThreadAction(() =>
                                           {
                                               try
                                               {
                                                   _rootFrame.Navigate(xamlUri);
                                               }
                                               catch (ThreadAbortException)
                                               {
                                                   throw;
                                               }
                                               catch (Exception exception)
                                               {
                                                   MvxTrace.Trace("Error seen during navigation request to {0} - error {1}", request.ViewModelType.Name, exception.ToLongString());
                                               }
                                           });
        }

        public bool RequestClose(IMvxViewModel toClose)
        {
            return RequestMainThreadAction(() =>
                                           {
                                               var topMost = _rootFrame.Content;
                                               if (topMost == null)
                                               {
                                                   MvxTrace.Trace(MvxTraceLevel.Warning, "Don't know how to close this viewmodel - no current content");
                                                   return;
                                               }

                                               var viewTopMost = topMost as IMvxView;
                                               if (viewTopMost == null)
                                               {
                                                   MvxTrace.Trace(MvxTraceLevel.Warning, "Don't know how to close this viewmodel - current content is not a view");
                                                   return;
                                               }

                                               var viewModel = viewTopMost.ReflectionGetViewModel();
                                               if (viewModel != toClose)
                                               {
                                                   MvxTrace.Trace(MvxTraceLevel.Warning, "Don't know how to close this viewmodel - viewmodel is not topmost");
                                                   return;
                                               }

                                               if (!_rootFrame.CanGoBack)
                                               {
                                                   MvxTrace.Trace(MvxTraceLevel.Warning, "Can't close - can't go back");
                                                   return;
                                               }

                                               _rootFrame.GoBack();
                                           });
        }

        public bool RequestRemoveBackStep()
        {
            return RequestMainThreadAction(() => _rootFrame.RemoveBackEntry());
        }

        #endregion
    }
}