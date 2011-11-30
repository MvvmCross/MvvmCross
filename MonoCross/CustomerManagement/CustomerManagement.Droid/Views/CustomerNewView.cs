using Android.App;
using Android.Views;
using CustomerManagement.ViewModels;

namespace CustomerManagement.Droid.Views
{
    [Activity(Label = "New Customer", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class CustomerNewView : BaseCustomerEditView<NewCustomerViewModel>
    {
        
    }
}