using Cirrious.MvvmCross.WindowsPhone.Views;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.WindowsPhone.Views.Lessons
{
    public partial class PullDownToRefreshView : BasePullDownToRefreshView
    {
        public PullDownToRefreshView()
        {
            InitializeComponent();
        }
    }

    public class BasePullDownToRefreshView : MvxPhonePage<PullToRefreshViewModel> { }
}