using Cirrious.MvvmCross.WindowsPhone.Views;
using CustomerManagement.AutoViews.Core.ViewModels;

namespace CustomerManagement.WindowsPhone.Views
{
    public class BaseCustomerNewView : MvxPhonePage<NewCustomerViewModel> { }

    [MvxPhoneView("/Views/CustomerNewView.xaml")]
    public partial class CustomerNewView : BaseCustomerNewView
    {
        public CustomerNewView()
        {
            InitializeComponent();
        }
    }
}