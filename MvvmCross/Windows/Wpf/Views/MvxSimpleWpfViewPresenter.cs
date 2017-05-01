// MvxSimpleWpfViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;

namespace MvvmCross.Wpf.Views
{
    using System.Windows;
    using System.Windows.Controls;

    public class MvxSimpleWpfViewPresenter
        : MvxWpfViewPresenter
    {
        private readonly ContentControl _contentControl;

        public MvxSimpleWpfViewPresenter(ContentControl contentControl)
        {
            this._contentControl = contentControl;
        }

        public override void Present(FrameworkElement frameworkElement)
        {
            this._contentControl.Content = frameworkElement;
        }

        public override void Close(IMvxViewModel toClose)
        {
        }
    }
}