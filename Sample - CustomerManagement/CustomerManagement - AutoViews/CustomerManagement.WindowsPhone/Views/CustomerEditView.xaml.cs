using Cirrious.MvvmCross.WindowsPhone.Views;
using CustomerManagement.AutoViews.Core.ViewModels;

namespace CustomerManagement.WindowsPhone.Views
{
    public class BaseCustomerEditView : MvxPhonePage<EditCustomerViewModel> { }

    [MvxPhoneView("/Views/CustomerEditView.xaml")]
    public partial class CustomerEditView : BaseCustomerEditView
    {
        public CustomerEditView()
        {
            InitializeComponent();
        }
    }
}