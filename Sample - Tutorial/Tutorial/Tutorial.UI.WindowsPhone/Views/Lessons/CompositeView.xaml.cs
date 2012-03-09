using Cirrious.MvvmCross.WindowsPhone.Views;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.WindowsPhone.Views.Lessons
{
    public partial class CompositeView : BaseCompositeView
    {
        public CompositeView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            TextView.ViewModel = ViewModel.Text;
            TipView.ViewModel = ViewModel.Tip;
            PullView.ViewModel = ViewModel.Pull;
        }
    }

    public class BaseCompositeView : MvxPhonePage<CompositeViewModel> { }
}