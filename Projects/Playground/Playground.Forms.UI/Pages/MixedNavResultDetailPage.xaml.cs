using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Playground.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace Playground.Forms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, WrapInNavigationPage = true)]
    public partial class MixedNavResultDetailPage : MvxContentPage<MixedNavResultDetailViewModel>
    {
        public MixedNavResultDetailPage()
        {
            InitializeComponent();
        }
    }
}
