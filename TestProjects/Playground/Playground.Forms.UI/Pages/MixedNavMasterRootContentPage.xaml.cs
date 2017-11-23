using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace Playground.Forms.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, WrapInNavigationPage = true, NoHistory = true)]
    public partial class MixedNavMasterRootContentPage : MvxContentPage<MixedNavMasterRootContentViewModel>
	{
		public MixedNavMasterRootContentPage ()
		{
			InitializeComponent ();
		}
	}
}