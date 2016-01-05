// MvxWpfViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Wpf.Views
{
    using System;
    using System.Windows;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    public abstract class MvxWpfViewPresenter
        : MvxViewPresenter, IMvxWpfViewPresenter
    {
        public override void Show(MvxViewModelRequest request)
        {
            try
            {
                var loader = Mvx.Resolve<IMvxSimpleWpfViewLoader>();
                var view = loader.CreateView(request);
                this.Present(view);
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
            if (this.HandlePresentationChange(hint)) return;

            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }
    }
}