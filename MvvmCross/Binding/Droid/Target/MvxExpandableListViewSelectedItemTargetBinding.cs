namespace MvvmCross.Binding.Droid.Target
{
    using System;

    using Android.Widget;

    using MvvmCross.Binding.Droid.Views;
    using MvvmCross.Platform.Platform;

    // This isn't a "pure" target binder like MvxListViewSelectedItemTargetBinding.
    // It differs in two ways:
    //  1. It checks the selected item.
    //  2. SetValueImpl typically compares value with null and _currentValue, returing
    //     if null or equal respectively.  This class foregoes this so that if the bound value of
    //     SelectedItem is set to null we can "override" _currentValue.
    public class MvxExpandableListViewSelectedItemTargetBinding : MvxAndroidTargetBinding
    {
        private object _currentValue;
        private bool _subscribed;

        public MvxExpandableListViewSelectedItemTargetBinding(MvxExpandableListView target)
            : base(target)
        { }

        protected MvxExpandableListView ListView => (MvxExpandableListView)Target;

        public override Type TargetType => typeof(object);

        protected override void SetValueImpl(object target, object value)
        {
            //if (value == null || value == _currentValue)
            //    return;

            if (value == null)
            {
                this._currentValue = null;
                this.ListView.ClearChoices();
                return;
            }

            var listView = (MvxExpandableListView)target;
            var positions = ((MvxExpandableListAdapter)listView.ExpandableListAdapter).GetPositions(value);
            if (positions == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value not found for spinner {0}", value.ToString());
                return;
            }

            this._currentValue = value;
            listView.SetSelectedChild(positions.Item1, positions.Item2, true);

            var pos =
                listView.GetFlatListPosition(ExpandableListView.GetPackedPositionForChild(positions.Item1,
                    positions.Item2));
            listView.SetItemChecked(pos, true);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var listView = ((ExpandableListView)this.ListView);
            if (listView == null)
                return;

            listView.ChildClick += this.OnChildClick;
            this._subscribed = true;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var listView = (ExpandableListView)this.ListView;
                if (listView != null && this._subscribed)
                {
                    listView.ChildClick -= this.OnChildClick;
                    this._subscribed = false;
                }
            }
            base.Dispose(isDisposing);
        }

        private void OnChildClick(object sender, ExpandableListView.ChildClickEventArgs childClickEventArgs)
        {
            var listView = this.ListView;
            if (listView == null)
                return;

            var newValue =
                ((MvxExpandableListAdapter)listView.ExpandableListAdapter).GetRawItem(
                    childClickEventArgs.GroupPosition, childClickEventArgs.ChildPosition);

            if (!newValue.Equals(this._currentValue))
            {
                var pos = listView.GetFlatListPosition(
                    ExpandableListView.GetPackedPositionForChild(
                        childClickEventArgs.GroupPosition,
                        childClickEventArgs.ChildPosition));
                listView.SetItemChecked(pos, true);

                this._currentValue = newValue;
                FireValueChanged(newValue);
            }
        }
    }
}