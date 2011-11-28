using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace CustomerManagement.ViewModels
{
    public class EditCustomerViewModel : BaseCustomerViewModel
    {
        public IMvxCommand SaveCommand
        {
            get
            {
#warning TODO - need to do Navigate better
                return new MvxRelayCommand(() =>
                                               {
                                                   // TODO!
                                               });
            }
        }
    }
}
