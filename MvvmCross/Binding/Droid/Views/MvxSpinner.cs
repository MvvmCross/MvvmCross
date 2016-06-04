// MvxSpinner.cs

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

    using MvvmCross.Binding.Attributes;

    [Register("mvvmcross.binding.droid.views.MvxSpinner")]
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
            Adapter = adapter;
            ItemSelected += OnItemSelected;
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
                var existing = Adapter;
                if (existing == value)
                    return;

                if (existing != null && value != null)
                {
                    value.ItemsSource = existing.ItemsSource;
                    value.ItemTemplateId = existing.ItemTemplateId;
                }
                if (existing != null)
                {
                    existing.ItemsSource = null;
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
