using System.Collections.Generic;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.ShortNames;
using Cirrious.MvvmCross.ViewModel;
using CustomerManagement.Shared.Model;

namespace CustomerManagement.ViewModels
{
    public class CustomerListViewModel : MvxViewModel 
    {
        public List<Customer> Customers { get; set; }

        public MvxRelayCommand<MvxSimpleSelectionChangedEventArgs> SelectionChanged
        {
            get
            {
                return new MvxRelayCommand<MvxSimpleSelectionChangedEventArgs>(OnSelectionChanged);
            }
        }

        public MvxRelayCommand AddCommand
        {
            get
            {
                return new MvxRelayCommand(OnAdd);
            }
        }

        private void OnAdd()
        {
            RequestNavigate<EditCustomerViewModel>("New");
        }

        private void OnSelectionChanged(MvxSimpleSelectionChangedEventArgs args)
        {
            if (args.AddedItems.Count != 1)
                return;

            var c = args.AddedItems[0] as Customer;
            if (c == null)
                return;

            RequestNavigate<DetailsCustomerViewModel>(new StringDict() { { "customerId", c.ID } });
        }
    }
}
