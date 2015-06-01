using System;
using System.Collections;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Cirrious.MvvmCross.Droid.Support.RecyclerView
{
    [Register("cirrious.mvvmcross.droid.support.recyclerview.MvxRecyclerView")]
    public class MvxRecyclerView : Android.Support.V7.Widget.RecyclerView
    {
        #region ctor

        protected MvxRecyclerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
        public MvxRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, 0, new MvxRecyclerAdapter()) { }
        public MvxRecyclerView(Context context, IAttributeSet attrs, int defStyle) : this(context, attrs, defStyle, new MvxRecyclerAdapter()) { }
        public MvxRecyclerView(Context context, IAttributeSet attrs, int defStyle, IMvxRecyclerAdapter adapter) : base(context, attrs, defStyle)
        {
            // Note: Any calling derived class passing a null adapter is responsible for setting
            // it's own itemTemplateId
            if (adapter == null)
                return;

            SetLayoutManager(new LinearLayoutManager(context));

            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;
            Adapter = adapter;
        }

        #endregion

        public sealed override void SetLayoutManager(LayoutManager layout)
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
                    value.ItemTemplateId = existing.ItemTemplateId;
                    value.ItemClick = existing.ItemClick;
                    value.ItemLongClick = existing.ItemLongClick;

                    SwapAdapter((Adapter)value, false);
                }
                else
                {
                    SetAdapter((Adapter)value);
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
            get { return Adapter.ItemTemplateId; }
            set { Adapter.ItemTemplateId = value; }
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
