using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CustomerManagement.Core.ViewModels;
using CustomerManagement;

namespace CustomerManagement.Droid.Views
{
    [Activity(Label = "Customer Changes", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class CustomerEditView : BaseCustomerEditView<EditCustomerViewModel>
    {
		public CustomerEditView ()
			: base(Resource.Menu.customer_edit_menu)
		{			
		}        
    }
}