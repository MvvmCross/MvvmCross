using System;
using System.Collections.Generic;
using Android.Content;

namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public class MvxBindableArrayAdapterWithChangedEvents<T>
        : MvxBindableArrayAdapter<T>
    {
        public MvxBindableArrayAdapterWithChangedEvents(Context context)
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