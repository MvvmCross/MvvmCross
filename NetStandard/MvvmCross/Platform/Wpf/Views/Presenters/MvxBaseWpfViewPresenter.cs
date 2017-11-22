// MvxBaseWpfViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Wpf.Views.Presenters
{
    public abstract class MvxBaseWpfViewPresenter
        : MvxViewPresenter, IMvxWpfViewPresenter
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
                MvxTrace.Error("Error seen during navigation request to {0} - error {1}", request.ViewModelType.Name,
                               exception.ToLongString());
            }
        }

        public abstract void Present(FrameworkElement frameworkElement);

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }
    }
}