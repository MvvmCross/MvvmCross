using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace Playground.Forms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxTabbedPagePresentation(WrapInNavigationPage = false, Title = "Tab1")]

    public partial class MixedNavTab1Page : MvxContentPage<MixedNavTab1ViewModel>
    {
        public MixedNavTab1Page()
        {
            InitializeComponent();
        }
    }
}