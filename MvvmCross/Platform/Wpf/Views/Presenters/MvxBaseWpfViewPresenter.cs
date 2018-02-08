// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using MvvmCross.Base;
using MvvmCross.Base.Exceptions;
using MvvmCross.Base.Logging;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Platform.Wpf.Views.Presenters
{
    public abstract class MvxBaseWpfViewPresenter
        : MvxWpfViewPresenter, IMvxWpfViewPresenter
    {
        public override void Show(MvxViewModelRequest request)
        {
            try
            {
                var loader = Mvx.Resolve<IMvxWpfViewLoader>();
                var view = loader.CreateView(request);
                Present(view);
            }
            catch (Exception exception)
            {
                MvxLog.Instance.Error("Error seen during navigation request to {0} - error {1}", request.ViewModelType.Name,
                               exception.ToLongString());
            }
        }

        public abstract void Present(FrameworkElement frameworkElement);

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            MvxLog.Instance.Warn("Hint ignored {0}", hint.GetType().Name);
        }
    }
}
