// MvxStoreViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Windows.UI.Xaml.Controls;

namespace Cirrious.MvvmCross.WindowsStore.Views
{
    public class MvxStoreViewPresenter
        : IMvxStoreViewPresenter
    {
        private readonly Frame _rootFrame;

        public MvxStoreViewPresenter(Frame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        public virtual void Show(MvxViewModelRequest request)
        {
            try
            {
                var requestTranslator = Mvx.Resolve<IMvxViewsContainer>();
                var viewType = requestTranslator.GetViewType(request.ViewModelType);
                _rootFrame.Navigate(viewType, request);
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error seen during navigation request to {0} - error {1}", request.ViewModelType.Name,
                               exception.ToLongString());
            }
        }

        public virtual void ChangePresentation(MvxPresentationHint hint)
        {
            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }
    }
}