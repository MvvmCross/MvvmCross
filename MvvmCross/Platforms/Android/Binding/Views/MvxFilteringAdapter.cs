// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Java.Lang;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using Object = Java.Lang.Object;

namespace MvvmCross.Platforms.Android.Binding.Views
{
    public class MvxFilteringAdapter
        : MvxAdapter, IFilterable
    {
        private object _syncLock = new object();

        private class MyFilter : Filter
        {
            private readonly MvxFilteringAdapter _owner;

            public MyFilter(MvxFilteringAdapter owner)
            {
                _owner = owner;
            }

            #region Overrides of Filter

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var stringConstraint = constraint == null ? string.Empty : constraint.ToString();

                var (count, items) = _owner.FilterValues(stringConstraint);

                if (count == -1)
                    return new FilterResults();

                return new FilterResults
                {
                    Count = count,
                    Values = new MvxReplaceableJavaContainer { Object = items }
                };
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                if (results != null && results.Count > 0)
                {
                    var items = results.Values as MvxReplaceableJavaContainer;
                    if (items != null)
                    {
                        lock(_owner._syncLock)
                        {
                            _owner.FilteredItemsSource = items.Object as IEnumerable;
                            _owner.NotifyDataSetChanged();
                        }
                    }
                }
            }

            #endregion Overrides of Filter
        }

        public Func<object, string, bool> DefaultFilterPredicate = (item, filterString) => item.ToString().ToLowerInvariant().Contains(filterString.ToLowerInvariant());
        public Func<object, string, bool> FilterPredicate { get; set; }

        protected virtual (int, IEnumerable) FilterValues(string constraint)
        {
            if (PartialText == constraint)
                return (-1, null);

            PartialText = constraint;
            var filteredItems = ItemsSource.Filter(item => FilterPredicate(item, constraint));
            return (filteredItems.Count(), filteredItems);
        }

        public override IEnumerable ItemsSource
        {
            get => base.ItemsSource;
            set
            {
                lock(_syncLock)
                {
                    FilteredItemsSource = value;
                }
                base.ItemsSource = value;
            }
        }

        private IEnumerable FilteredItemsSource { get; set; }

        private string _partialText;

        public event EventHandler PartialTextChanged;

        public string PartialText
        {
            get => _partialText;
            private set
            {
                _partialText = value;
                FireConstraintChanged();
            }
        }

        private void FireConstraintChanged()
        {
            var activity = Context as Activity;

            activity?.RunOnUiThread(() =>
            {
                PartialTextChanged?.Invoke(this, EventArgs.Empty);
            });
        }

        public MvxFilteringAdapter(Context context) : this(context, MvxAndroidBindingContextHelpers.Current())
        {
        }

        public MvxFilteringAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
            ReturnSingleObjectFromGetItem = true;
            FilterPredicate = DefaultFilterPredicate;
            Filter = new MyFilter(this);
        }
        
        protected MvxFilteringAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public bool ReturnSingleObjectFromGetItem { get; set; }

        private MvxReplaceableJavaContainer _javaContainer;

        public override Object GetItem(int position)
        {
            // for autocomplete views we need to return something other than null here
            // - see @JonPryor's answer in http://stackoverflow.com/questions/13842864/why-does-the-gref-go-too-high-when-i-put-a-mvxbindablespinner-in-a-mvxbindableli/13995199#comment19319057_13995199
            // - and see problem report in https://github.com/slodge/MvvmCross/issues/145
            // - obviously this solution is not good for general Java code!
            if (ReturnSingleObjectFromGetItem)
            {
                if (_javaContainer == null)
                    _javaContainer = new MvxReplaceableJavaContainer();
                _javaContainer.Object = GetRawItem(position);
                return _javaContainer;
            }

            return base.GetItem(position);
        }

        public override object GetRawItem(int position)
        {
            lock(_syncLock)
            {
                var element = FilteredItemsSource?.ElementAt(position);
                return element;
            }
        }

        public override int GetPosition(object item)
        {
            lock (_syncLock)
            {
                var pos = FilteredItemsSource?.GetPosition(item) ?? 0;
                return pos;
            }
        }

        public override int Count
        {
            get
            {
                lock(_syncLock)
                {
                    return FilteredItemsSource?.Count() ?? 0;
                }
            }
        }

        #region Implementation of IFilterable

        public Filter Filter { get; set; }

        #endregion Implementation of IFilterable
    }
}
