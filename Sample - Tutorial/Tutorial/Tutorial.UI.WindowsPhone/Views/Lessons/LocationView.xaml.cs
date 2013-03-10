using Cirrious.MvvmCross.WindowsPhone.Views;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.WindowsPhone.Views.Lessons
{
    public partial class LocationView : MvxPhonePage
    {
        public new LocationViewModel ViewModel
        {
            get { return (LocationViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public LocationView()
        {
            InitializeComponent();
        }
    }
}