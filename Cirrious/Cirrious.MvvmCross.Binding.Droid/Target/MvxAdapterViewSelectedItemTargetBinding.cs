using System;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
#warning THis needs to be redone for all adapterviews not just list view!
    public class MvxAdapterViewSelectedItemTargetBinding : MvxBaseAndroidTargetBinding
    {
        private readonly MvxBindableListView _view;
        private object _currentValue;

        public MvxAdapterViewSelectedItemTargetBinding(MvxBindableListView view)
        {
            _view = view;
            _view.ItemSelected += _spinner_ItemSelected;
#warning Hack Hack Hack why does ItemSelected not fire? :/
            ((ListView)_view).ItemClick += OnItemClick;
        }

        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
#warning More HACK HACK HACK
            //var container = (_view.SelectedItem as MvxJavaContainer);
            var container = (_view.GetItemAtPosition(itemClickEventArgs.Position) as MvxJavaContainer);
            if (container == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Missing MvxJavaContainer in MvxAdapterViewSelectedItemTargetBinding");
                return;
            }
            var newValue = container.Object;
            if (!newValue.Equals(_currentValue))
            {
                _currentValue = newValue;
                FireValueChanged(newValue);
            }
        }

        void _spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            MvxTrace.Trace("Haha - selected");
        }

        public override void SetValue(object value)
        {
#warning Sort out Equals test here
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
            get { return typeof(object); }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _view.ItemSelected -= _spinner_ItemSelected;
                ((ListView)_view).ItemClick -= OnItemClick;
            }
            base.Dispose(isDisposing);
        }
    }
}