// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using MvvmCross.Platform.Wpf.Views;

namespace MvvmCross.Platform.Wpf.Presenters
{
    public class MvxSimpleWpfViewPresenter
        : MvxBaseWpfViewPresenter
    {
        private readonly ContentControl _contentControl;
        private Stack<FrameworkElement> _frameworkElements = new Stack<FrameworkElement>();

        public MvxSimpleWpfViewPresenter(ContentControl contentControl)
        {
            _contentControl = contentControl;
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            base.ChangePresentation(hint);

            if (hint is MvxClosePresentationHint)
            {
                Close((hint as MvxClosePresentationHint).ViewModelToClose);
            }
        }

        public override void Present(FrameworkElement frameworkElement)
        {
            _frameworkElements.Push(frameworkElement);
            _contentControl.Content = frameworkElement;
        }

        public override void Close(IMvxViewModel toClose)
        {
            if (_frameworkElements.Any() && CloseFrameworkElement(toClose))
                return;

            MvxLog.Instance.Warn($"Could not close ViewModel type {toClose.GetType().Name}");
        }

        protected virtual bool CloseFrameworkElement(IMvxViewModel toClose)
        {
            var view = _frameworkElements.Peek() as IMvxWpfView;
            if (toClose == view?.ViewModel)
            {
                _frameworkElements.Pop(); // Pop closing view
                if (_frameworkElements.Any())
                {
                    Present(_frameworkElements.Pop());
                    return true;
                }
            }
            return false;
        }
    }
}
