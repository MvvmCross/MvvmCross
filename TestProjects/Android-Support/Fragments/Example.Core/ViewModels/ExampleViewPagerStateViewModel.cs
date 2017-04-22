using MvvmCross.Core.ViewModels;

namespace Example.Core.ViewModels
{
    public class ExampleViewPagerStateViewModel
        : MvxViewModel
    {
        public RecyclerViewModel Recycler { get; private set; }

        public ExampleViewPagerStateViewModel()
        {
            Recycler = new RecyclerViewModel();
        }
    }
}