using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Core.Pages
{
    [MvxMasterDetailPagePresentationAttribute(MasterDetailPosition.Detail, WrapInNavigationPage = true)]
    public partial class MasterDetailDetail1Page : MvxContentPage<MasterDetailDetail1ViewModel>
    {
        public MasterDetailDetail1Page()
        {
            InitializeComponent();
        }
    }
}
