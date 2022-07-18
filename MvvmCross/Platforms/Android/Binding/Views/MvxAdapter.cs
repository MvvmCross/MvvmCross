// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Extensions;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.WeakSubscription;
using Object = Java.Lang.Object;

namespace MvvmCross.Platforms.Android.Binding.Views
{
    public class MvxAdapter
        : BaseAdapter
        , IMvxAdapter
    {
        private static int[] SimpleItemTemplateIds { get; } =
        {
            global::Android.Resource.Layout.SimpleListItem1,
            global::Android.Resource.Layout.SimpleSpinnerItem
        };

        private int _itemTemplateId = global::Android.Resource.Layout.SimpleListItem1;
        private int _dropDownItemTemplateId = global::Android.Resource.Layout.SimpleSpinnerDropDownItem;
        private IEnumerable _itemsSource;
        private IDisposable _subscription;

        public MvxAdapter(Context context)
            : this(context, MvxAndroidBindingContextHelpers.Current())
        {
        }

        public MvxAdapter(Context context, IMvxAndroidBindingContext bindingContext)
        {
            Context = context;
            BindingContext = bindingContext;
            if (BindingContext == null)
            {
                throw new MvxException(
                    "bindingContext is null during MvxAdapter creation - " +
                    "Adapter's should only be created when a specific binding " +
                    "context has been placed on the stack");
            }
        }

        protected MvxAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected Context Context { get; }

        protected IMvxAndroidBindingContext BindingContext { get; }

        public bool ReloadOnAllItemsSourceSets { get; set; }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get => _itemsSource;
            set => SetItemsSource(value);
        }

        public virtual int ItemTemplateId
        {
            get => _itemTemplateId;
            set => SetItemTemplate(ref _itemTemplateId, value);
        }

        public virtual int DropDownItemTemplateId
        {
            get => _dropDownItemTemplateId;
            set => SetItemTemplate(ref _dropDownItemTemplateId, value);
        }

        private void SetItemTemplate(ref int templateId, int newTemplateId)
        {
            if (templateId == newTemplateId) return;

            templateId = newTemplateId;

            if (ItemsSource != null)
                NotifyDataSetChanged();
        }

        public override int Count => ItemsSource.Count();

        protected virtual void SetItemsSource(IEnumerable value)
        {
            if (ReferenceEquals(_itemsSource, value)
                && !ReloadOnAllItemsSourceSets)
            {
                return;
            }

            _subscription?.Dispose();
            _subscription = null;

            _itemsSource = value;

            if (_itemsSource != null && !(_itemsSource is IList))
            {
                MvxLogHost.GetLog<MvxAdapter>()?.Log(LogLevel.Warning,
                  "You are currently binding to IEnumerable - " +
                  "this can be inefficient, especially for large collections. " +
                  "Binding to IList is more efficient.");
            }

            if (_itemsSource is INotifyCollectionChanged newObservable)
                _subscription = newObservable?.WeakSubscribe(OnItemsSourceCollectionChanged);

            NotifyDataSetChanged();
        }

        protected virtual void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyDataSetChanged(e);
        }

        public virtual void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
        {
            RealNotifyDataSetChanged();
        }

        public override void NotifyDataSetChanged()
        {
            RealNotifyDataSetChanged();
        }

        protected virtual void RealNotifyDataSetChanged()
        {
            try
            {
                base.NotifyDataSetChanged();
            }
            catch (Exception exception)
            {
                MvxLogHost.GetLog<MvxAdapter>()?.Log(LogLevel.Warning, exception,
                    "Exception masked during Adapter RealNotifyDataSetChanged Are you trying to update your collection from a background task? See http://goo.gl/0nW0L6");
            }
        }

        public virtual int GetPosition(object item)
        {
            return ItemsSource.GetPosition(item);
        }

        public virtual object GetRawItem(int position)
        {
            return ItemsSource.ElementAt(position);
        }

        public override Object GetItem(int position)
        {
            // we return null to Java here
            // we do **not**: return new MvxJavaContainer<object>(GetRawItem(position));
            // - see @JonPryor's answer in http://stackoverflow.com/questions/13842864/why-does-the-gref-go-too-high-when-i-put-a-mvxbindablespinner-in-a-mvxbindableli/13995199#comment19319057_13995199
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
            => GetView(position, convertView, parent, DropDownItemTemplateId);

        public override View GetView(int position, View convertView, ViewGroup parent)
            => GetView(position, convertView, parent, ItemTemplateId);

        protected virtual View GetView(int position, View convertView, ViewGroup parent, int templateId)
        {
            if (ItemsSource == null)
            {
                MvxBindingLog.Error("GetView called when ItemsSource is null");
                return null;
            }

            var source = GetRawItem(position);

            return GetBindableView(convertView, source, parent, templateId);
        }

        protected virtual View GetBindableView(
            View convertView, object dataContext, ViewGroup parent, int templateId)
        {
            IMvxListItemView viewToUse = null;

            // we have a templateid lets use bind and inflate on it :)
            if (convertView?.Tag is IMvxListItemView item &&
                item.TemplateId == templateId)
            {
                viewToUse = item;
            }
            else
            {
                viewToUse = CreateBindableView(dataContext, parent, templateId);
                viewToUse.Content.Tag = viewToUse as Object;
            }

            BindBindableView(dataContext, viewToUse);

            return viewToUse.Content;// as View;
        }

        protected virtual void BindBindableView(
            object source, IMvxListItemView viewToUse)
            => viewToUse.DataContext = source;

        protected virtual IMvxListItemView CreateBindableView(
            object dataContext, ViewGroup parent, int templateId)
        {
            if (SimpleItemTemplateIds.Contains(templateId) ||
                global::Android.Resource.Layout.SimpleSpinnerDropDownItem == templateId)
            {
                return new MvxSimpleListItemView(Context, BindingContext.LayoutInflaterHolder,
                    dataContext, parent, templateId);
            }

            return new MvxListItemView(Context, BindingContext.LayoutInflaterHolder,
                dataContext, parent, templateId);
        }
    }

    public class MvxAdapter<TItem> : MvxAdapter where TItem : class
    {
        public MvxAdapter(Context context)
            : base(context, MvxAndroidBindingContextHelpers.Current())
        {
        }

        public MvxAdapter(Context context, IMvxAndroidBindingContext bindingContext)
            : base(context, bindingContext)
        {
        }

        public MvxAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        [MvxSetToNullAfterBinding]
        public new IEnumerable<TItem> ItemsSource
        {
            get => base.ItemsSource as IEnumerable<TItem>;
            set => base.ItemsSource = value;
        }
    }
}
