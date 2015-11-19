// MvxPhoneViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Microsoft.Phone.Controls;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public class MvxPhoneViewPresenter
        : MvxViewPresenter, IMvxPhoneViewPresenter
    {
        private readonly PhoneApplicationFrame _rootFrame;

        protected PhoneApplicationFrame RootFrame => _rootFrame;

        public MvxPhoneViewPresenter(PhoneApplicationFrame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        public override void Show(MvxViewModelRequest request)
        {
            try
            {
                var requestTranslator = Mvx.Resolve<IMvxPhoneViewModelRequestTranslator>();
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

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint)
            {
                Close((hint as MvxClosePresentationHint).ViewModelToClose);
                return;
            }

            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }

        public virtual void Close(IMvxViewModel viewModel)
        {
            var currentView = _rootFrame.Content as IMvxView;
            if (currentView == null)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe has no current page");
                return;
            }

            if (currentView.ViewModel != viewModel)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return;
            }

            if (!_rootFrame.CanGoBack)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe refuses to go back");
                return;
            }

            _rootFrame.GoBack();
        }
    }
}