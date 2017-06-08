// MvxSimpleWpfViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Windows;
using System.Windows.Controls;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Wpf.Views
{
    public class MvxSimpleWpfViewPresenter
        : MvxWpfViewPresenter
    {
        private readonly ContentControl _contentControl;

        public MvxSimpleWpfViewPresenter(ContentControl contentControl)
        {
            _contentControl = contentControl;
        }

        public override void Present(FrameworkElement frameworkElement)
        {
            _contentControl.Content = frameworkElement;
        }

        public override void Close(IMvxViewModel toClose)
        {
        }
    }
}