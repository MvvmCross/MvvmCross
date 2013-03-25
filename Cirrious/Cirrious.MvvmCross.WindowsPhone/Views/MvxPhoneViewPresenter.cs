// MvxPhoneViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Microsoft.Phone.Controls;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public class MvxPhoneViewPresenter
        : IMvxPhoneViewPresenter
    {
        private readonly PhoneApplicationFrame _rootFrame;

        public MvxPhoneViewPresenter(PhoneApplicationFrame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        public virtual void Show(MvxViewModelRequest request)
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

        public virtual void ChangePresentation(MvxPresentationHint hint)
        {
            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }
    }
}