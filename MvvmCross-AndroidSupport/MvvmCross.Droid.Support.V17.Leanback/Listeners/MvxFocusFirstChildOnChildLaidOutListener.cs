using Android.Support.V17.Leanback.Widget;
using Android.Views;

namespace MvvmCross.Droid.Support.V17.Leanback.Listeners
{
	/// <summary>
	/// Requests focus for first laid out child.
	/// </summary>
	public class MvxFocusFirstChildOnChildLaidOutListener : Java.Lang.Object, IOnChildLaidOutListener
	{
		public void OnChildLaidOut(ViewGroup parent, View view, int position, long id)
		{
			if (view != null && position == 0)
			{
				view.RequestFocus();
			}
		}
	}
}