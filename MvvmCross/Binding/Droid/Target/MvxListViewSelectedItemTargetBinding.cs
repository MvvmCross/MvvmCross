// MvxListViewSelectedItemTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Target
{
    using System;

    using Android.Widget;

    using MvvmCross.Binding.Droid.Views;
    using MvvmCross.Platform.Platform;

#warning Can this be expanded to GridView too? Or to others?

    public class MvxListViewSelectedItemTargetBinding
        : MvxAndroidTargetBinding
    {
        protected MvxListView ListView => (MvxListView)Target;

        private object _currentValue;
        private bool _subscribed;

        public MvxListViewSelectedItemTargetBinding(MvxListView view)
            : base(view)
        {
        }

        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
            var listView = this.ListView;
            if (listView == null)
                return;

            var newValue = listView.Adapter.GetRawItem(itemClickEventArgs.Position);

            if (!newValue.Equals(this._currentValue))
            {
                this._currentValue = newValue;
                FireValueChanged(newValue);
            }
        }

        protected override void SetValueImpl(object target, object value)
        {
            if (value == null || value == this._currentValue)
                return;

            var listView = (MvxListView)target;

            var index = listView.Adapter.GetPosition(value);
            if (index < 0)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value not found for spinner {0}", value.ToString());
                return;
            }
            this._currentValue = value;
            listView.SetSelection(index);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var listView = ((ListView)this.ListView);
            if (listView == null)
                return;

            listView.ItemClick += this.OnItemClick;
            this._subscribed = true;
        }

        public override Type TargetType => typeof(object);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var listView = (ListView)this.ListView;
                if (listView != null && this._subscribed)
                {
                    listView.ItemClick -= this.OnItemClick;
                    this._subscribed = false;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}