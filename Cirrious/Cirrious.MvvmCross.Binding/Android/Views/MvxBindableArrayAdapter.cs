using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Runtime;
using Cirrious.MvvmCross.Binding.Android.Interfaces.Views;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Java.Lang;

namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public class MvxBindableArrayAdapter<T>
        : BaseAdapter, IFilterable
    {
        private readonly IMvxBindingActivity _bindingActivity;
        private readonly Context _context;
        private Filter _filter;
        private int _itemTemplateId;
        private IList<T> _itemsSource;
        private IList<T> _itemsOriginal;

        public MvxBindableArrayAdapter(Context context)
        {
            _context = context;
            _bindingActivity = context as IMvxBindingActivity;
            if (_bindingActivity == null)
                throw new MvxException("MvxBindableAutoComleteTextView can only be used within a Context which supports IMvxBindingActivity");
        }

        protected Context Context { get { return _context; } }
        protected IMvxBindingActivity BindingActivity { get { return _bindingActivity; } }

        public IList<T> ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                SetItemsSource(value);
                //if(ItemsOriginal==null)
                    SetItemsCopy(value);
            }
        }

        public IList<T> ItemsOriginal
        {
            get { return _itemsOriginal; }
            set
            {
                SetItemsCopy(value);
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

        public override int Count
        {
            get
            {
                if (_itemsSource == null)
                    return 0;

                return _itemsSource.Count;
            }
        }

        protected virtual void SetItemsSource(IList<T> value)
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

        protected virtual void SetItemsCopy(IList<T> value)
        {
            foreach (var item in value)
            {
                _itemsOriginal.Add(item);
            }
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyDataSetChanged();
        }

        public override Object GetItem(int position)
        {
            if (_itemsSource == null)
                return null;
            return new MvxJavaContainer<object>(_itemsSource[position]);
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
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

            var source = _itemsSource[position];

            return GetBindableView(convertView, source);
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
            var view = _bindingActivity.NonBindingInflate(Resource.Layout.SimpleListItem1, null);
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

        public override void NotifyDataSetChanged()
        {
            base.NotifyDataSetChanged();
        } 

        public Filter Filter
        {
            get
            {
                if (_filter == null)
                {
                    _filter = new MvxBindableArrayAdapterFilter<T>(this);
                    return _filter;
                }
                else
                    return _filter;
            }
        }

        public void Clear()
        {
            _itemsSource.Clear();
        }

        public void Add(T o)
        {

            _itemsSource.Add(o);
            
        }


    }

    public class MvxBindableArrayAdapterFilter<T> : Filter
    {
        MvxBindableArrayAdapter<T> adapter;

        public MvxBindableArrayAdapterFilter(MvxBindableArrayAdapter<T> _adapter)
        {
            adapter = _adapter;
        }

        protected override void PublishResults(Java.Lang.ICharSequence constraint, Filter.FilterResults results)
        {
            var getThem = results.Values as JavaList<T>;

            // Clone items so they are local.
            List<T> localItems = new List<T>();
            foreach (var item in getThem)
            {
                localItems.Add(item);
            }

            adapter.NotifyDataSetChanged();
            adapter.Clear();
            foreach (var item in localItems)
            {
                adapter.Add(item);
            }
        }

        protected override FilterResults PerformFiltering(Java.Lang.ICharSequence prefix)
        {
            Converters.MvxLanguageBinderConverter converter = new Converters.MvxLanguageBinderConverter();
            IList<T> filtered;
            if (prefix != null)
            {
                List<T> result = new List<T>();
                foreach (var item in adapter.ItemsSource)
                {
                    
                    string searchable = item.ToString();
                    if (searchable.ToLowerInvariant().StartsWith(prefix.ToString().ToLowerInvariant()))
                        result.Add(item);
                    else
                    {
                        string value = searchable.ToLowerInvariant();
                        string[] valueSplit = value.Split(' ');
                        foreach (var word in valueSplit)
                        {
                            if (word.StartsWith(prefix.ToString().ToLowerInvariant()))
                                result.Add(item);
                        }
                    }
                }
                filtered = result;
            }
            else
            {
                filtered = adapter.ItemsSource;
            }
            JavaList list = new JavaList();
            foreach (var item in filtered)
            {
                list.Add((Java.Lang.Object)item);
            }
            FilterResults results = new FilterResults();
            results.Values = list;
            results.Count = filtered.Count;
            return results;
        }
    }
}