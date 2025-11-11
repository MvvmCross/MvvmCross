// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxAdapterViewSelectedItemPositionTargetBinding(AdapterView adapterView)
    : MvxAndroidTargetBinding(adapterView)
{
    private MvxAndroidTargetEventSubscription<AdapterView, AdapterView.ItemSelectedEventArgs>? _subscription;

    private AdapterView? AdapterView => (AdapterView?)Target;

    protected override void SetValueImpl(object target, object? value)
    {
        if (value != null)
            AdapterView?.SetSelection((int)value);
    }

    private void AdapterViewOnItemSelected(object? sender, AdapterView.ItemSelectedEventArgs itemSelectedEventArgs)
    {
        FireValueChanged(itemSelectedEventArgs.Position);
    }

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
    public override void SubscribeToEvents()
    {
        var view = AdapterView;

        if (view == null)
            return;

        _subscription = view.WeakSubscribe<AdapterView, AdapterView.ItemSelectedEventArgs>(
            nameof(adapterView.ItemSelected), AdapterViewOnItemSelected);
    }

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public override Type TargetValueType => typeof(int);

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
}
