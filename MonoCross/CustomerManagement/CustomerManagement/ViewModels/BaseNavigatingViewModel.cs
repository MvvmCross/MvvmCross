using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.ViewModel;

namespace CustomerManagement.ViewModels
{
    public class BaseNavigatingViewModel : MvxViewModel
    {
        public IMvxCommand BackCommand
        {
            get
            {
                return new MvxRelayCommand(() => RequestNavigateBack());
            }
        }
    }
}