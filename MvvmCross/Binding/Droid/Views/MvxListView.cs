// MvxListView.cs
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

    [Register("cirrious.mvvmcross.binding.droid.views.MvxListView")]
    public class MvxListView
        : ListView
    {
        public MvxListView(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxAdapter(context))
        {
        }

        public MvxListView(Context context, IAttributeSet attrs, IMvxAdapter adapter)
            : base(context, attrs)
        {
            // Note: Any calling derived class passing a null adapter is responsible for setting
            // it's own itemTemplateId
            if (adapter == null)
                return;

            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;
            this.Adapter = adapter;
        }

        protected MvxListView(IntPtr javaReference, JniHandleOwnership transfer)
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

                if (value != null && existing != null)
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

        private ICommand _itemClick;

        public new ICommand ItemClick
        {
            get { return this._itemClick; }
            set { this._itemClick = value; if (this._itemClick != null) this.EnsureItemClickOverloaded(); }
        }

        private bool _itemClickOverloaded = false;

        private void EnsureItemClickOverloaded()
        {
            if (this._itemClickOverloaded)
                return;

            this._itemClickOverloaded = true;
            base.ItemClick += (sender, args) => this.ExecuteCommandOnItem(this.ItemClick, args.Position);
        }

        private ICommand _itemLongClick;

        public new ICommand ItemLongClick
        {
            get { return this._itemLongClick; }
            set { this._itemLongClick = value; if (this._itemLongClick != null) this.EnsureItemLongClickOverloaded(); }
        }

        private bool _itemLongClickOverloaded = false;

        private void EnsureItemLongClickOverloaded()
        {
            if (this._itemLongClickOverloaded)
                return;

            this._itemLongClickOverloaded = true;
            base.ItemLongClick += (sender, args) => this.ExecuteCommandOnItem(this.ItemLongClick, args.Position);
        }

        protected virtual void ExecuteCommandOnItem(ICommand command, int position)
        {
            if (command == null)
                return;

            var item = this.Adapter.GetRawItem(position);
            if (item == null)
                return;

            if (!command.CanExecute(item))
                return;

            command.Execute(item);
        }
    }
}