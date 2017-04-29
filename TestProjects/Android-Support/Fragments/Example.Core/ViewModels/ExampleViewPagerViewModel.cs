using MvvmCross.Core.ViewModels;

namespace Example.Core.ViewModels
{
    public class ExampleViewPagerViewModel
        : MvxViewModel
    {
        public ExampleViewPagerViewModel()
        {
            Recycler = new RecyclerViewModel();
        }

        public RecyclerViewModel Recycler { get; }
    }
}