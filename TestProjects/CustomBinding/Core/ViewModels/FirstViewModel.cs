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
            set { SetProperty(ref _hello, value); }
		}

        private int _counter = 2;
        public int Counter
        {
            get { return _counter; }
            set { SetProperty(ref _counter, value); }
        }
    }
}
