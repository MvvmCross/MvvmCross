// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Diagnostics.CodeAnalysis;
using Android.Text;
using MvvmCross.Binding;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxTextViewTextFormattedTargetBinding(TextView target)
    : MvxAndroidTargetBinding(target), IMvxEditableTextView
{
    private readonly bool _isEditTextBinding = target is EditText;
    private MvxAndroidTargetEventSubscription<TextView, AfterTextChangedEventArgs>? _subscription;

    protected TextView? TextView => Target as TextView;

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public override Type TargetValueType => typeof(ISpanned);

    [RequiresUnreferencedCode("This method uses reflection to get type information and perform conversions which may not be preserved by trimming.")]
    protected override bool ShouldSkipSetValueForViewSpecificReasons(object target, object? value)
    {
        if (!_isEditTextBinding)
            return false;

        return this.ShouldSkipSetValueAsHaveNearlyIdenticalNumericText(target, value);
    }

    protected override void SetValueImpl(object target, object? value)
    {
        ((TextView)target).TextFormatted = (ISpanned?)value;
    }

    public override MvxBindingMode DefaultMode => _isEditTextBinding ? MvxBindingMode.TwoWay : MvxBindingMode.OneWay;

    [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
    public override void SubscribeToEvents()
    {
        var view = TextView;
        if (view == null)
            return;

        _subscription = view.WeakSubscribe<TextView, AfterTextChangedEventArgs>(
            nameof(view.AfterTextChanged),
            EditTextOnAfterTextChanged);
    }

    private void EditTextOnAfterTextChanged(object? sender, AfterTextChangedEventArgs afterTextChangedEventArgs)
    {
        FireValueChanged(TextView?.TextFormatted);
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _subscription?.Dispose();
            _subscription = null;
        }
        base.Dispose(isDisposing);
    }

    public string? CurrentText
    {
        get
        {
            var view = TextView;
            return view?.TextFormatted?.ToString();
        }
    }
}
