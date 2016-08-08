// MvxListViewSelectedItemTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Platform.Platform;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Binding.Droid.Target
{
#warning Can this be expanded to GridView too? Or to others?

    public class MvxListViewSelectedItemTargetBinding
        : MvxConvertingTargetBinding
    {
        protected MvxListView ListView => (MvxListView)Target;

        private object _currentValue;
        private IDisposable _subscription;

        public MvxListViewSelectedItemTargetBinding(MvxListView view)
            : base(view)
        {
        }

        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
            var listView = ListView;
            if (listView == null)
                return;

            var newValue = listView.Adapter.GetRawItem(itemClickEventArgs.Position);

            if (!newValue.Equals(_currentValue))
            {
                _currentValue = newValue;
                FireValueChanged(newValue);
            }
        }

        protected override void SetValueImpl(object target, object value)
        {
            if (value == null || value == _currentValue)
                return;

            var listView = (MvxListView)target;

            var index = listView.Adapter.GetPosition(value);
            if (index < 0)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value not found for spinner {0}", value.ToString());
                return;
            }
            _currentValue = value;
            listView.SetSelection(index);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var listView = ((ListView)ListView);
            if (listView == null)
                return;

            _subscription = listView.WeakSubscribe<ListView, AdapterView.ItemClickEventArgs>(nameof(listView.ItemClick), OnItemClick);
        }

        public override Type TargetType => typeof(object);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }
            base.Dispose(isDisposing);
        }
    }
}