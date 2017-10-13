using MvvmCross.Core.Views;
using MvvmCross.Wpf.Views;
using MvvmCross.Wpf.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{
    /// <summary>
    /// Interaction logic for WindowChildView.xaml
    /// </summary>
    public partial class WindowChildView : MvxWpfView<WindowChildViewModel>, IMvxOverridePresentationAttribute
    {
        public WindowChildView()
        {
            InitializeComponent();
        }

        public MvxBasePresentationAttribute PresentationAttribute()
        {
            return new MvxContentPresentationAttribute
            {
                WindowIdentifier = $"{nameof(WindowView)}.ViewModel.ParentNo",
                StackNavigation = false
            };
        }
    }
}
