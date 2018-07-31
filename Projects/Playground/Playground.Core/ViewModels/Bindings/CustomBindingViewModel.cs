using System;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.Bindings
{
    public class CustomBindingViewModel
        : MvxNavigationViewModel
    {
        private IMvxAsyncCommand _closeCommand;

        private int _counter = 2;

        private DateTime _date = DateTime.Now;

        private string _hello = "Hello MvvmCross";

        public CustomBindingViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
        }

        public string Hello
        {
            get => _hello;
            set => SetProperty(ref _hello, value);
        }

        public IMvxAsyncCommand CloseCommand => _closeCommand ??
                                                (_closeCommand = new MvxAsyncCommand(async () =>
                                                    await NavigationService.Close(this)));

        public int Counter
        {
            get => _counter;
            set => SetProperty(ref _counter, value);
        }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }
    }
}
