using MvvmCross.Core.ViewModels;

namespace Example.Core.ViewModels
{
    public class ExampleViewPagerStateViewModel
        : MvxViewModel
    {
        public ExampleViewPagerStateViewModel()
        {
            Recycler = new RecyclerViewModel();
        }

        public RecyclerViewModel Recycler { get; }
    }
}