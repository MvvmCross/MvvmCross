using Cirrious.MvvmCross.WindowsPhone.Views;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.WindowsPhone.Views.Lessons
{
    public partial class LocationView : BaseLocationView
    {
        public LocationView()
        {
            InitializeComponent();
        }
    }

    public class BaseLocationView : MvxPhonePage<LocationViewModel> { }
}