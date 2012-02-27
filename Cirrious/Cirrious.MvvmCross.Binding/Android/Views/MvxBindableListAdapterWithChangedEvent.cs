using System;
using Android.Content;

namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public class MvxBindableListAdapterWithChangedEvent
        : MvxBindableListAdapter 
    {
        public MvxBindableListAdapterWithChangedEvent(Context context)
            : base(context)
        {
        }

        public event EventHandler DataSetChanged;

        public override void NotifyDataSetChanged()
        {
            base.NotifyDataSetChanged();

            var handler = DataSetChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}