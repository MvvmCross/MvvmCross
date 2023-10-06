// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

using MvvmCross.Platforms.WinUi.Presenters.Attributes;
using MvvmCross.Platforms.WinUi.Views;
using Playground.Core.ViewModels;

namespace Playground.WinUi.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [MvxSplitViewPresentation(Position = SplitPanePosition.Content)]
    public sealed partial class SplitDetailView : SplitDetailViewPage
    {
        public SplitDetailView()
        {
            this.InitializeComponent();
        }
    }

    public abstract class SplitDetailViewPage : MvxWindowsPage<SplitDetailViewModel>
    {
    }
}
