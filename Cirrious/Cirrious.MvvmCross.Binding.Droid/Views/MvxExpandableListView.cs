using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Attributes;
using System;
using System.Collections;
using System.Windows.Input;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    [Register("cirrious.mvvmcross.binding.droid.views.MvxExpandableListView")]
    public class MvxExpandableListView : ExpandableListView
    {
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
            get { return this.ThisAdapter.ItemsSource; }
            set { this.ThisAdapter.ItemsSource = value; }
        }

        public int ItemTemplateId
        {
            get { return this.ThisAdapter.ItemTemplateId; }
            set { this.ThisAdapter.ItemTemplateId = value; }
        }

        public int GroupTemplateId
        {
            get { return this.ThisAdapter.GroupTemplateId; }
            set { this.ThisAdapter.GroupTemplateId = value; }
        }

        private ICommand _itemClick;

        public new ICommand ItemClick
        {
            get { return this._itemClick; }
            set
            {
                this._itemClick = value;
                if (this._itemClick != null) this.EnsureItemClickOverloaded();
            }
        }

        private ICommand _itemLongClick;

        public new ICommand ItemLongClick
        {
            get { return this._itemLongClick; }
            set
            {
                this._itemLongClick = value;
                if (this._itemLongClick != null) this.EnsureItemLongClickOverloaded();
            }
        }

        private ICommand _groupClick;

        public new ICommand GroupClick
        {
            get { return _groupClick; }
            set
            {
                this._groupClick = value;
                if (this._groupClick != null) this.EnsureGroupClickOverloaded();
            }
        }

        private ICommand _groupLongClick;

        public new ICommand GroupLongClick
        {
            get { return _groupLongClick; }
            set
            {
                this._groupLongClick = value;
                if (this._groupLongClick != null) this.EnsureItemLongClickOverloaded();
            }
        }

        private bool _itemClickOverloaded = false;

        private void EnsureItemClickOverloaded()
        {
            if (this._itemClickOverloaded)
                return;

            this._itemClickOverloaded = true;
            base.ChildClick +=
                (sender, args) => this.ExecuteCommandOnItem(this.ItemClick, args.GroupPosition, args.ChildPosition);
        }

        private bool _groupClickOverloaded = false;

        private void EnsureGroupClickOverloaded()
        {
            if (this._groupClickOverloaded)
                return;

            this._groupClickOverloaded = true;
            base.GroupClick +=
                (sender, args) => this.ExecuteCommandOnGroup(this.GroupClick, args.GroupPosition);
        }

        private bool _itemLongClickOverloaded;

        private void EnsureItemLongClickOverloaded()
        {
            if (this._itemLongClickOverloaded)
                return;
            this._itemLongClickOverloaded = true;
            base.ItemLongClick += (sender, args) =>
            {
                var type = GetPackedPositionType(args.Id);
                long packedPos = ((ExpandableListView)args.Parent).GetExpandableListPosition(args.Position);
                int groupPosition = GetPackedPositionGroup(packedPos);
                int childPosition = GetPackedPositionChild(packedPos);

                if (type == PackedPositionType.Child)
                {
                    this.ExecuteCommandOnItem(this.ItemLongClick, groupPosition, childPosition);
                }
                else if (type == PackedPositionType.Group)
                {
                    this.ExecuteCommandOnGroup(this.GroupLongClick, groupPosition);
                }
            };
        }

        protected virtual void ExecuteCommandOnItem(ICommand command, int groupPosition, int position)
        {
            if (command == null)
                return;

            var item = this.ThisAdapter.GetRawItem(groupPosition, position);
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

            var item = this.ThisAdapter.GetRawGroup(groupPosition);
            if (item == null)
                return;

            if (!command.CanExecute(item))
                return;

            command.Execute(item);
        }
    }
}