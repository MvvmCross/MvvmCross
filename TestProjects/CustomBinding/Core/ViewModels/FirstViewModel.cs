using MvvmCross.Core.ViewModels;

namespace MvvmCross.TestProjects.CustomBinding.Core.ViewModels
{
    public class FirstViewModel
        : MvxViewModel
    {
        private int _counter = 2;
        private string _hello = "Hello MvvmCross";

        public string Hello
        {
            get => _hello;
            set
            {
                _hello = value;
                RaisePropertyChanged(() => Hello);
            }
        }

        public int Counter
        {
            get => _counter;
            set
            {
                _counter = value;
                RaisePropertyChanged(() => Counter);
            }
        }
    }
}