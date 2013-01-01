// MvxBindableListAdapterWithChangedEvent.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Specialized;
using Android.Content;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxBindableListAdapterWithChangedEvent
        : MvxBindableListAdapter
    {
        public MvxBindableListAdapterWithChangedEvent(Context context)
            : base(context)
        {
        }

        public event EventHandler<NotifyCollectionChangedEventArgs> DataSetChanged;

        public override void NotifyDataSetChanged()
        {
            this.NotifyDataSetChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public override void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
        {
            base.NotifyDataSetChanged(e);

            var handler = DataSetChanged;
            if (handler != null)
                handler(this, e);
        }
    }
}