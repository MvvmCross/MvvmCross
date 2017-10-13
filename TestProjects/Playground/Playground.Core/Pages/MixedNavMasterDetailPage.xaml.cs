using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace Playground.Core.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Master, WrapInNavigationPage = false)]
    public partial class MixedNavMasterDetailPage : MvxContentPage<MixedNavMasterDetailViewModel>
    {
        public MixedNavMasterDetailPage()
        {
            InitializeComponent();
        }
    }
}