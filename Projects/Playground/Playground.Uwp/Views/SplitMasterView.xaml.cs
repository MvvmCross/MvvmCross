

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

using MvvmCross.Platform.Uap.Presenters.Attributes;
using MvvmCross.Platform.Uap.Views;

namespace Playground.Uwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [MvxSplitViewPresentation(Position = SplitPanePosition.Pane)]
    public sealed partial class SplitMasterView : MvxWindowsPage
    {
        public SplitMasterView()
        {
            this.InitializeComponent();
        }
    }
}
