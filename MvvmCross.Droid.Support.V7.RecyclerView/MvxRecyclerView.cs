// MvxRecyclerView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V7.RecyclerView.AttributeHelpers;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace MvvmCross.Droid.Support.V7.RecyclerView
{
    [Register("mvvmcross.droid.support.v7.recyclerview.MvxRecyclerView")]
    public class MvxRecyclerView : Android.Support.V7.Widget.RecyclerView
    {
        #region ctor

        protected MvxRecyclerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
        public MvxRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, 0, new MvxRecyclerAdapter()) { }
        public MvxRecyclerView(Context context, IAttributeSet attrs, int defStyle) : this(context, attrs, defStyle, new MvxRecyclerAdapter()) { }
        public MvxRecyclerView(Context context, IAttributeSet attrs, int defStyle, IMvxRecyclerAdapter adapter) : base(context, attrs, defStyle)
        {
            // Note: Any calling derived class passing a null adapter is responsible for setting
            // it's own ItemTemplateSelector
            if (adapter == null)
                return;

            SetLayoutManager(new LinearLayoutManager(context));

            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            var itemTemplateSelector = MvxRecyclerViewAttributeExtensions.BuildItemTemplateSelector(context, attrs);

            adapter.ItemTemplateSelector = itemTemplateSelector;
            Adapter = adapter;

            if (itemTemplateSelector.GetType() == typeof (MvxDefaultTemplateSelector))
                ItemTemplateId = itemTemplateId;
        }

        #endregion

        public override sealed void SetLayoutManager(LayoutManager layout)
        {
            base.SetLayoutManager(layout);
        }

        public new IMvxRecyclerAdapter Adapter
        {
            get { return base.GetAdapter() as IMvxRecyclerAdapter; }
            set
            {
                var existing = Adapter;

                if (existing == value)
                    return;

                // Support lib doesn't seem to have anything similar to IListAdapter yet
                // hence cast to Adapter.

                if (value != null && existing != null)
                {
                    value.ItemsSource = existing.ItemsSource;
                    value.ItemTemplateSelector = existing.ItemTemplateSelector;
                    value.ItemClick = existing.ItemClick;
                    value.ItemLongClick = existing.ItemLongClick;

                    SwapAdapter((Adapter)value, false);
                }
                else
                {
                    SetAdapter((Adapter)value);
                }

                if (existing != null)
                {
                    existing.ItemsSource = null;
                }
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
            get
            {
                var singleItemDefaultTemplateSelector = ItemTemplateSelector as MvxDefaultTemplateSelector;

                if (singleItemDefaultTemplateSelector == null)
                    throw new InvalidOperationException(
                        $"If you wan't to use single item-template RecyclerView Adapter you can't change it's" +
                        $"{nameof(IMvxTemplateSelector)} to anything other than {nameof(MvxDefaultTemplateSelector)}");

                return singleItemDefaultTemplateSelector.ItemTemplateId;
            }
            set
            {
                var singleItemDefaultTemplateSelector = ItemTemplateSelector as MvxDefaultTemplateSelector;

                if (singleItemDefaultTemplateSelector == null)
                    throw new InvalidOperationException(
                        $"If you wan't to use single item-template RecyclerView Adapter you can't change it's" +
                        $"{nameof(IMvxTemplateSelector)} to anything other than {nameof(MvxDefaultTemplateSelector)}");

                singleItemDefaultTemplateSelector.ItemTemplateId = value;
                Adapter.ItemTemplateSelector = singleItemDefaultTemplateSelector;
            }
        }


        public IMvxTemplateSelector ItemTemplateSelector
        {
            get { return Adapter.ItemTemplateSelector; }
            set { Adapter.ItemTemplateSelector = value; }
        }

        public ICommand ItemClick
        {
            get { return this.Adapter.ItemClick; }
            set { this.Adapter.ItemClick = value; }
        }

        public ICommand ItemLongClick
        {
            get { return this.Adapter.ItemLongClick; }
            set { this.Adapter.ItemLongClick = value; }
        }
    }
}
