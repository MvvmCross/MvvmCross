﻿// MvxRelativeLayout.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Collections.Specialized;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Binding.Droid.Views
{
    [Register("mvvmcross.binding.droid.views.MvxRelativeLayout")]
    public class MvxRelativeLayout
        : RelativeLayout, IMvxWithChangeAdapter
    {
        public MvxRelativeLayout(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxAdapterWithChangedEvent(context))
        {
        }

        public MvxRelativeLayout(Context context, IAttributeSet attrs, IMvxAdapterWithChangedEvent adapter)
            : base(context, attrs)
        {
            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            if (adapter != null)
            {
                Adapter = adapter;
                Adapter.ItemTemplateId = itemTemplateId;
            }
            ChildViewRemoved += OnChildViewRemoved;
        }

        protected MvxRelativeLayout(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public void AdapterOnDataSetChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            this.UpdateDataSetFromChange(sender, eventArgs);
        }

        private void OnChildViewRemoved(object sender, ChildViewRemovedEventArgs childViewRemovedEventArgs)
        {
            var boundChild = childViewRemovedEventArgs.Child as IMvxBindingContextOwner;
            boundChild?.ClearAllBindings();
        }

        private IMvxAdapterWithChangedEvent _adapter;

        public IMvxAdapterWithChangedEvent Adapter
        {
            get
            {
                return _adapter;
            }
            protected set
            {
                var existing = _adapter;
                if (existing == value)
                {
                    return;
                }

                if (existing != null)
                {
                    existing.DataSetChanged -= AdapterOnDataSetChanged;
                    if (value != null)
                    {
                        value.ItemsSource = existing.ItemsSource;
                        value.ItemTemplateId = existing.ItemTemplateId;
                    }
                }

                _adapter = value;

                if (_adapter != null)
                {
                    _adapter.DataSetChanged += AdapterOnDataSetChanged;
                }
                else
                {
                    MvxLog.Instance.Warn(
                        "Setting Adapter to null is not recommended - you may lose ItemsSource binding when doing this");
                }

                if (existing != null)
                    existing.ItemsSource = null;
            }
        }

        [MvxSetToNullAfterBinding]
        public IEnumerable ItemsSource
        {
            get { return Adapter.ItemsSource; }
            set { Adapter.ItemsSource = value; }
        }

        public int ItemTemplateId
        {
            get { return Adapter.ItemTemplateId; }
            set { Adapter.ItemTemplateId = value; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ChildViewRemoved -= OnChildViewRemoved;

                if (_adapter != null)
                    _adapter.DataSetChanged -= AdapterOnDataSetChanged;
            }
            base.Dispose(disposing);
        }
    }
}
