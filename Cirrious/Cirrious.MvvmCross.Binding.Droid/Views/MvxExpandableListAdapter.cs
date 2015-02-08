using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.ExtensionMethods;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxExpandableListAdapter : MvxAdapter, IExpandableListAdapter
    {
        private IList _itemsSource;

        public MvxExpandableListAdapter(Context context)
            : base(context) { }

        private int _groupTemplateId;

        public int GroupTemplateId
        {
            get { return this._groupTemplateId; }
            set
            {
                if (this._groupTemplateId == value)
                    return;

                this._groupTemplateId = value;
                // since the template has changed then let's force the list to redisplay by firing NotifyDataSetChanged()
                if (ItemsSource != null)
                    NotifyDataSetChanged();
            }
        }

        protected override void SetItemsSource(IEnumerable value)
        {
            Mvx.Trace("Setting itemssource");

            if (this._itemsSource == value)
                return;

            var existingObservable = this._itemsSource as INotifyCollectionChanged;
            if (existingObservable != null)
                existingObservable.CollectionChanged -= OnItemsSourceCollectionChanged;

            this._itemsSource = value as IList;

            var newObservable = this._itemsSource as INotifyCollectionChanged;
            if (newObservable != null)
                newObservable.CollectionChanged += OnItemsSourceCollectionChanged;

            base.SetItemsSource(value);
        }

        public int GroupCount
        {
            get { return (this._itemsSource != null ? this._itemsSource.Count : 0); }
        }

        public void OnGroupExpanded(int groupPosition)
        {
            // do nothing
        }

        public void OnGroupCollapsed(int groupPosition)
        {
            // do nothing
        }

        public bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        public View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            var item = this._itemsSource[groupPosition];
            return base.GetBindableView(convertView, item, this.GroupTemplateId);
        }

        public long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public Java.Lang.Object GetGroup(int groupPosition)
        {
            return null;
        }

        // Base implementation returns a long (from BaseExpandableListAdapter.java):
        // bit 0: Whether this ID points to a child (unset) or group (set), so for this method
        //        this bit will be 1.
        // bit 1-31: Lower 31 bits of the groupId
        // bit 32-63: Lower 32 bits of the childId.
        public long GetCombinedGroupId(long groupId)
        {
            return (groupId & 0x7FFFFFFF) << 32;
        }

        // Base implementation returns a long:
        // bit 0: Whether this ID points to a child (unset) or group (set), so for this method
        //        this bit will be 0.
        // bit 1-31: Lower 31 bits of the groupId
        // bit 32-63: Lower 32 bits of the childId.
        public long GetCombinedChildId(long groupId, long childId)
        {
            return (long)(0x8000000000000000UL | (ulong)((groupId & 0x7FFFFFFF) << 32) | (ulong)(childId & 0xFFFFFFFF));
        }

        public object GetRawItem(int groupPosition, int position)
        {
            return (this.GetRawGroup(groupPosition) as IEnumerable).Cast<object>().ToList()[position];
        }

        public object GetRawGroup(int groupPosition)
        {
            return this._itemsSource[groupPosition];
        }

        public View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView,
            ViewGroup parent)
        {
            var sublist = (this.GetRawGroup(groupPosition) as IEnumerable).Cast<object>().ToList();
            var item = sublist[childPosition];

            return base.GetBindableView(convertView, item, ItemTemplateId);
        }

        public int GetChildrenCount(int groupPosition)
        {
            return (this.GetRawGroup(groupPosition) as IEnumerable).Cast<object>().ToList().Count();
        }

        public long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return null;
        }

        public Tuple<int, int> GetPositions(object childItem)
        {
            int groupPosition = 0;

            foreach (var item in this._itemsSource)
            {
                int childPosition = MvxEnumerableExtensions.GetPosition((IEnumerable)item, childItem);
                if (childPosition != -1)
                    return new Tuple<int, int>(groupPosition, childPosition);

                groupPosition++;
            }

            return null;
        }
    }
}