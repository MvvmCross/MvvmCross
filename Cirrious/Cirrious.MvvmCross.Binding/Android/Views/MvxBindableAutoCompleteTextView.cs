#region Copyright
// <copyright file="MvxBindableAutoCompleteTextView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections;
using Android.Content;
using Android.Util;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public class MvxBindableAutoCompleteTextView
        : AutoCompleteTextView
    {
        public MvxBindableAutoCompleteTextView(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxFilteringBindableListAdapter(context))
        {
            this.ItemClick += OnItemClick;
        }        

        public MvxBindableAutoCompleteTextView(Context context, IAttributeSet attrs, MvxFilteringBindableListAdapter adapter)
            : base(context, attrs)
        {
            var itemTemplateId = MvxBindableListViewHelpers.ReadAttributeValue(context, attrs, MvxAndroidBindingResource.Instance.BindableListViewStylableGroupId, MvxAndroidBindingResource.Instance.BindableListItemTemplateId);
            adapter.ItemTemplateId = itemTemplateId;
            Adapter = adapter;
            this.ItemClick += OnItemClick;
        }

        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
            MvxTrace.Trace("Item clicked {0}", itemClickEventArgs.Position);
            var selectedObject = Adapter.ItemsSource[itemClickEventArgs.Position];
            MvxTrace.Trace("Item is {0}", selectedObject);
            SelectedObject = selectedObject;
        }

        public new MvxFilteringBindableListAdapter Adapter
        {
            get { return base.Adapter as MvxFilteringBindableListAdapter; }
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
        public IList ItemsSource
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
            private set { _selectedObject = value; FireChanged(SelectedObjectChanged); }
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