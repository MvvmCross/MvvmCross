// MvxListView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections;
using System.Windows.Input;
using Android.Content;
using Android.Util;
using Android.Widget;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.Binding.Attributes;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
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
            Adapter = adapter;
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

        private ICommand _itemClick;
        public new ICommand ItemClick
        {
            get { return _itemClick; }
            set { _itemClick = value; if (_itemClick != null) EnsureItemClickOverloaded(); }
        }

        private bool _itemClickOverloaded = false;
        private void EnsureItemClickOverloaded()
        {
            if (_itemClickOverloaded)
                return;

            _itemClickOverloaded = true;
            base.ItemClick += (sender, args) => ExecuteCommandOnItem(this.ItemClick, args.Position);
        }

        private ICommand _itemLongClick;
        public new ICommand ItemLongClick
        {
            get { return _itemLongClick; }
            set { _itemLongClick = value; if (_itemLongClick != null) EnsureItemLongClickOverloaded(); }
        }

        private bool _itemLongClickOverloaded = false;
        private void EnsureItemLongClickOverloaded()
        {
            if (_itemLongClickOverloaded)
                return;

            _itemLongClickOverloaded = true;
            base.ItemLongClick += (sender, args) => ExecuteCommandOnItem(this.ItemLongClick, args.Position);
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
    }
}