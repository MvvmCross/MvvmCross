using System.Windows.Input;
using Android.Support.V17.Leanback.Widget;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Droid.Support.Leanback.Listeners
{
	/// <summary>
	/// Forwards "OnChildViewHolderSelected"-Events to a command.
	/// </summary>
	public class MvxOnChildViewHolderSelectedListener : OnChildViewHolderSelectedListener
	{
		public ICommand ItemSelection { get; set; }

		public override void OnChildViewHolderSelected(Android.Support.V7.Widget.RecyclerView parent, Android.Support.V7.Widget.RecyclerView.ViewHolder child, int position, int subposition)
		{
			base.OnChildViewHolderSelected(parent, child, position, subposition);

			var adapter = parent.GetAdapter() as RecyclerView.MvxRecyclerAdapter;
			var item = adapter?.GetItem(position);

			if (item == null)
			{
				MvxTrace.Error("Could not retrieve item from adapter. Can't pass currently selected item through!");
				return;
			}

			if (ItemSelection != null && ItemSelection.CanExecute(item))
			{
				ItemSelection.Execute(item);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ItemSelection = null;
			}
			base.Dispose(disposing);
		}
	}
}