// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding
    : MvxAndroidPropertyInfoTargetBinding<MvxAppCompatAutoCompleteTextView>
{
    private MvxJavaEventSubscription<MvxAppCompatAutoCompleteTextView>? _subscription;

    public MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding(
        MvxAppCompatAutoCompleteTextView target, PropertyInfo targetPropertyInfo)
        : base(target, targetPropertyInfo)
    {
        var autoComplete = View;
        if (autoComplete == null)
        {
            MvxBindingLog.Instance?.LogError(
                "autoComplete is null in MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding");
        }
    }

    private void AutoCompleteOnPartialTextChanged(object? sender, EventArgs eventArgs)
    {
        FireValueChanged(View?.PartialText);
    }

    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWayToSource;

    public override void SubscribeToEvents()
    {
        var autoComplete = View;
        if (autoComplete == null)
            return;

        _subscription = autoComplete.WeakSubscribe(
            nameof(autoComplete.PartialTextChanged),
            AutoCompleteOnPartialTextChanged);
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
