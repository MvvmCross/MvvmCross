// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Specialized;
using Android.Content;
using Android.Runtime;

namespace MvvmCross.Platforms.Android.Binding.Views
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
