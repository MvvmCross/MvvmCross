// MvxAppCompatListView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Support.V7.AppCompat.Widget
{
    using System;
    using System.Collections;
    using System.Windows.Input;

    using Android.Content;
    using Android.Runtime;
    using Android.Support.V7.Widget;
    using Android.Util;

    using MvvmCross.Binding.Attributes;
    using MvvmCross.Binding.Droid.Views;

    [Register("mvvmcross.droid.support.v7.appcompat.widget.MvxAppCompatListView")]
    public class MvxAppCompatListView
        : ListViewCompat
    {
        private bool _itemClickOverloaded;
        private bool _itemLongClickOverloaded;

        private ICommand _itemClick;
        private ICommand _itemLongClick;

        public MvxAppCompatListView(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxAdapter(context))
        {
        }

        public MvxAppCompatListView(Context context, IAttributeSet attrs, IMvxAdapter adapter)
            : base(context, attrs)
        {
            // Note: Any calling derived class passing a null adapter is responsible for setting
            // it's own itemTemplateId
            if (adapter == null)
                return;

            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;
            Adapter = adapter;
        }

        protected MvxAppCompatListView(IntPtr javaReference, JniHandleOwnership transfer)
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

                if (value != null && existing != null)
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

        public new ICommand ItemClick
        {
            get { return this._itemClick; }
            set
            {
                this._itemClick = value;
                if (this._itemClick != null)
                    this.EnsureItemClickOverloaded();
            }
        }

        private void EnsureItemClickOverloaded()
        {
            if (this._itemClickOverloaded)
                return;

            this._itemClickOverloaded = true;
            base.ItemClick += OnItemClick;
        }

        public new ICommand ItemLongClick
        {
            get { return this._itemLongClick; }
            set
            {
                this._itemLongClick = value;
                if (this._itemLongClick != null)
                    this.EnsureItemLongClickOverloaded();
            }
        }

        private void EnsureItemLongClickOverloaded()
        {
            if (this._itemLongClickOverloaded)
                return;

            this._itemLongClickOverloaded = true;
            base.ItemLongClick += OnItemLongClick;
        }

        protected virtual void ExecuteCommandOnItem(ICommand command, int position)
        {
            if (command == null)
                return;

            var item = Adapter.GetRawItem(position);
            if (item == null)
                return;

            if (!command.CanExecute(item))
                return;

            command.Execute(item);
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            ExecuteCommandOnItem(ItemClick, e.Position);
        }

        private void OnItemLongClick(object sender, ItemLongClickEventArgs e)
        {
            ExecuteCommandOnItem(ItemLongClick, e.Position);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.ItemLongClick -= OnItemLongClick;
                base.ItemClick -= OnItemClick;
            }

            base.Dispose(disposing);
        }
    }
}
