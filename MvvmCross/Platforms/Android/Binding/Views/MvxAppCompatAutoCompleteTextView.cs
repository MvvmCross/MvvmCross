// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using MvvmCross.Binding.Attributes;

namespace MvvmCross.Platforms.Android.Binding.Views
{
    [Register("mvvmcross.platforms.android.binding.views.MvxAppCompatAutoCompleteTextView")]
    public class MvxAppCompatAutoCompleteTextView
        : AppCompatAutoCompleteTextView
    {
        public MvxAppCompatAutoCompleteTextView(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxFilteringAdapter(context))
        {
            // note - we shouldn't realy need both of these... but we do
            ItemClick += OnItemClick;
            ItemSelected += OnItemSelected;
        }

        public MvxAppCompatAutoCompleteTextView(Context context, IAttributeSet attrs,
            MvxFilteringAdapter adapter) : base(context, attrs)
        {
            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;
            Adapter = adapter;
            ItemClick += OnItemClick;
        }

        protected MvxAppCompatAutoCompleteTextView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
            OnItemClick(itemClickEventArgs.Position);
        }

        private void OnItemSelected(object sender, AdapterView.ItemSelectedEventArgs itemSelectedEventArgs)
        {
            OnItemSelected(itemSelectedEventArgs.Position);
        }

        protected virtual void OnItemClick(int position)
        {
            var selectedObject = Adapter.GetRawItem(position);
            SelectedObject = selectedObject;
        }

        protected virtual void OnItemSelected(int position)
        {
            var selectedObject = Adapter.GetRawItem(position);
            SelectedObject = selectedObject;
        }

        public new MvxFilteringAdapter Adapter
        {
            get
            {
                return base.Adapter as MvxFilteringAdapter;
            }
            set
            {
                var existing = Adapter;
                if (existing == value)
                    return;

                if (existing != null)
                    existing.PartialTextChanged -= AdapterOnPartialTextChanged;

                if (existing != null && value != null)
                {
                    value.ItemsSource = existing.ItemsSource;
                    value.ItemTemplateId = existing.ItemTemplateId;
                }

                if (value != null)
                    value.PartialTextChanged += AdapterOnPartialTextChanged;

                base.Adapter = value;
            }
        }

        private void AdapterOnPartialTextChanged(object sender, EventArgs eventArgs)
        {
            FireChanged(PartialTextChanged);
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

        public string PartialText => Adapter.PartialText;

        private object _selectedObject;

        public object SelectedObject
        {
            get
            {
                return _selectedObject;
            }
            private set
            {
                if (_selectedObject == value)
                    return;

                _selectedObject = value;
                FireChanged(SelectedObjectChanged);
            }
        }

        public event EventHandler SelectedObjectChanged;

        public event EventHandler PartialTextChanged;

        private void FireChanged(EventHandler eventHandler)
        {
            eventHandler?.Invoke(this, EventArgs.Empty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ItemClick -= OnItemClick;
                ItemSelected -= OnItemSelected;

                if (Adapter != null)
                {
                    Adapter.PartialTextChanged -= AdapterOnPartialTextChanged;
                }
            }
            base.Dispose(disposing);
        }
    }
}
