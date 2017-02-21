using System.Windows.Input;

namespace MvvmCross.Droid.Support.V7.RecyclerView.Grouping
{
    public interface IMvxHeaderFooterRecyclerViewAdapter : IMvxRecyclerAdapter
    {
        ICommand HeaderClickCommand { get; set; }

        ICommand FooterClickCommand { get; set; }


        bool HidesHeaderIfEmpty { get; set; }

        bool HidesFooterIfEmpty { get; set; }
    }
}