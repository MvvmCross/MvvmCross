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