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
                return new MvxRelayCommand(() =>
                                               {
#warning TODO - need to do the real save
                                                   RequestNavigateBack();
                                               });
            }
        }
    }
}
