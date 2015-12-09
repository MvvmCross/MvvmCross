// MvxAutoCompleteTextView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Views
{
    using System;
    using System.Collections;

    using Android.Content;
    using Android.Runtime;
    using Android.Util;
    using Android.Widget;

    [Register("cirrious.mvvmcross.binding.droid.views.MvxAutoCompleteTextView")]
    public class MvxAutoCompleteTextView
        : AutoCompleteTextView
    {
        public MvxAutoCompleteTextView(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxFilteringAdapter(context))
        {
            // note - we shouldn't realy need both of these... but we do
            this.ItemClick += this.OnItemClick;
            this.ItemSelected += this.OnItemSelected;
        }

        public MvxAutoCompleteTextView(Context context, IAttributeSet attrs,
                                       MvxFilteringAdapter adapter)
            : base(context, attrs)
        {
            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;
            this.Adapter = adapter;
            this.ItemClick += this.OnItemClick;
        }

        protected MvxAutoCompleteTextView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
            this.OnItemClick(itemClickEventArgs.Position);
        }

        private void OnItemSelected(object sender, AdapterView.ItemSelectedEventArgs itemSelectedEventArgs)
        {
            this.OnItemSelected(itemSelectedEventArgs.Position);
        }

        protected virtual void OnItemClick(int position)
        {
            var selectedObject = this.Adapter.GetRawItem(position);
            this.SelectedObject = selectedObject;
        }

        protected virtual void OnItemSelected(int position)
        {
            var selectedObject = this.Adapter.GetRawItem(position);
            this.SelectedObject = selectedObject;
        }

        public new MvxFilteringAdapter Adapter
        {
            get { return base.Adapter as MvxFilteringAdapter; }
            set
            {
                var existing = this.Adapter;
                if (existing == value)
                    return;

                if (existing != null)
                    existing.PartialTextChanged -= this.AdapterOnPartialTextChanged;

                if (existing != null && value != null)
                {
                    value.ItemsSource = existing.ItemsSource;
                    value.ItemTemplateId = existing.ItemTemplateId;
                }

                if (value != null)
                    value.PartialTextChanged += this.AdapterOnPartialTextChanged;

                base.Adapter = value;
            }
        }

        private void AdapterOnPartialTextChanged(object sender, EventArgs eventArgs)
        {
            this.FireChanged(this.PartialTextChanged);
        }

        [MvxSetToNullAfterBinding]
        public IEnumerable ItemsSource
        {
            get { return this.Adapter.ItemsSource; }
            set { this.Adapter.ItemsSource = value; }
        }

        public int ItemTemplateId
        {
            get { return this.Adapter.ItemTemplateId; }
            set { this.Adapter.ItemTemplateId = value; }
        }

        public string PartialText => this.Adapter.PartialText;

        private object _selectedObject;

        public object SelectedObject
        {
            get { return this._selectedObject; }
            private set
            {
                if (this._selectedObject == value)
                    return;

                this._selectedObject = value;
                this.FireChanged(this.SelectedObjectChanged);
            }
        }

        public event EventHandler SelectedObjectChanged;

        public event EventHandler PartialTextChanged;

        private void FireChanged(EventHandler eventHandler)
        {
            var handler = eventHandler;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}