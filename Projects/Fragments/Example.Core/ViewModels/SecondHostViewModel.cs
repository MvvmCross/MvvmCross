using MvvmCross.Core.ViewModels;

namespace Example.Core.ViewModels
{
	public class SecondHostViewModel
        : MvxViewModel
    {
		public SecondHostViewModel()
        {
        }

        public void ShowMenu()
        {
            ShowViewModel<HomeViewModel>();
        }
    }
}