#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxNumberPickerValueTargetBinding(
    [DynamicallyAccessedMembers(
        DynamicallyAccessedMemberTypes.PublicEvents |
                    DynamicallyAccessedMemberTypes.PublicProperties)]
        object target,
        PropertyInfo targetPropertyInfo)
    : MvxPropertyInfoTargetBinding<NumberPicker>(target, targetPropertyInfo)
{
    private IDisposable? _subscription;

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    protected override void SetValueImpl(object target, object? value)
    {
        var numberPicker = (NumberPicker?)target;
        if (numberPicker == null)
            return;

        if (value != null)
            numberPicker.Value = (int)value;
    }

    private void NumberPickerValueChanged(object? sender, NumberPicker.ValueChangeEventArgs e)
    {
        if (!e.OldVal.Equals(e.NewVal))
            FireValueChanged(e.NewVal);
    }

    public override void SubscribeToEvents()
    {
        var numberPicker = View;
        if (numberPicker == null)
        {
            MvxBindingLog.Instance?.LogError("NumberPicker is null in MvxNumberPickerValueTargetBinding");
            return;
        }

        _subscription = numberPicker.WeakSubscribe<NumberPicker, NumberPicker.ValueChangeEventArgs>(
            nameof(numberPicker.ValueChanged),
            NumberPickerValueChanged);
    }

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
