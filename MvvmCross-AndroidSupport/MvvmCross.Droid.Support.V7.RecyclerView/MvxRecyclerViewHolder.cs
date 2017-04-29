// MvxRecyclerViewHolder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView.Grouping;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemSources.Data;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace MvvmCross.Droid.Support.V7.RecyclerView
{
    [Register("mvvmcross.droid.support.v7.recyclerview.MvxRecyclerViewHolder")]
    public class MvxRecyclerViewHolder : Android.Support.V7.Widget.RecyclerView.ViewHolder, IMvxRecyclerViewHolder,
        IMvxBindingContextOwner
    {
        private readonly IMvxBindingContext _bindingContext;
        private readonly int _viewType;

        private object _cachedDataContext;
        private ICommand _click, _longClick;
        private bool _clickOverloaded, _longClickOverloaded;

        public MvxRecyclerViewHolder(View itemView, IMvxAndroidBindingContext context)
            : this(itemView, context, 0)
        {
        }

        public MvxRecyclerViewHolder(View itemView, IMvxAndroidBindingContext context, int viewType = 0)
            : base(itemView)
        {
            _viewType = viewType;
            _bindingContext = context;
        }

        public MvxRecyclerViewHolder(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        public ICommand Click
        {
            get => _click;
            set
            {
                _click = value;
                if (_click != null)
                    EnsureClickOverloaded();
            }
        }

        public ICommand LongClick
        {
            get => _longClick;
            set
            {
                _longClick = value;
                if (_longClick != null)
                    EnsureLongClickOverloaded();
            }
        }

        public ICommand HeaderClickCommand { get; set; }

        public ICommand FooterClickCommand { get; set; }

        public ICommand GroupHeaderClickCommand { get; set; }

        public IMvxBindingContext BindingContext
        {
            get => _bindingContext;
            set => throw new NotImplementedException("BindingContext is readonly in the list item");
        }

        public object DataContext
        {
            get => _bindingContext.DataContext;
            set
            {
                _bindingContext.DataContext = value;

                // This is just a precaution.  If we've set the DataContext to something
                // then we don't need to have the old one still cached.
                if (value != null)
                    _cachedDataContext = null;
            }
        }

        public virtual void OnAttachedToWindow()
        {
            if (_cachedDataContext != null && DataContext == null)
                DataContext = _cachedDataContext;
        }

        public virtual void OnDetachedFromWindow()
        {
            _cachedDataContext = DataContext;
            DataContext = null;
        }

        public virtual void OnViewRecycled()
        {
            _cachedDataContext = null;
            DataContext = null;
        }

        private void EnsureClickOverloaded()
        {
            if (_clickOverloaded)
                return;
            _clickOverloaded = true;
            ItemView.Click += OnItemViewOnClick;
        }

        private void EnsureLongClickOverloaded()
        {
            if (_longClickOverloaded)
                return;
            _longClickOverloaded = true;
            ItemView.LongClick += OnItemViewOnLongClick;
        }

        protected virtual void ExecuteCommandOnItem(ICommand command)
        {
            if (command == null)
                return;

            var item = DataContext;
            if (item == null)
                return;

            if (_viewType == MvxBaseTemplateSelector.HeaderViewTypeId)
            {
                if (item is MvxHeaderItemData)
                    if (HeaderClickCommand != null && HeaderClickCommand.CanExecute(null))
                        HeaderClickCommand.Execute(null);
                return;
            }

            if (_viewType == MvxBaseTemplateSelector.FooterViewTypeId)
            {
                if (item is MvxFooterItemData)
                    if (FooterClickCommand != null && FooterClickCommand.CanExecute(null))
                        FooterClickCommand.Execute(null);
                return;
            }

            if (item is MvxGroupedData)
            {
                var groupedData = item as MvxGroupedData;
                if (GroupHeaderClickCommand != null && GroupHeaderClickCommand.CanExecute(groupedData.Key))
                    GroupHeaderClickCommand.Execute(groupedData.Key);
                return;
            }

            if (!command.CanExecute(item))
                return;

            command.Execute(item);
        }

        private void OnItemViewOnClick(object sender, EventArgs args)
        {
            ExecuteCommandOnItem(Click);
        }

        private void OnItemViewOnLongClick(object sender, View.LongClickEventArgs args)
        {
            ExecuteCommandOnItem(LongClick);
        }

        protected override void Dispose(bool disposing)
        {
            // Clean up the binding context since nothing
            // explicitly Disposes of the ViewHolder.
            _bindingContext?.ClearAllBindings();

            if (disposing)
            {
                _cachedDataContext = null;

                if (ItemView != null)
                {
                    ItemView.Click -= OnItemViewOnClick;
                    ItemView.LongClick -= OnItemViewOnLongClick;
                }
            }

            base.Dispose(disposing);
        }
    }
}