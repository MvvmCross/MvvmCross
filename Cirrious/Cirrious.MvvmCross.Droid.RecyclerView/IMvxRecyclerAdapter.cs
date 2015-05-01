using System.Collections;
using Cirrious.MvvmCross.Binding.Attributes;

namespace Cirrious.MvvmCross.Droid.RecyclerView
{
    public interface IMvxRecyclerAdapter
    {
        [MvxSetToNullAfterBinding]
        IEnumerable ItemsSource { get; set; }

        int ItemTemplateId { get; set; }

        object GetItem(int position);
    }
}