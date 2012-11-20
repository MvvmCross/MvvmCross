using Cirrious.MvvmCross.WindowsPhone.Views;
using CustomerManagement.AutoViews.Core.ViewModels;

namespace CustomerManagement.WindowsPhone.Views
{
    public class BaseCustomerListView : MvxPhonePage<CustomerListViewModel> { }

    [MvxPhoneView("/Views/CustomerListView.xaml")]
    public partial class CustomerListView : BaseCustomerListView
    {
        // Constructor
        public CustomerListView()
        {
            InitializeComponent();

            ApplicationTitle.Text = "Customer Management";
            PageTitle.Text = "Customers";
        }
    }
}