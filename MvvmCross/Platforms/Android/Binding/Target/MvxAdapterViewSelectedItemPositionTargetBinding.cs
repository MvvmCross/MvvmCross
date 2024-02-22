// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxAdapterViewSelectedItemPositionTargetBinding
    : MvxAndroidTargetBinding
{
    private IDisposable? _subscription;
        
    private AdapterView? AdapterView => (AdapterView?)Target;

    public MvxAdapterViewSelectedItemPositionTargetBinding(AdapterView adapterView)
        : base(adapterView)
    {
    }

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

    public override void SubscribeToEvents()
    {
        var adapterView = AdapterView;

        if (adapterView == null)
            return;

        _subscription = adapterView.WeakSubscribe<AdapterView, AdapterView.ItemSelectedEventArgs>(
            nameof(adapterView.ItemSelected), AdapterViewOnItemSelected);
    }

    public override Type TargetValueType => typeof(int);

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
