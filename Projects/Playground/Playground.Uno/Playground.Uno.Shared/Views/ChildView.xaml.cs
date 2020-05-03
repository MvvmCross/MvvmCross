using MvvmCross.Platforms.Uap.Views;
using Playground.Core.ViewModels;

namespace Playground.Uwp.Views
{
    public sealed partial class ChildView
    {
        public ChildView()
        {
            this.InitializeComponent();
        }
    }

    public abstract partial class ChildBase: MvxWindowsPage<ChildViewModel>
    {

    }
}
