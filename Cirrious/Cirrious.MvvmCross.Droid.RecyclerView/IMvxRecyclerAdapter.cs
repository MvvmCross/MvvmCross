using System.Collections;
using System.Windows.Input;
using Cirrious.MvvmCross.Binding.Attributes;

namespace Cirrious.MvvmCross.Droid.RecyclerView
{
    public interface IMvxRecyclerAdapter
    {
        [MvxSetToNullAfterBinding]
        IEnumerable ItemsSource { get; set; }

        int ItemTemplateId { get; set; }
        ICommand ItemClick { get; set; }
        ICommand ItemLongClick { get; set; }

        object GetItem(int position);
    }
}