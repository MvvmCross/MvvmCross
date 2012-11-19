using Android.App;
using Android.Views;
using CustomerManagement.Core.ViewModels;

namespace CustomerManagement.Droid.Views
{
    [Activity(Label = "New Customer", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class CustomerNewView : BaseCustomerEditView<NewCustomerViewModel>
    {
        public CustomerNewView ()
			: base(Resource.Menu.customer_new_menu)
        {        	
        }
    }
}