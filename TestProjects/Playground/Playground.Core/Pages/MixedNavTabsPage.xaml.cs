using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace Playground.Core.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail)]
    public partial class MixedNavTabsPage : MvxTabbedPage<MixedNavTabsViewModel>
	{
		public MixedNavTabsPage ()
		{
			InitializeComponent ();
		}
	}
}