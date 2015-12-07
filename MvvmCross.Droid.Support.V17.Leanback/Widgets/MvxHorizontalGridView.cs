// MvxHorizontalGridView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Runtime;
using Android.Util;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V7.RecyclerView;
using System;
using System.Collections;
using System.Windows.Input;

namespace Cirrious.MvvmCross.Droid.Support.Leanback.Widgets
{
    /// <remarks>
    /// This class is actually (almost) the same as MvxReyclerView. Please keep this in mind if fixing bugs or implementing improvements!
    /// </remarks>
    [Register("cirrious.mvvmcross.droid.support.leanback.widgets.MvxHorizontalGridView")]
    public class MvxHorizontalGridView : Android.Support.V17.Leanback.Widget.HorizontalGridView
    {
        #region ctor

        protected MvxHorizontalGridView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public MvxHorizontalGridView(Context context, IAttributeSet attrs) : this(context, attrs, 0, new MvxRecyclerAdapter())
        {
        }

        public MvxHorizontalGridView(Context context, IAttributeSet attrs, int defStyle) : this(context, attrs, defStyle, new MvxRecyclerAdapter())
        {
        }

        public MvxHorizontalGridView(Context context, IAttributeSet attrs, int defStyle, IMvxRecyclerAdapter adapter) : base(context, attrs, defStyle)
        {
            // Note: Any calling derived class passing a null adapter is responsible for setting
            // it's own itemTemplateId
            if (adapter == null)
                return;

            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);

			adapter.ItemTemplateId = itemTemplateId;
            Adapter = adapter;

			// We need this listener to get information about the currently _selected_ item
	        OnChildViewHolderSelectedListener = new MvxOnChildViewHolderSelectedListener();
            SetOnChildViewHolderSelectedListener(OnChildViewHolderSelectedListener);
        }

        #endregion ctor

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

		protected MvxOnChildViewHolderSelectedListener OnChildViewHolderSelectedListener { get; set; }

		public new void SetOnChildViewHolderSelectedListener(OnChildViewHolderSelectedListener listener)
		{
			MvxTrace.Warning("Overwriting OnChildViewHolderSelectedListener will possibly break ItemSelectedPosition command.");
			base.SetOnChildViewHolderSelectedListener(listener);
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

		public ICommand ItemSelection
		{
			get { return OnChildViewHolderSelectedListener?.ItemSelection; }
			set { OnChildViewHolderSelectedListener.ItemSelection = value; }
		}
    }
}