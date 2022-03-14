// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxListViewSelectedItemTargetBinding
        : MvxAndroidTargetBinding
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
                MvxBindingLog.Warning("Value not found for spinner {0}", value.ToString());
                return;
            }
            _currentValue = value;
            listView.SetSelection(index);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var listView = (ListView)ListView;
            if (listView == null)
                return;

            _subscription = listView.WeakSubscribe<ListView, AdapterView.ItemClickEventArgs>(nameof(listView.ItemClick), OnItemClick);
        }

        public override Type TargetValueType => typeof(object);

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
