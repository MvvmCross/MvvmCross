// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using MvvmCross.Binding.Attributes;

namespace MvvmCross.Platforms.Android.Binding.Views
{
    [Register("mvvmcross.platforms.android.binding.views.MvxSpinner")]
    public class MvxSpinner : Spinner
    {
        public MvxSpinner(Context context, IAttributeSet attrs)
            : this(
                context, attrs,
                new MvxAdapter(context)
                {
                    ItemTemplateId = global::Android.Resource.Layout.SimpleSpinnerItem,
                    DropDownItemTemplateId = global::Android.Resource.Layout.SimpleSpinnerDropDownItem
                })
        {
        }

        public MvxSpinner(Context context, IAttributeSet attrs, IMvxAdapter adapter)
            : base(context, attrs)
        {
            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            var dropDownItemTemplateId = MvxAttributeHelpers.ReadDropDownListItemTemplateId(context, attrs);

            if (itemTemplateId > 0)
                adapter.ItemTemplateId = itemTemplateId;
            if (dropDownItemTemplateId > 0)
                adapter.DropDownItemTemplateId = dropDownItemTemplateId;

            Adapter = adapter;
            ItemSelected += OnItemSelected;
        }

        protected MvxSpinner(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public new IMvxAdapter Adapter
        {
            get
            {
                return base.Adapter as IMvxAdapter;
            }
            set
            {
                var existing = Adapter;
                if (existing == value)
                    return;

                if (existing != null && value != null)
                {
                    value.ItemsSource = existing.ItemsSource;
                    value.ItemTemplateId = existing.ItemTemplateId;
                    value.DropDownItemTemplateId = existing.DropDownItemTemplateId;
                }

                base.Adapter = value;

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

        public int DropDownItemTemplateId
        {
            get { return Adapter.DropDownItemTemplateId; }
            set { Adapter.DropDownItemTemplateId = value; }
        }

        public ICommand HandleItemSelected { get; set; }

        private void OnItemSelected(object sender, ItemSelectedEventArgs e)
        {
            var position = e.Position;
            HandleSelected(position);
        }

        protected virtual void HandleSelected(int position)
        {
            var item = Adapter.GetRawItem(position);
            if (HandleItemSelected == null
                || item == null
                || !HandleItemSelected.CanExecute(item))
                return;

            HandleItemSelected.Execute(item);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ItemSelected -= OnItemSelected;
            }
            base.Dispose(disposing);
        }
    }
}
