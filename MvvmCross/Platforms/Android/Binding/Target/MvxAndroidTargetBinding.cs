// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using Android.Runtime;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Android.Binding.Target;

public abstract class MvxAndroidTargetBinding
    : MvxConvertingTargetBinding
{
    protected MvxAndroidTargetBinding(object target)
        : base(target)
    {
    }

    protected override bool ShouldSkipSetValueForPlatformSpecificReasons(object target, object? value)
    {
        return TargetIsInvalid(target);
    }

    public static bool TargetIsInvalid(object target)
    {
        if (target is IJavaObject javaTarget && javaTarget.Handle == IntPtr.Zero)
        {
            MvxBindingLog.Instance?.LogWarning("Weak Target has been GCed by Android {TargetTypeName}",
                javaTarget.GetType().Name);
            return true;
        }
        return false;
    }
}

public abstract class MvxAndroidTargetBinding<TTarget, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TValue>
    : MvxConvertingTargetBinding<TTarget, TValue>
    where TTarget : class
{
    protected MvxAndroidTargetBinding(TTarget target)
        : base(target)
    {
    }

    protected override bool ShouldSkipSetValueForPlatformSpecificReasons(TTarget target, TValue? value)
    {
        return TargetIsInvalid(target);
    }

    public static bool TargetIsInvalid(TTarget target)
    {
        if (target is IJavaObject javaTarget && javaTarget.Handle == IntPtr.Zero)
        {
            MvxBindingLog.Instance?.LogWarning("Weak Target has been GCed by Android {TargetTypeName}",
                javaTarget.GetType().Name);
            return true;
        }
        return false;
    }
}
