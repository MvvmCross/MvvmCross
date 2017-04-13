// MvxAppCompatSpinner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Droid.Views;
using System;
using System.Collections;
using System.Windows.Input;

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
            : this(
                context, attrs,
                new MvxAdapter(context)
                {
                    SimpleViewLayoutId = global::Android.Resource.Layout.SimpleSpinnerItem,
                    DropDownItemTemplateId = Resource.Layout.support_simple_spinner_dropdown_item
                })
        { }

        public MvxAppCompatSpinner(Context context, IAttributeSet attrs, IMvxAdapter adapter)
            : base(context, attrs)
        {
            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            var dropDownItemTemplateId = MvxAttributeHelpers.ReadDropDownListItemTemplateId(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;
            adapter.DropDownItemTemplateId = dropDownItemTemplateId;
            Adapter = adapter;
            ItemSelected += OnItemSelected;
        }

        protected MvxAppCompatSpinner(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public new IMvxAdapter Adapter
        {
            get { return base.Adapter as IMvxAdapter; }
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
            if (this.HandleItemSelected == null
                || item == null
                || !this.HandleItemSelected.CanExecute(item))
                return;

            this.HandleItemSelected.Execute(item);
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