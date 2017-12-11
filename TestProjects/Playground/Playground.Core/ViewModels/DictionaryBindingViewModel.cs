using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Core.ViewModels
{
    public class DictionaryBindingViewModel : BaseViewModel
    {
        int _value = 0;
        public int Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        IMvxAsyncCommand _closeCommand;
        public IMvxAsyncCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this)));


        IMvxCommand _incrementCommand;
        private IMvxNavigationService _navigationService;

        public IMvxCommand IncrementCommand =>
            _incrementCommand ?? (_incrementCommand = new MvxCommand(Increment));

        public DictionaryBindingViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private void Increment()
        {
            if(Value < 3)
            {
                Value++;
            }
            else
            {
                Value = 0;
            }
        }
    }
}
