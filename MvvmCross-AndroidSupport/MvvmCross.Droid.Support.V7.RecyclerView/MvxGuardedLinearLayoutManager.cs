using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Java.Lang;
using MvvmCross.Platform;

namespace MvvmCross.Droid.Support.V7.RecyclerView
{
    [Register("mvvmcross.droid.support.v7.recyclerview.MvxGuardedLinearLayoutManager")]
    public class MvxGuardedLinearLayoutManager : LinearLayoutManager
    {
        public MvxGuardedLinearLayoutManager(Context context) : base(context)
        {
        }

        protected MvxGuardedLinearLayoutManager(IntPtr ptr, JniHandleOwnership transfer) : base(ptr, transfer)
        {
        }

        public override void OnLayoutChildren(Android.Support.V7.Widget.RecyclerView.Recycler recycler,
            Android.Support.V7.Widget.RecyclerView.State state)
        {
            try
            {
                base.OnLayoutChildren(recycler, state);
            }
            catch (IndexOutOfBoundsException e)
            {
                Mvx.Warning(
                    "Workaround of issue - https://code.google.com/p/android/issues/detail?id=77846#c1 - IndexOutOfBoundsException " +
                    e.Message);
            }
        }
    }
}