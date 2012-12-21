#region Copyright
// <copyright file="MvxBindableListAdapter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections;
using System.Collections.Specialized;
using Android;
using Android.Content;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Android.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Java.Lang;

namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public class MvxBindableListAdapter 
        : BaseAdapter
    {
        private readonly IMvxBindingActivity _bindingActivity;
        private readonly Context _context;
        private int _itemTemplateId;
        private int _dropDownItemTemplateId;
        private IList _itemsSource;

        public MvxBindableListAdapter(Context context)
        {
            _context = context;
            _bindingActivity = context as IMvxBindingActivity;
            if (_bindingActivity == null)
                throw new MvxException("MvxBindableListView can only be used within a Context which supports IMvxBindingActivity");
            SimpleViewLayoutId = Resource.Layout.SimpleListItem1;
        }

        protected Context Context { get { return _context; } }
        protected IMvxBindingActivity BindingActivity { get { return _bindingActivity; } }

        public int SimpleViewLayoutId { get; set; }

        [MvxSetToNullAfterBinding]
        public IList ItemsSource
        {
            get { return _itemsSource; }
            set {
                SetItemsSource(value);
            }
        }

        public int ItemTemplateId
        {
            get { return _itemTemplateId; }
            set
            {
                if (_itemTemplateId == value)
                    return;
                _itemTemplateId = value;

                // since the template has changed then let's force the list to redisplay by firing NotifyDataSetChanged()
                if (_itemsSource != null)
                    NotifyDataSetChanged();
            }
        }

        public int DropDownItemTemplateId
        {
            get { return _dropDownItemTemplateId; }
            set
            {
                if (_dropDownItemTemplateId == value)
                    return;
                _dropDownItemTemplateId = value;

                // since the template has changed then let's force the list to redisplay by firing NotifyDataSetChanged()
                if (_itemsSource != null)
                    NotifyDataSetChanged();
            }
        }

        public override int Count
        {
            get
            {
                if (_itemsSource == null)
                    return 0;

                return _itemsSource.Count;
            }
        }

        protected virtual void SetItemsSource(IList value)
        {
            if (_itemsSource == value)
                return;
            var existingObservable = _itemsSource as INotifyCollectionChanged;
            if (existingObservable != null)
                existingObservable.CollectionChanged -= OnItemsSourceCollectionChanged;
            _itemsSource = value;
            var newObservable = _itemsSource as INotifyCollectionChanged;
            if (newObservable != null)
                newObservable.CollectionChanged += OnItemsSourceCollectionChanged;
            NotifyDataSetChanged();
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyDataSetChanged(e);
        }

        public virtual void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
        {
            base.NotifyDataSetChanged();
        }

        public int GetPosition(object item)
        {
            if (_itemsSource == null)
                return -1;
            return _itemsSource.IndexOf(item);
        }

        public override Object GetItem(int position)
        {
            // we return null to Java here
            // - see @JonPryor's answer in http://stackoverflow.com/questions/13842864/why-does-the-gref-go-too-high-when-i-put-a-mvxbindablespinner-in-a-mvxbindableli/13995199#comment19319057_13995199
            return null;

            //var item = GetRawItem(position);
            //if (item == null)
            //    return null;
            //var wrapped = new MvxJavaContainer<object>(item); 
            //wrapped.Dispose();
            //return wrapped;
        }

        public object GetRawItem(int position)
        {
            if (_itemsSource == null)
                return null;
            return _itemsSource[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            return GetView(position, convertView, parent, DropDownItemTemplateId);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return GetView(position, convertView, parent, ItemTemplateId);
        }

        private View GetView(int position, View convertView, ViewGroup parent, int templateId)
        {
            if (_itemsSource == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "GetView called when ItemsSource is null");
                return null;
            }

            if (position < 0 || position >= _itemsSource.Count)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "GetView called with invalid Position - zero indexed {0} out of {1}", position, _itemsSource.Count);
                return null;
            }

            var rawItem = GetRawItem(position);
            var toReturn = GetBindableView(convertView, rawItem, templateId);
            return toReturn;
        }

        protected virtual View GetSimpleView(View convertView, object source)
        {
            if (convertView == null)
            {
                convertView = CreateSimpleView(source);
            }
            else
            {
                BindSimpleView(convertView, source);
            }

            return convertView;
        }

        protected virtual void BindSimpleView(View convertView, object source)
        {
            var textView = convertView as TextView;
            if (textView != null)
            {
                textView.Text = (source ?? string.Empty).ToString();
            }
        }

        protected virtual View CreateSimpleView(object source)
        {
            var view = _bindingActivity.NonBindingInflate(SimpleViewLayoutId, null);
            BindSimpleView(view, source);
            return view;
        }

        protected virtual View GetBindableView(View convertView, object source)
        {
            return GetBindableView(convertView, source, ItemTemplateId);
        }

        protected virtual View GetBindableView(View convertView, object source, int templateId)
        {
            if (templateId == 0)
            {
                // no template seen - so use a standard string view from Android and use ToString()
                return GetSimpleView(convertView, source);
            }

            // we have a templateid so lets use bind and inflate on it :)
            var viewToUse = convertView as IMvxBindableListItemView;
            if (viewToUse != null)
            {
                if (viewToUse.TemplateId != templateId)
                {
                    viewToUse = null;
                }
            }

            if (viewToUse == null)
            {
                viewToUse = CreateBindableView(source, templateId);
            }
            else
            {
                BindBindableView(source, viewToUse);
            }

            return viewToUse as View;
        }

        protected virtual void BindBindableView(object source, IMvxBindableListItemView viewToUse)
        {
            viewToUse.BindTo(source);
        }

        protected virtual MvxBindableListItemView CreateBindableView(object source, int templateId)
        {
            return new MvxBindableListItemView(_context, _bindingActivity, templateId, source);
        }
    }
}