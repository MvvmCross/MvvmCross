namespace MvvmCross.Binding.Droid.Views
{
    using System;
    using System.Collections;
    using System.Windows.Input;

    using Android.Content;
    using Android.Runtime;
    using Android.Util;
    using Android.Widget;

    using MvvmCross.Binding.Attributes;

    [Register("mvvmcross.binding.droid.views.MvxExpandableListView")]
    public class MvxExpandableListView : ExpandableListView
    {
        private bool _groupClickOverloaded;
        private bool _itemClickOverloaded;
        private bool _itemLongClickOverloaded;

        private ICommand _itemClick;
        private ICommand _itemLongClick;
        private ICommand _groupClick;
        private ICommand _groupLongClick;

        public MvxExpandableListView(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxExpandableListAdapter(context))
        { }

        public MvxExpandableListView(Context context, IAttributeSet attrs, MvxExpandableListAdapter adapter)
            : base(context, attrs)
        {
            if (adapter == null)
                return;

            var groupTemplateId = MvxAttributeHelpers.ReadGroupItemTemplateId(context, attrs);
            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);

            SetAdapter(adapter);

            adapter.GroupTemplateId = groupTemplateId;
            adapter.ItemTemplateId = itemTemplateId;
        }

        protected MvxExpandableListView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        // An expandableListView has ExpandableListAdapter as propertyname, but Adapter still exists but is always null.
        protected MvxExpandableListAdapter ThisAdapter => ExpandableListAdapter as MvxExpandableListAdapter;

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get { return ThisAdapter.ItemsSource; }
            set { ThisAdapter.ItemsSource = value; }
        }

        public int ItemTemplateId
        {
            get { return ThisAdapter.ItemTemplateId; }
            set { ThisAdapter.ItemTemplateId = value; }
        }

        public int GroupTemplateId
        {
            get { return ThisAdapter.GroupTemplateId; }
            set { ThisAdapter.GroupTemplateId = value; }
        }

        public new ICommand ItemClick
        {
            get { return _itemClick; }
            set
            {
                _itemClick = value;
                if (_itemClick != null) EnsureItemClickOverloaded();
            }
        }

        public new ICommand ItemLongClick
        {
            get { return _itemLongClick; }
            set
            {
                _itemLongClick = value;
                if (_itemLongClick != null) EnsureItemLongClickOverloaded();
            }
        }

        public new ICommand GroupClick
        {
            get { return _groupClick; }
            set
            {
                _groupClick = value;
                if (_groupClick != null) EnsureGroupClickOverloaded();
            }
        }

        public ICommand GroupLongClick
        {
            get { return _groupLongClick; }
            set
            {
                _groupLongClick = value;
                if (_groupLongClick != null) EnsureItemLongClickOverloaded();
            }
        }

        private void EnsureItemClickOverloaded()
        {
            if (_itemClickOverloaded)
                return;

            _itemClickOverloaded = true;
            ChildClick += ChildOnClick;
        }

        private void ChildOnClick(object sender, ChildClickEventArgs e)
        {
            ExecuteCommandOnItem(ItemClick, e.GroupPosition, e.ChildPosition);
        }

        private void EnsureGroupClickOverloaded()
        {
            if (_groupClickOverloaded)
                return;

            _groupClickOverloaded = true;
            base.GroupClick += GroupOnClick;
        }

        private void GroupOnClick(object sender, GroupClickEventArgs e)
        {
           ExecuteCommandOnGroup(GroupClick, e.GroupPosition);
        }

        private void EnsureItemLongClickOverloaded()
        {
            if (_itemLongClickOverloaded)
                return;
            _itemLongClickOverloaded = true;
            base.ItemLongClick += ItemOnLongClick;
        }

        private void ItemOnLongClick(object sender, ItemLongClickEventArgs e)
        {
            var type = GetPackedPositionType(e.Id);
            long packedPos = ((ExpandableListView)e.Parent).GetExpandableListPosition(e.Position);
            int groupPosition = GetPackedPositionGroup(packedPos);
            int childPosition = GetPackedPositionChild(packedPos);

            if (type == PackedPositionType.Child)
            {
                ExecuteCommandOnItem(ItemLongClick, groupPosition, childPosition);
            }
            else if (type == PackedPositionType.Group)
            {
                ExecuteCommandOnGroup(GroupLongClick, groupPosition);
            }
        }

        protected virtual void ExecuteCommandOnItem(ICommand command, int groupPosition, int position)
        {
            if (command == null)
                return;

            var item = ThisAdapter.GetRawItem(groupPosition, position);
            if (item == null)
                return;

            if (!command.CanExecute(item))
                return;

            command.Execute(item);
        }

        protected virtual void ExecuteCommandOnGroup(ICommand command, int groupPosition)
        {
            if (command == null)
                return;

            var item = ThisAdapter.GetRawGroup(groupPosition);
            if (item == null)
                return;

            if (!command.CanExecute(item))
                return;

            command.Execute(item);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                base.GroupClick -= GroupOnClick;
                ChildClick -= ChildOnClick;
                base.ItemLongClick -= ItemOnLongClick;
            }
        }
    }
}