// MvxListView.cs

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

    [Register("mvvmcross.binding.droid.views.MvxListView")]
    public class MvxListView
        : ListView
    {
        private bool _itemClickOverloaded;
        private bool _itemLongClickOverloaded;

        private ICommand _itemClick;
        private ICommand _itemLongClick;

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

        protected MvxListView(IntPtr javaReference, JniHandleOwnership transfer)
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
            get { return _itemClick; }
            set
            {
                _itemClick = value;
                if (_itemClick != null)
                    EnsureItemClickOverloaded(); 
            }
        }

        private void EnsureItemClickOverloaded()
        {
            if (_itemClickOverloaded)
                return;

            _itemClickOverloaded = true;
            base.ItemClick += OnItemClick;
        }

        public new ICommand ItemLongClick
        {
            get { return _itemLongClick; }
            set
            {
                _itemLongClick = value;
                if (_itemLongClick != null)
                    EnsureItemLongClickOverloaded();
            }
        }

        private void EnsureItemLongClickOverloaded()
        {
            if (_itemLongClickOverloaded)
                return;

            _itemLongClickOverloaded = true;
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
