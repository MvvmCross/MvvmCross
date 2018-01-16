using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Forms.Pages
{
    [MvxContentPagePresentation(WrapInNavigationPage = false)]
    public partial class MixedNavFirstPage : MvxContentPage<MixedNavFirstViewModel>
    {
        public MixedNavFirstPage()
        {
            InitializeComponent();
        }
    }
}