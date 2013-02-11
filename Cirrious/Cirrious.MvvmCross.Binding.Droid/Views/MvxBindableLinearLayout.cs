// MvxBindableLinearLayout.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Collections.Specialized;
using Android.Content;
using Android.Util;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxBindableLinearLayout
        : LinearLayout
    {
        private readonly IMvxViewBindingManager _bindingManager;

        public MvxBindableLinearLayout(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            var bindingActivity = context as IMvxBindingActivity;
            if (bindingActivity == null)
                throw new MvxException(
                    "MvxBindableListView can only be used within a Context which supports IMvxBindingActivity");
            _bindingManager = bindingActivity.BindingManager;

            var itemTemplateId = MvxBindableListViewHelpers.ReadAttributeValue(context, attrs,
                                                                               MvxAndroidBindingResource.Instance
                                                                                                        .BindableListViewStylableGroupId,
                                                                               MvxAndroidBindingResource.Instance
                                                                                                        .BindableListItemTemplateId);
            Adapter = new MvxBindableListAdapterWithChangedEvent(context);
            Adapter.ItemTemplateId = itemTemplateId;
            Adapter.DataSetChanged += AdapterOnDataSetChanged;
            this.ChildViewRemoved += OnChildViewRemoved;
        }

        private void OnChildViewRemoved(object sender, ChildViewRemovedEventArgs childViewRemovedEventArgs)
        {
            _bindingManager.UnbindView (childViewRemovedEventArgs.Child);
        }

        private MvxBindableListAdapterWithChangedEvent _adapter;

        public MvxBindableListAdapterWithChangedEvent Adapter
        {
            get { return _adapter; }
            set
            {
                var existing = _adapter;
                if (existing == value)
                    return;

                if (existing != null && value != null)
                {
                    existing.DataSetChanged -= AdapterOnDataSetChanged;
                    value.ItemsSource = existing.ItemsSource;
                    value.ItemTemplateId = existing.ItemTemplateId;
                }

                if (value != null)
                {
                    value.DataSetChanged += AdapterOnDataSetChanged;
                }

                if (value == null)
                {
                    MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                          "Setting Adapter to null is not recommended - you amy lose ItemsSource binding when doing this");
                }

                _adapter = value;
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

        private void AdapterOnDataSetChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            switch (eventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this.Add(Adapter, eventArgs.NewStartingIndex, eventArgs.NewItems.Count);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    this.Remove(Adapter, eventArgs.OldStartingIndex, eventArgs.OldItems.Count);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (eventArgs.NewItems.Count != eventArgs.OldItems.Count)
                    {
                        this.Refill(Adapter);
                    }
                    else
                    {
                        this.Replace(Adapter, eventArgs.NewStartingIndex, eventArgs.NewItems.Count);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    // move is not implemented - so we call Refill instead 
                    this.Refill(Adapter);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.Refill(Adapter);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Refill(IAdapter adapter)
        {
            RemoveAllViews();
            var count = adapter.Count;
            for (var i = 0; i < count; i++)
            {
                AddView(adapter.GetView(i, null, this));
            }
        }

        public void Add(IAdapter adapter, int insertionIndex, int count)
        {
            for (var i = 0; i < count; i++)
            {
                AddView(adapter.GetView(insertionIndex + i, null, this), insertionIndex + i);
            }
        }

        public void Remove(IAdapter adapter, int removalIndex, int count)
        {
            for (var i = 0; i < count; i++)
            {
                RemoveViewAt(removalIndex + i);
            }
        }

        public void Replace(IAdapter adapter, int startIndex, int count)
        {
            for (var i = 0; i < count; i++)
            {
                RemoveViewAt(startIndex + i);
                AddView(adapter.GetView(startIndex + i, null, this), startIndex + i);
            }
        }
    }
}