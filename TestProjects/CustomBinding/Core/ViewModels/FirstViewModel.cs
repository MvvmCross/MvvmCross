using MvvmCross.Core.ViewModels;

namespace MvvmCross.TestProjects.CustomBinding.Core.ViewModels
{
    public class FirstViewModel 
		: MvxViewModel
    {
		private string _hello = "Hello MvvmCross";
        public string Hello
		{ 
			get { return _hello; }
			set { _hello = value; RaisePropertyChanged(() => Hello); }
		}

        private int _counter = 2;
        public int Counter
        {
            get { return _counter; }
            set { _counter = value; RaisePropertyChanged(() => Counter); }
        }
        
    }
}
