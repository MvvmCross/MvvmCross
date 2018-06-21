// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using MvvmCross.Binding.Attributes;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MvvmCross.Droid.Support.V7.AppCompat.Widget
{
    /// <summary>
    /// Tint-aware version of MvxSpinner styled properly with AppCompat V22.2+.
    /// TODO: We may want to figure out a way to delegate to a common class for both.
    /// </summary>
    [Register("mvvmcross.droid.support.v7.appcompat.widget.MvxAppCompatSpinner")]
    public class MvxAppCompatSpinner : AppCompatSpinner
    {
        public MvxAppCompatSpinner(Context context, IAttributeSet attrs)
            : this(context, attrs,
                new MvxAdapter(context)
                {
                    ItemTemplateId = global::Android.Resource.Layout.SimpleSpinnerItem,
                    DropDownItemTemplateId = global::Android.Resource.Layout.SimpleSpinnerDropDownItem
                })
        {
        }

        public MvxAppCompatSpinner(Context context, IAttributeSet attrs, IMvxAdapter adapter)
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

        protected MvxAppCompatSpinner(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public new IMvxAdapter Adapter
        {
            get => base.Adapter as IMvxAdapter;
            set
            {
                var existing = Adapter;
                if (existing == value)
                    return;

                if (existing != null && value != null)
                {
                    value.ItemTemplateId = existing.ItemTemplateId;
                    value.DropDownItemTemplateId = existing.DropDownItemTemplateId;
                    value.ItemsSource = existing.ItemsSource;
                }

                base.Adapter = value;
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

        public int DropDownItemTemplateId
        {
            get => Adapter.DropDownItemTemplateId;
            set => Adapter.DropDownItemTemplateId = value;
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
            base.Dispose(disposing);

            if (disposing)
            {
                ItemSelected -= OnItemSelected;
            }
        }
    }
}
