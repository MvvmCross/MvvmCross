// MvxWpfSimpleViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows;

namespace Cirrious.MvvmCross.Wpf.Views
{
#warning Delete this class - see https://github.com/slodge/MvvmCross/issues/359
    [Obsolete("Please use MvxSimpleWpfViewPresenter instead - this class will be deleted in future versions")]
    public class MvxWpfSimpleViewPresenter
        : MvxWpfViewPresenter
    {
        private readonly Window _mainWindow;

        public MvxWpfSimpleViewPresenter(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Present(FrameworkElement frameworkElement)
        {
            _mainWindow.Content = frameworkElement;
        }  
    }
}