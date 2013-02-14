// MvxBindableListAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections;
using System.Collections.Specialized;
using Android;
using Android.Content;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.Droid.Converters;
using Cirrious.MvvmCross.Binding.Droid.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Java.Lang;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxBindableListAdapter
        : BaseAdapter
    {
        private readonly IMvxViewBindingManager _bindingManager;
        private readonly Context _context;
        private int _itemTemplateId;
        private int _dropDownItemTemplateId;
        private IEnumerable _itemsSource;

        public MvxBindableListAdapter(Context context)
        {
            _context = context;
            var bindingActivity = context as IMvxBindingActivity;
            if (bindingActivity == null)
                throw new MvxException(
                    "MvxBindableListView can only be used within a Context which supports IMvxBindingActivity");
            _bindingManager = bindingActivity.BindingManager;
            SimpleViewLayoutId = Resource.Layout.SimpleListItem1;
        }

        protected Context Context
        {
            get { return _context; }
        }

        protected IMvxViewBindingManager BindingManager
        {
            get { return _bindingManager; }
        }

        public int SimpleViewLayoutId { get; set; }

        [MvxSetToNullAfterBinding]
        public IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set { SetItemsSource(value); }
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
            get { return _itemsSource.Count(); }
        }

        protected virtual void SetItemsSource(IEnumerable value)
        {
            if (_itemsSource == value)
                return;
            var existingObservable = _itemsSource as INotifyCollectionChanged;
            if (existingObservable != null)
                existingObservable.CollectionChanged -= OnItemsSourceCollectionChanged;
            _itemsSource = value;
            if (_itemsSource != null && !(_itemsSource is IList))
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Binding to IEnumerable rather than IList - this can be inefficient, especially for large lists");
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
            return _itemsSource.GetPosition(item);
        }

        public System.Object GetRawItem(int position)
        {
            return _itemsSource.ElementAt(position);
        }

        public override Object GetItem(int position)
        {
            // we return null to Java here
            // - see @JonPryor's answer in http://stackoverflow.com/questions/13842864/why-does-the-gref-go-too-high-when-i-put-a-mvxbindablespinner-in-a-mvxbindableli/13995199#comment19319057_13995199
            return null;
            //return new MvxJavaContainer<object>(GetRawItem(position));
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

            // 15 Oct 2012 - this position check removed as it's inefficient, especially for IEnumerable based collections
            /*
            if (position < 0 || position >= Count)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "GetView called with invalid Position - zero indexed {0} out of {1}", position, Count);
                return null;
            }
            */

            var source = GetRawItem(position);

            return GetBindableView(convertView, source, templateId);
        }

        protected virtual View CreateSimpleView()
        {
            var inflater = LayoutInflater.FromContext (Context);
            var view = inflater.Inflate (SimpleViewLayoutId, null);
            
            // Create manual binding:
            var bindingDesc = new MvxBindingDescription(
                "Text",
                "",
                MvxStringValueConverter.Instance,
                null,
                string.Empty,
                MvxBindingMode.Default).ToEnumerable();
            view.SetBindingTag(new MvxViewBindingTag(bindingDesc));

            return view;
        }

        protected virtual View GetBindableView(View convertView, object source)
        {
            return GetBindableView(convertView, source, ItemTemplateId);
        }

        protected virtual View GetBindableView(View convertView, object source, int templateId)
        {
            if (convertView == null)
            {
                if (templateId == 0) {
                    convertView = CreateSimpleView();
                } else {
                    var inflater = LayoutInflater.FromContext (Context);
                    convertView = inflater.Inflate (templateId, null);
                }
            }

            BindView (convertView, source);

            return convertView;
        }

        protected virtual void BindView(View view, object source)
        {
            _bindingManager.BindView (view, source);
        }
    }
}