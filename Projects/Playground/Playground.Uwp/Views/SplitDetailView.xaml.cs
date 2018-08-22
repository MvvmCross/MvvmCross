

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

using MvvmCross.Platforms.Uap.Presenters.Attributes;
using MvvmCross.Platforms.Uap.Views;

namespace Playground.Uwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [MvxSplitViewPresentation(Position = SplitPanePosition.Content)]
    public sealed partial class SplitDetailView : MvxWindowsPage
    {
        public SplitDetailView()
        {
            this.InitializeComponent();
        }
    }
}
