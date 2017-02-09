namespace MvvmCross.Droid.Support.V7.RecyclerView.Model
{
    public class MvxViewHolderBoundEventArgs
    {
        public MvxViewHolderBoundEventArgs(int itemPosition, object dataContext, Android.Support.V7.Widget.RecyclerView.ViewHolder holder)
        {
            ItemPosition = itemPosition;
            DataContext = dataContext;
            Holder = holder;
        }

        public int ItemPosition { get; }

        public object DataContext { get; }

        public Android.Support.V7.Widget.RecyclerView.ViewHolder Holder { get; }
    }
}