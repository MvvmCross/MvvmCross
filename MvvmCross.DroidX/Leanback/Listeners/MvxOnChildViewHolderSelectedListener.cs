// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Windows.Input;
using AndroidX.Leanback.Widget;
using Microsoft.Extensions.Logging;
using MvvmCross.DroidX.RecyclerView;

namespace MvvmCross.DroidX.Leanback.Listeners
{
    /// <summary>
    /// Forwards "OnChildViewHolderSelected"-Events to a command.
    /// </summary>
    public class MvxOnChildViewHolderSelectedListener
        : OnChildViewHolderSelectedListener
    {
        public ICommand ItemSelection { get; set; }

        public override void OnChildViewHolderSelected(AndroidX.RecyclerView.Widget.RecyclerView parent, AndroidX.RecyclerView.Widget.RecyclerView.ViewHolder child, int position, int subposition)
        {
            base.OnChildViewHolderSelected(parent, child, position, subposition);

            var adapter = parent.GetAdapter() as IMvxRecyclerAdapter;
            var item = adapter?.GetItem(position);

            if (item == null)
            {
                MvxAndroidLog.Instance.Log(LogLevel.Error, "Could not retrieve item from adapter. Can't pass currently selected item through!");
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
