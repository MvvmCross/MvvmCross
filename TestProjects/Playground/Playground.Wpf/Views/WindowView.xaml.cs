using MvvmCross.Wpf.Views;
using MvvmCross.Wpf.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{
    /// <summary>
    /// Interaction logic for WindowView.xaml
    /// </summary>
    [MvxWindowPresentation(ViewType = typeof(WindowViewModel))]
    public partial class WindowView : MvxWindow
    {
        public WindowView()
        {
            InitializeComponent();
        }
    }
}
