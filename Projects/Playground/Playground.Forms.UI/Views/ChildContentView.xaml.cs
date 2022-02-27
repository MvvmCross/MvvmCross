using MvvmCross.Forms.Views;
using Playground.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace Playground.Forms.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChildContentView : MvxContentView<ChildContentViewModel>
    {
        public ChildContentView()
        {
            InitializeComponent();
        }
    }
}
