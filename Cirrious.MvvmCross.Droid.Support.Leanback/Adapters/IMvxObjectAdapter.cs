using System.Collections;
using Cirrious.MvvmCross.Binding.Attributes;

namespace Cirrious.MvvmCross.Droid.Support.Leanback.Adapters
{
	public interface IMvxObjectAdapter
	{
		[MvxSetToNullAfterBinding]
		IEnumerable ItemsSource { get; set; }
	}
}