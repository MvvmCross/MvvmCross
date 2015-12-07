using Cirrious.MvvmCross.Binding.Attributes;
using System.Collections;

namespace Cirrious.MvvmCross.Droid.Support.Leanback.Adapters
{
    public interface IMvxObjectAdapter
    {
        [MvxSetToNullAfterBinding]
        IEnumerable ItemsSource { get; set; }
    }
}