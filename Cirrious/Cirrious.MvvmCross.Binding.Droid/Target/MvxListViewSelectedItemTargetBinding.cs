// MvxListViewSelectedItemTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxListViewSelectedItemTargetBinding : MvxAndroidTargetBinding
    {
        protected MvxListView ListView
        {
            get { return (MvxListView) Target; }
        }

        private object _currentValue;

        public MvxListViewSelectedItemTargetBinding(MvxListView view)
            : base(view)
        {
            // note that we use ItemClick here because the Selected event simply does not fire on the Android ListView
            ((ListView) ListView).ItemClick += OnItemClick;
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

        public override void SetValue(object value)
        {
            if (value != null && value != _currentValue)
            {
                var listView = ListView;
                if (listView == null)
                    return;

                var index = listView.Adapter.GetPosition(value);
                if (index < 0)
                {
                    MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value not found for spinner {0}", value.ToString());
                    return;
                }
                _currentValue = value;
                listView.SetSelection(index);
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        public override Type TargetType
        {
            get { return typeof (object); }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var listView = ListView;
                if (listView != null)
                {
                    ((ListView) ListView).ItemClick -= OnItemClick;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}