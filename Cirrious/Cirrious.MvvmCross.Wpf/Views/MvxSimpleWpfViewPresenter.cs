#region Copyright

// <copyright file="MvxSimpleWpfViewPresenter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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