﻿using System.Windows.Input;
using Android.Support.V17.Leanback.Widget;
using Android.Support.V7.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Droid.Support.V17.Leanback.Listeners
{
	/// <summary>
	/// Forwards "OnChildViewHolderSelected"-Events to a command.
	/// </summary>
	public class MvxOnChildViewHolderSelectedListener 
        : OnChildViewHolderSelectedListener
	{
		public ICommand ItemSelection { get; set; }

		public override void OnChildViewHolderSelected(RecyclerView parent, RecyclerView.ViewHolder child, int position, int subposition)
		{
			base.OnChildViewHolderSelected(parent, child, position, subposition);

			var adapter = parent.GetAdapter() as MvxRecyclerAdapter;
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