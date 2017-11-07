using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Core.Pages
{
    [MvxMasterDetailPagePresentationAttribute(MasterDetailPosition.Master, WrapInNavigationPage = false)]
    public partial class MasterDetailMasterPage : MvxContentPage<MasterDetailMasterViewModel>
    {
        public MasterDetailMasterPage()
        {
            InitializeComponent();
        }
    }
}
