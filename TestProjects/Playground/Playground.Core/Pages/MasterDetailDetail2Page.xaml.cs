using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Core.Pages
{
    [MvxMasterDetailPagePresentationAttribute(MasterDetailPosition.Detail, WrapInNavigationPage = true)]
    public partial class MasterDetailDetail2Page : MvxContentPage<MasterDetailDetail2ViewModel>
    {
        public MasterDetailDetail2Page()
        {
            InitializeComponent();
        }
    }
}
