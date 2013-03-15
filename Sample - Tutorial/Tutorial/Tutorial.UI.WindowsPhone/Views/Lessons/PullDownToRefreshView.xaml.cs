using Cirrious.MvvmCross.WindowsPhone.Views;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.WindowsPhone.Views.Lessons
{
    public partial class PullDownToRefreshView : MvxPhonePage
    {
        public new PullToRefreshViewModel ViewModel
        {
            get { return (PullToRefreshViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public PullDownToRefreshView()
        {
            InitializeComponent();
        }
    }
}