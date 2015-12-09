// MvxSimpleWpfViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Windows;
using System.Windows.Controls;

namespace Cirrious.MvvmCross.Wpf.Views
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
    }
}