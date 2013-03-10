using Cirrious.MvvmCross.WindowsPhone.Views;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.WindowsPhone.Views.Lessons
{
    public partial class TipView : MvxPhonePage
    {
        public new TipViewModel ViewModel
        {
            get { return (TipViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public TipView()
        {
            InitializeComponent();
        }
    }
}