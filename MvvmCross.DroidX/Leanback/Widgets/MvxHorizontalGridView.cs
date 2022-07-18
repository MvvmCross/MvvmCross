// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.Leanback.Widget;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.Attributes;
using MvvmCross.DroidX.Leanback.Listeners;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MvvmCross.DroidX.Leanback.Widgets
{
    /// <remarks>
    /// This class is actually (almost) the same as MvxRecylerView. Please keep this in mind if fixing bugs or implementing improvements!
    /// </remarks>
    [Register("mvvmcross.droidx.leanback.widgets.MvxHorizontalGridView")]
    public class MvxHorizontalGridView
        : HorizontalGridView
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

            var typedArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.MvxHorizontalGridView);
            try
            {
                FocusFirstChildOnLaidOut = typedArray.GetBoolean(Resource.Styleable.MvxHorizontalGridView_FocusFirstChildOnLaidOut, false);
                if (FocusFirstChildOnLaidOut)
                {
                    SetOnChildLaidOutListener(new MvxFocusFirstChildOnChildLaidOutListener());
                }
            }
            finally
            {
                typedArray.Recycle();
            }

            // We need this listener to get information about the currently _selected_ item
            // Overriding setter of base.SelectedPosition is not enough!
            OnChildViewHolderSelectedListener = new MvxOnChildViewHolderSelectedListener();
            SetOnChildViewHolderSelectedListener(OnChildViewHolderSelectedListener);
        }

        #endregion ctor
        /// <summary>
        /// If true, the child at position 0 will request focus.
        /// </summary>
        public bool FocusFirstChildOnLaidOut { get; private set; }

        public new IMvxRecyclerAdapter Adapter
        {
            get
            {
                return GetAdapter() as IMvxRecyclerAdapter;
            }
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
            MvxAndroidLog.Instance.Log(LogLevel.Warning, "Overwriting OnChildViewHolderSelectedListener will possibly break ItemSelection command.");
            base.SetOnChildViewHolderSelectedListener(listener);
        }

        public new void SetOnChildLaidOutListener(IOnChildLaidOutListener listener)
        {
            if (FocusFirstChildOnLaidOut && !(listener is MvxFocusFirstChildOnChildLaidOutListener))
            {
                MvxAndroidLog.Instance.Log(LogLevel.Warning, "Overwriting OnChildLaidOutListener will possibly break focusing of first child!");
            }
            base.SetOnChildLaidOutListener(listener);
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
            get { return Adapter.ItemClick; }
            set { Adapter.ItemClick = value; }
        }

        public ICommand ItemLongClick
        {
            get { return Adapter.ItemLongClick; }
            set { Adapter.ItemLongClick = value; }
        }

        public ICommand ItemSelection
        {
            get { return OnChildViewHolderSelectedListener?.ItemSelection; }
            set { OnChildViewHolderSelectedListener.ItemSelection = value; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (OnChildViewHolderSelectedListener != null)
                {
                    OnChildViewHolderSelectedListener.Dispose();
                    OnChildViewHolderSelectedListener = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
