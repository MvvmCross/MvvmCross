using MvvmCross.Platforms.Wpf.Presenters;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{
    [MvxRegion("Tab2Content")]
    public partial class Tab2View : MvxWpfPage
    {
        public new Tab2ViewModel ViewModel
        {
            get { return (Tab2ViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public Tab2View()
        {
            InitializeComponent();
        }
    }
}
