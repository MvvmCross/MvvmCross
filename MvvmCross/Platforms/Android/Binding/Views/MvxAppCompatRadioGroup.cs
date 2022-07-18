// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;

namespace MvvmCross.Platforms.Android.Binding.Views
{
    [Register("mvvmcross.platforms.android.binding.views.MvxAppCompatRadioGroup")]
    public class MvxAppCompatRadioGroup : RadioGroup, IMvxWithChangeAdapter
    {
        public MvxAppCompatRadioGroup(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxAdapterWithChangedEvent(context))
        {
        }

        public MvxAppCompatRadioGroup(Context context, IAttributeSet attrs, IMvxAdapterWithChangedEvent adapter)
            : base(context, attrs)
        {
            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            if (adapter != null)
            {
                Adapter = adapter;
                Adapter.ItemTemplateId = itemTemplateId;
            }

            ChildViewAdded += OnChildViewAdded;
            ChildViewRemoved += OnChildViewRemoved;
        }

        protected MvxAppCompatRadioGroup(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public void AdapterOnDataSetChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            this.UpdateDataSetFromChange(sender, eventArgs);
        }

        private void OnChildViewAdded(object sender, ChildViewAddedEventArgs args)
        {
            //var li = (args.Child as MvxListItemView);
            var radioButton = args.Child as AppCompatRadioButton;

            // radio buttons require an id so that they get un-checked correctly
            if (radioButton?.Id == NoId)
            {
                radioButton.Id = GenerateViewId();
            }
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
                    MvxBindingLog.Warning(
                        "Setting Adapter to null is not recommended - you may lose ItemsSource binding when doing this");
                }

                if (existing != null)
                    existing.ItemsSource = null;
            }
        }

        [MvxSetToNullAfterBinding]
        public IEnumerable ItemsSource
        {
            get => Adapter.ItemsSource;
            set => Adapter.ItemsSource = value;
        }

        public int ItemTemplateId
        {
            get => Adapter.ItemTemplateId;
            set => Adapter.ItemTemplateId = value;
        }

        private static long _nextGeneratedViewId = 1;

        private static new int GenerateViewId()
        {
            for (; ; )
            {
                int result = (int)Interlocked.Read(ref _nextGeneratedViewId);

                // aapt-generated IDs have the high byte nonzero; clamp to the range under that.
                int newValue = result + 1;
                if (newValue > 0x00FFFFFF)
                {
                    // Roll over to 1, not 0.
                    newValue = 1;
                }

                if (Interlocked.CompareExchange(ref _nextGeneratedViewId, newValue, result) == result)
                {
                    return result;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_adapter != null)
                    _adapter.DataSetChanged -= AdapterOnDataSetChanged;

                ChildViewAdded -= OnChildViewAdded;
                ChildViewRemoved -= OnChildViewRemoved;
            }

            base.Dispose(disposing);
        }
    }
}
