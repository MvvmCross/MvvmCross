using MvvmCross.Platforms.Wpf.Presenters;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{
    [MvxRegion("Tab1Content")]

    public partial class Tab1View : MvxWpfPage
    {
        public new Tab1ViewModel ViewModel
        {
            get { return (Tab1ViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public Tab1View()
        {
            InitializeComponent();
        }
    }
}
