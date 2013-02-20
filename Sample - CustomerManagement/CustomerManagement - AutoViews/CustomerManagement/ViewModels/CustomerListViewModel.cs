using System.Windows.Input;
using Cirrious.CrossCore.Commands;
using Cirrious.MvvmCross.AutoView;
using Cirrious.MvvmCross.AutoView.Auto;
using Cirrious.MvvmCross.AutoView.Auto.List;
using Cirrious.MvvmCross.AutoView.Auto.Menu;
using Cirrious.MvvmCross.AutoView.Interfaces;
using CrossUI.Core.Descriptions;
using CustomerManagement.AutoViews.Core.Interfaces.Models;
using CustomerManagement.AutoViews.Core.Models;

namespace CustomerManagement.AutoViews.Core.ViewModels
{
    public class CustomerListViewModel
        : BaseViewModel
        , IMvxAutoListViewModel

    {
        public CustomerListViewModel()
        {
            
        }

        public IObservableCollection<Customer> Customers { get { return DataStore.Customers; } }

        public ICommand CustomerSelectedCommand
        {
            get
            {
                return new MvxRelayCommand<Customer>(DoCustomerSelect);
            }
        }

        public void DoCustomerSelect(Customer customer)
        {
            RequestNavigate<DetailsCustomerViewModel>(new { customerId = customer.ID });
        }

        public ICommand AddCommand
        {
            get
            {
                return new MvxRelayCommand(DoAdd);
            }
        }

        public void DoAdd()
        {
            RequestNavigate<NewCustomerViewModel>();
        }

        public bool SupportsAutoView(string type)
        {
            switch (type)
            {
                case MvxAutoViewConstants.List:
                    return true;

                case MvxAutoViewConstants.Menu:
                    return true;

                default:
                    return false;
            }
        }

        public KeyedDescription GetAutoView(string type)
        {
            switch (type)
            {
                case MvxAutoViewConstants.List:
                    return GetListAutoView();

                case MvxAutoViewConstants.Menu:
                    return GetMenuAutoView();

                default:
                    return null;
            }
        }

        private KeyedDescription GetMenuAutoView()
        {
            var auto = new ParentMenuAuto()
                           {
                               new MenuAuto(caption: "New",
                                   longCaption: "New Customer",
                                   icon: "ic_menu_add",
                                   command: () => AddCommand),
                           };

            return auto.ToParentMenuDescription();
        }

        private KeyedDescription GetListAutoView()
        {
            var list = new ListAuto(key: "General",
                                    itemsSource: () => Customers,
                                    selectedCommand: () => CustomerSelectedCommand);

            list.DefaultLayout = new ListLayoutAuto<Customer>(key: "General",
                                                    layoutName: "TitleAndSubTitle")
                                     {
                                         new BindingAuto<Customer>("Title", c => c.Name),
                                         new BindingAuto<Customer>("SubTitle", c => c.Website)
                                     };

            return list.ToDescription();
        }
    }
}
