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


namespace CustomerManagement.Droid.Views
{
    /*
    [Activity(Label = "Customer Info", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class DetailsCustomerView : BaseDialogView<DetailsCustomerViewModel>
    {
        protected override string JsonText
        {
            get
            {
                var json = Mvx.Resolve<IMvxJsonConverter>();
                var text = json.SerializeObject(ViewModel.DefaultView());
                return text; //_jsonText;
            }
        }

        protected override string JsonText
        {
            get { return _jsonText; }
        }

        private const string _jsonText =
            @"
{
    'Key':'Root',
    'Properties':{
        'Caption':'TestRootElement'
    },
    'Sections':[
        {
            'Properties':{
                'Header':'Customer Info'
             },
            'Elements':[
                {
                    'Key':'String',
                    'Properties':{
                        'Caption':'ID',
                        'Value':'@MvxBind:{\'Path\':\'Customer.ID\'}'
                    }
                },
                {
                    'Key':'String',
                    'Properties':{
                        'Caption':'Name',
                        'Value':'@MvxBind:{\'Path\':\'Customer.Name\'}'
                    }
                },
                {
                    'Key':'String',
                    'Properties':{
                        'Caption':'Website',
                        'Value':'@MvxBind:{\'Path\':\'Customer.Website\'}',
                        'SelectedCommand':'@MvxBind:{\'Path\':\'ShowWebsiteCommand\'}'
                    }
                },
                {
                    'Key':'String',
                    'Properties':{
                        'Caption':'Phone',
                        'Value':'@MvxBind:{\'Path\':\'Customer.PrimaryPhone\'}',
                        'SelectedCommand':'@MvxBind:{\'Path\':\'CallCustomerCommand\'}'
                    }
                }
            ]
        },
        {
            'Properties':{
                'Header':'General Info'
             },
            'Elements':[
                {
                    'Key':'StyledMultiline',
                    'Properties':{
                        'Caption':'Address',
                        'Value':'@MvxBind:{\'Path\':\'Customer.PrimaryAddress\'}',
                        'SelectedCommand':'@MvxBind:{\'Path\':\'ShowOnMapCommand\'}'
                    }
                }
            ]
        },
    ]
}
";
		
		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.customer_menu, menu);
			return true;
		}
		
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.change_customer:
                ViewModel.DoEdit();
                return true;
            case Resource.Id.delete_customer:
                ViewModel.DoDelete();
                return true;
			}				
			return base.OnOptionsItemSelected (item);
		}
    }
     */
}