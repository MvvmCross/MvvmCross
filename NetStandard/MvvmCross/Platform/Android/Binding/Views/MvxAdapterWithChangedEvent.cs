// MvxAdapterWithChangedEvent.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Specialized;
using Android.Content;
using Android.Runtime;

namespace MvvmCross.Binding.Droid.Views
{
    public class MvxAdapterWithChangedEvent
        : MvxAdapter
        , IMvxAdapterWithChangedEvent
    {
        public MvxAdapterWithChangedEvent(Context context)
            : base(context)
        {
        }

        protected MvxAdapterWithChangedEvent(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public event EventHandler<NotifyCollectionChangedEventArgs> DataSetChanged;

        public override void NotifyDataSetChanged()
        {
            NotifyDataSetChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public override void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
        {
            base.NotifyDataSetChanged(e);

            DataSetChanged?.Invoke(this, e);
        }
    }
}