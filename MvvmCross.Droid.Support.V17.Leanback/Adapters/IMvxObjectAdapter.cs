using System.Collections;
using Cirrious.MvvmCross.Binding.Attributes;

namespace MvvmCross.Droid.Support.V17.Leanback.Adapters
{
    public interface IMvxObjectAdapter
    {
        [MvxSetToNullAfterBinding]
        IEnumerable ItemsSource { get; set; }
    }
}