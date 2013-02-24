// MvxPhoneViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using System;
using System.Threading;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;
using Microsoft.Phone.Controls;

#endregion

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public class MvxPhoneViewDispatcher
        : MvxMainThreadDispatcher
          , IMvxViewDispatcher
          , IMvxConsumer
    {
        private readonly IMvxPhoneViewPresenter _presenter;
        private readonly PhoneApplicationFrame _rootFrame;

        public MvxPhoneViewDispatcher(IMvxPhoneViewPresenter presenter, PhoneApplicationFrame rootFrame)
            : base(rootFrame.Dispatcher)
        {
            _presenter = presenter;
            _rootFrame = rootFrame;
        }

        #region IMvxViewDispatcher Members

        public bool RequestNavigate(MvxShowViewModelRequest request)
        {
            return RequestMainThreadAction(() => _presenter.Show(request));
        }

        public bool RequestClose(IMvxViewModel toClose)
        {
            return RequestMainThreadAction(() => _presenter.Close(toClose));
        }

        public bool RequestRemoveBackStep()
        {
            return RequestMainThreadAction(() => _rootFrame.RemoveBackEntry());
        }

        #endregion
    }

    public interface IMvxPhoneViewPresenter
    {
        void Show(MvxShowViewModelRequest request);
        void Close(IMvxViewModel viewModel);
    }

    public class MvxPhoneViewPresenter
        : IMvxPhoneViewPresenter
          , IMvxConsumer
    {
        private readonly PhoneApplicationFrame _rootFrame;

        public MvxPhoneViewPresenter(PhoneApplicationFrame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        public virtual void Show(MvxShowViewModelRequest request)
        {
            try
            {
                var requestTranslator = this.Resolve<IMvxWindowsPhoneViewModelRequestTranslator>();
                var xamlUri = requestTranslator.GetXamlUriFor(request);
                _rootFrame.Navigate(xamlUri);
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error seen during navigation request to {0} - error {1}",
                               request.ViewModelType.Name, exception.ToLongString());
            }
        }

        public virtual void Close(IMvxViewModel toClose)
        {
#warning This method is cut and paste from WinRT code - would prefer a shared code file somehow
            var topMost = _rootFrame.Content;
            if (topMost == null)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning,
                               "Don't know how to close this viewmodel - no current content");
                return;
            }

            var viewTopMost = topMost as IMvxView;
            if (viewTopMost == null)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning,
                               "Don't know how to close this viewmodel - current content is not a view");
                return;
            }

            var viewModel = viewTopMost.ReflectionGetViewModel();
            if (viewModel != toClose)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning,
                               "Don't know how to close this viewmodel - viewmodel is not topmost");
                return;
            }

            if (!_rootFrame.CanGoBack)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "Can't close - can't go back");
                return;
            }

            _rootFrame.GoBack();
        }
    }
}