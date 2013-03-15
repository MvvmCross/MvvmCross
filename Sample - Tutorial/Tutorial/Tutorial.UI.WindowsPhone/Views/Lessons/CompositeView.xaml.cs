using Cirrious.MvvmCross.WindowsPhone.Views;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.WindowsPhone.Views.Lessons
{
    public partial class CompositeView : MvxPhonePage
    {
        public new CompositeViewModel ViewModel
        {
            get { return (CompositeViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

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
}