using Android.Views;

namespace CustomerManagement.Droid.Views
{
    /*
    public class BaseCustomerEditView<TViewModel> : BaseDialogView<TViewModel> 
        where TViewModel : BaseEditCustomerViewModel
    {
        private readonly int _whichMenu;
		
		public BaseCustomerEditView (int whichMenu)
		{
			_whichMenu = whichMenu;
		}

        protected override string JsonText
        {
            get
            {
                return _jsonText;
            }
        }

        private const string _jsonText = @"
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
                    'Key':'Entry',
                    'Properties':{
                        'Caption':'Name',
                        'Value':'@MvxBind:{\'Path\':\'Customer.Name\'}'
                    }
                },
                {
                    'Key':'Entry',
                    'Properties':{
                        'Caption':'Website',
                        'Value':'@MvxBind:{\'Path\':\'Customer.Website\'}'
                    }
                },
                {
                    'Key':'Entry',
                    'Properties':{
                        'Caption':'Phone',
                        'Value':'@MvxBind:{\'Path\':\'Customer.PrimaryPhone\'}'
                    }
                }
            ]
        },
        {
            'Properties':{
                'Header':'Primary Address'
             },
            'Elements':[
                {
                    'Key':'Entry',
                    'Properties':{
                        'Caption':'Address',
                        'Value':'@MvxBind:{\'Path\':\'Customer.PrimaryAddress.Street1\'}'
                    }
                },
                {
                    'Key':'Entry',
                    'Properties':{
                        'Caption':'Address1',
                        'Value':'@MvxBind:{\'Path\':\'Customer.PrimaryAddress.Street2\'}'
                    }
                },
                {
                    'Key':'Entry',
                    'Properties':{
                        'Caption':'City',
                        'Value':'@MvxBind:{\'Path\':\'Customer.PrimaryAddress.City\'}'
                    }
                },
                {
                    'Key':'Entry',
                    'Properties':{
                        'Caption':'State',
                        'Value':'@MvxBind:{\'Path\':\'Customer.PrimaryAddress.State\'}'
                    }
                },
                {
                    'Key':'Entry',
                    'Properties':{
                        'Caption':'Zip',
                        'Value':'@MvxBind:{\'Path\':\'Customer.PrimaryAddress.Zip\'}'
                    }
                }
            ]
        },
    ]
}
";

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(_whichMenu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.save_customer:
#warning TODO - why can't you use `.SaveCommand.Execute(null);` here?
                    ViewModel.DoSave(); //
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
     */
}