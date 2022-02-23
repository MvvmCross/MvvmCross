using MvvmCross.Forms.Views;
using Playground.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace Playground.Forms.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParentContentPage : MvxContentPage<ParentContentViewModel>
    {
        public ParentContentPage()
        {
            InitializeComponent();
        }
    }
}
