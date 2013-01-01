// MvxListViewSelectedItemTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxListViewSelectedItemTargetBinding : MvxBaseAndroidTargetBinding
    {
        private readonly MvxBindableListView _view;
        private object _currentValue;

        public MvxListViewSelectedItemTargetBinding(MvxBindableListView view)
        {
            _view = view;
            // note that we use ItemClick here because the Selected event simply does not fire on the Android ListView
            ((ListView) _view).ItemClick += OnItemClick;
        }

        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
            var newValue = _view.Adapter.GetRawItem(itemClickEventArgs.Position);

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
                var index = _view.Adapter.GetPosition(value);
                if (index < 0)
                {
                    MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value not found for spinner {0}", value.ToString());
                    return;
                }
                _currentValue = value;
                _view.SetSelection(index);
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
                ((ListView) _view).ItemClick -= OnItemClick;
            }
            base.Dispose(isDisposing);
        }
    }
}