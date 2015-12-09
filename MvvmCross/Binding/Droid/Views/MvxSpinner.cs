// MvxSpinner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Views
{
    using System;
    using System.Collections;
    using System.Windows.Input;

    using Android.Content;
    using Android.Runtime;
    using Android.Util;
    using Android.Widget;

    [Register("cirrious.mvvmcross.binding.droid.views.MvxSpinner")]
    public class MvxSpinner : Spinner
    {
        public MvxSpinner(Context context, IAttributeSet attrs)
            : this(
                context, attrs,
                new MvxAdapter(context)
                {
                    SimpleViewLayoutId = global::Android.Resource.Layout.SimpleDropDownItem1Line
                })
        {
        }

        public MvxSpinner(Context context, IAttributeSet attrs, IMvxAdapter adapter)
            : base(context, attrs)
        {
            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            var dropDownItemTemplateId = MvxAttributeHelpers.ReadDropDownListItemTemplateId(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;
            adapter.DropDownItemTemplateId = dropDownItemTemplateId;
            this.Adapter = adapter;
            this.SetupHandleItemSelected();
        }

        protected MvxSpinner(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public new IMvxAdapter Adapter
        {
            get { return base.Adapter as IMvxAdapter; }
            set
            {
                var existing = this.Adapter;
                if (existing == value)
                    return;

                if (existing != null && value != null)
                {
                    value.ItemsSource = existing.ItemsSource;
                    value.ItemTemplateId = existing.ItemTemplateId;
                }

                base.Adapter = value;
            }
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

        public int DropDownItemTemplateId
        {
            get { return this.Adapter.DropDownItemTemplateId; }
            set { this.Adapter.DropDownItemTemplateId = value; }
        }

        public ICommand HandleItemSelected { get; set; }

        private void SetupHandleItemSelected()
        {
            base.ItemSelected += (sender, args) =>
                {
                    var position = args.Position;
                    this.HandleSelected(position);
                };
        }

        protected virtual void HandleSelected(int position)
        {
            var item = this.Adapter.GetRawItem(position);
            if (this.HandleItemSelected == null
                || item == null
                || !this.HandleItemSelected.CanExecute(item))
                return;

            this.HandleItemSelected.Execute(item);
        }
    }
}