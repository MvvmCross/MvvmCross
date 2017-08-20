// MvxSimpleWpfViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Windows;
using System.Windows.Controls;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using System.Collections.Generic;
using System.Linq;

namespace MvvmCross.Wpf.Views.Presenters
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

            MvxTrace.Warning($"Could not close ViewModel type {toClose.GetType().Name}");
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