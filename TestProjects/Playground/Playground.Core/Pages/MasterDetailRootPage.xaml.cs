using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Core.Pages
{
    [MvxMasterDetailPagePresentationAttribute(MasterDetailPosition.Root, WrapInNavigationPage = false)]
    public partial class MasterDetailRootPage : MvxMasterDetailPage<MasterDetailRootViewModel>
    {
        public MasterDetailRootPage()
        {
            InitializeComponent();
        }
    }
}
