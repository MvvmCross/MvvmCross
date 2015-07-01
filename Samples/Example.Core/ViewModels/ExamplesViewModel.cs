using Cirrious.MvvmCross.ViewModels;

namespace Example.Core.ViewModels
{
    public class ExamplesViewModel
        : MvxViewModel
    {
        public RecyclerViewModel Recycler { get; private set; }

        public ExamplesViewModel() {
            Recycler = new RecyclerViewModel();
        }
    }
}