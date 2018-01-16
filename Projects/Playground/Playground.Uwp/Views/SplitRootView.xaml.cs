using Windows.UI.Xaml;
using MvvmCross.Uwp.Attributes;
using MvvmCross.Uwp.Views;
using Playground.Core.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Playground.Uwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [MvxPagePresentation]
    public sealed partial class SplitRootView : MvxWindowsPage
    {
        public SplitRootView()
        {
            this.InitializeComponent();
        }
    }
}
