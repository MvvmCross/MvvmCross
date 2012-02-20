using Cirrious.MvvmCross.Views;
using CustomerManagement.Core.ViewModels;

namespace CustomerManagement.Touch
{
    public class CustomerNewView: BaseCustomerEditView<NewCustomerViewModel>
    {
        public CustomerNewView(MvxShowViewModelRequest request)
            : base(request)
        { 
        }
    }
}