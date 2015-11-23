using Cirrious.MvvmCross.ViewModels;

namespace Example.Core.ViewModels
{
    public class ExampleViewPagerViewModel
        : MvxViewModel
    {
        public RecyclerViewModel Recycler { get; private set; }

        public ExampleViewPagerViewModel()
        {
            Recycler = new RecyclerViewModel();
        }
    }
}