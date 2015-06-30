using Cirrious.MvvmCross.ViewModels;

namespace $rootnamespace$.ViewModels
{
    public class FirstViewModel : MvxViewModel
    {
        public FirstViewModel() { Hello = "Hello, from MvvmCross"; }

        private string _hello;
        public string Hello
        {
            get { return _hello; }
            set
            {
                _hello = value; 
                RaisePropertyChanged(() => Hello);
            }
        }
    }
}
