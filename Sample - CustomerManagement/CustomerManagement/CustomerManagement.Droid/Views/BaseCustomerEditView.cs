using Android.Views;
using CustomerManagement.Core.ViewModels;

namespace CustomerManagement.Droid.Views
{
    public class BaseCustomerEditView<TViewModel> : BaseView<TViewModel>
        where TViewModel : BaseEditCustomerViewModel
    {
        private readonly int _whichMenu;

        public BaseCustomerEditView(int whichMenu)
        {
            _whichMenu = whichMenu;
        }

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_EditCustomerView);
        }

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
}