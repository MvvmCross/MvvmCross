using System;
using Android.Widget;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Platform.Platform;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Binding.Droid.Target
{
    // This isn't a "pure" target binder like MvxListViewSelectedItemTargetBinding.
    // It differs in two ways:
    //  1. It checks the selected item.
    //  2. SetValueImpl typically compares value with null and _currentValue, returing
    //     if null or equal respectively.  This class foregoes this so that if the bound value of
    //     SelectedItem is set to null we can "override" _currentValue.
    public class MvxExpandableListViewSelectedItemTargetBinding 
        : MvxConvertingTargetBinding
    {
        private object _currentValue;
        private IDisposable _subscription;

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
                _currentValue = null;
                ListView.ClearChoices();
                return;
            }

            var listView = (MvxExpandableListView)target;
            var positions = ((MvxExpandableListAdapter)listView.ExpandableListAdapter).GetPositions(value);
            if (positions == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value not found for spinner {0}", value.ToString());
                return;
            }

            _currentValue = value;
            listView.SetSelectedChild(positions.Item1, positions.Item2, true);

            var pos =
                listView.GetFlatListPosition(ExpandableListView.GetPackedPositionForChild(positions.Item1,
                    positions.Item2));
            listView.SetItemChecked(pos, true);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var listView = ((ExpandableListView)ListView);
            if (listView == null)
                return;

            _subscription = listView.WeakSubscribe<ExpandableListView, ExpandableListView.ChildClickEventArgs>(
                nameof(listView.ChildClick),
                OnChildClick);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
            }
            base.Dispose(isDisposing);
        }

        private void OnChildClick(object sender, ExpandableListView.ChildClickEventArgs childClickEventArgs)
        {
            var listView = ListView;
            if (listView == null)
                return;

            var newValue =
                ((MvxExpandableListAdapter)listView.ExpandableListAdapter).GetRawItem(
                    childClickEventArgs.GroupPosition, childClickEventArgs.ChildPosition);

            if (!newValue.Equals(_currentValue))
            {
                var pos = listView.GetFlatListPosition(
                    ExpandableListView.GetPackedPositionForChild(
                        childClickEventArgs.GroupPosition,
                        childClickEventArgs.ChildPosition));
                listView.SetItemChecked(pos, true);

                _currentValue = newValue;
                FireValueChanged(newValue);
            }
        }
    }
}