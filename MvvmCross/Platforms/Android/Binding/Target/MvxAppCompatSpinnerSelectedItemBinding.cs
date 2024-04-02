// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxAppCompatSpinnerSelectedItemBinding
    : MvxAndroidTargetBinding
{
    private object? _currentValue;
    private MvxAndroidTargetEventSubscription<MvxAppCompatSpinner, AdapterView.ItemSelectedEventArgs>? _subscription;

    protected MvxAppCompatSpinner? Spinner => (MvxAppCompatSpinner?)Target;

    public MvxAppCompatSpinnerSelectedItemBinding(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
            MvxAppCompatSpinner spinner)
        : base(spinner)
    {
    }

    private void SpinnerItemSelected(object? sender, AdapterView.ItemSelectedEventArgs e)
    {
        var spinner = Spinner;
        if (spinner == null)
            return;

        var newValue = spinner.Adapter.GetRawItem(e.Position);

        bool changed;
        if (newValue == null)
        {
            changed = _currentValue != null;
        }
        else
        {
            changed = !newValue.Equals(_currentValue);
        }

        if (!changed)
        {
            return;
        }

        _currentValue = newValue;
        FireValueChanged(newValue);
    }

    protected override void SetValueImpl(object target, object? value)
    {
        var spinner = (MvxAppCompatSpinner)target;

        if (value == null)
        {
            MvxBindingLog.Instance?.LogWarning(
                "Null values not permitted in spinner SelectedItem binding currently");
            return;
        }

        if (!value.Equals(_currentValue))
        {
            var index = spinner.Adapter.GetPosition(value);
            if (index < 0)
            {
                MvxBindingLog.Instance?.LogWarning("Value not found for spinner @{Value}", value);
                return;
            }
            _currentValue = value;
            spinner.SetSelection(index);
        }
    }

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    public override void SubscribeToEvents()
    {
        var spinner = Spinner;
        if (spinner == null)
            return;

        _subscription = spinner.WeakSubscribe<MvxAppCompatSpinner, AdapterView.ItemSelectedEventArgs>(
            nameof(spinner.ItemSelected),
            SpinnerItemSelected);
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
