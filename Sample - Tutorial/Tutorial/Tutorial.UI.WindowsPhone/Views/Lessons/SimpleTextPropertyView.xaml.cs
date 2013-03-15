using Cirrious.MvvmCross.WindowsPhone.Views;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.WindowsPhone.Views.Lessons
{
    public partial class SimpleTextPropertyView : MvxPhonePage
    {
        public new SimpleTextPropertyViewModel ViewModel
        {
            get { return (SimpleTextPropertyViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public SimpleTextPropertyView()
        {
            InitializeComponent();
        }
    }
}