using System;
using System.Windows.Input;
using Phone7.Fx.Commands;
using Phone7.Fx.Ioc;
using Phone7.Fx.Mvvm;
using Phone7.Fx.Navigation;
using Phone7.Fx.Sample.Views;

namespace Phone7.Fx.Sample.ViewModels
{
    [ViewModel(typeof(NextView))]
    public class NextViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public ICommand ToNextViewCommand { get; set; }

          [Injection]
        public NextViewModel(INavigationService navigationService )
          {
              _navigationService = navigationService;
              ToNextViewCommand = new DelegateCommand<object>(ToNextViewCommandHandler);
          }

        private void ToNextViewCommandHandler(object obj)
        {
            int value = (new Random().Next(0, 50));
            _navigationService.UriFor<NextViewModel>().WithParam(m => m.Param, value).Navigate();
        }

        public int Param { get; set; }

        public override void InitalizeData()
        {
            var e = this.Param;
        }
    }
}