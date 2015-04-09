// MvxAutoCompleteTextView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Attributes;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxAutoCompleteTextView
        : AutoCompleteTextView
    {
        public MvxAutoCompleteTextView(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxFilteringAdapter(context))
        {
            // note - we shouldn't realy need both of these... but we do
            this.ItemClick += OnItemClick;
            this.ItemSelected += OnItemSelected;
        }

        public MvxAutoCompleteTextView(Context context, IAttributeSet attrs,
                                       MvxFilteringAdapter adapter)
            : base(context, attrs)
        {
            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;
            Adapter = adapter;
            this.ItemClick += OnItemClick;
        }

		protected MvxAutoCompleteTextView(IntPtr javaReference, JniHandleOwnership transfer)
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
            get { return base.Adapter as MvxFilteringAdapter; }
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

        public string PartialText
        {
            get { return Adapter.PartialText; }
        }

        private object _selectedObject;

        public object SelectedObject
        {
            get { return _selectedObject; }
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
            var handler = eventHandler;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}