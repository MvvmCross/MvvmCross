using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Telephony;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Plugins.Json;
using CustomerManagement.Core.ViewModels;


namespace CustomerManagement.Droid.Views
{
    [Activity(Label = "Customer Info", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class DetailsCustomerView : BaseView<DetailsCustomerViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_DetailsCustomerView);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.customer_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.change_customer:
                    ViewModel.DoEdit();
                    return true;
                case Resource.Id.delete_customer:
                    ViewModel.DoDelete();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}