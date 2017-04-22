using System.Windows.Input;
using MvvmCross.Droid.Support.V7.RecyclerView.Grouping.DataConverters;

namespace MvvmCross.Droid.Support.V7.RecyclerView.Grouping
{
    public interface IMvxGroupableRecyclerViewAdapter : IMvxRecyclerAdapter
    {
        ICommand GroupHeaderClickCommand { get; set; }

        IMvxGroupedDataConverter GroupedDataConverter { get; set; }
    }
}