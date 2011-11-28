using System.Windows.Input;
using Phone7.Fx.Commands;
using Phone7.Fx.Ioc;
using Phone7.Fx.Mvvm;
using Phone7.Fx.Navigation;
using Phone7.Fx.Sample.Services.Contracts;
using Phone7.Fx.Sample.Views;
using Phone7.Fx.Sample.Views.Contracts;

namespace Phone7.Fx.Sample.ViewModels
{
    [ViewModel(typeof(MainView))]
    public class MainViewModel : ViewModelBase
    {
        private readonly IHelloService _helloService;
        private readonly INavigationService _navigationService;

        public ICommand ToNextViewCommand { get; set; }

        [Injection]
        public MainViewModel(IHelloService helloService, INavigationService navigationService)
        {
            _helloService = helloService;
            _navigationService = navigationService;

            ToNextViewCommand = new DelegateCommand<object>(ToNextViewCommandHandler);
        }

        private void ToNextViewCommandHandler(object obj)
        {
            //
            _navigationService.UriFor<NextViewModel>().WithParam(m=>m.Param, 15).Navigate();
            //_navigationService.UriFor<NextViewModel>().WithParam(m=>m.Param, )
        }

        public override void InitalizeData()
        {
            Message = _helloService.SayHello();

          

            base.InitalizeData();
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged("Message"); }
        }
    }
}