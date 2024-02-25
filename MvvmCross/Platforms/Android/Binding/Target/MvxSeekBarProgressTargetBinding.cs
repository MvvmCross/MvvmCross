// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxSeekBarProgressTargetBinding
    : MvxPropertyInfoTargetBinding<SeekBar>
{
    private MvxWeakEventSubscription<SeekBar, SeekBar.ProgressChangedEventArgs>? _subscription;

    public MvxSeekBarProgressTargetBinding(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.PublicProperties)]
        object target, PropertyInfo targetPropertyInfo)
        : base(target, targetPropertyInfo)
    {
    }

    protected override void SetValueImpl(object target, object? value)
    {
        var seekbar = (SeekBar?)target;
        if (seekbar == null || value == null)
            return;

        seekbar.Progress = (int)value;
    }

    private void SeekBarProgressChanged(object? sender, SeekBar.ProgressChangedEventArgs e)
    {
        if (e.FromUser)
            FireValueChanged(e.Progress);
    }

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    public override void SubscribeToEvents()
    {
        var seekBar = View;
        if (seekBar == null)
        {
            MvxBindingLog.Instance?.LogError("SeekBar is null in MvxSeekBarProgressTargetBinding");
            return;
        }

        _subscription = seekBar.WeakSubscribe<SeekBar, SeekBar.ProgressChangedEventArgs>(
            nameof(seekBar.ProgressChanged),
            SeekBarProgressChanged);
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
