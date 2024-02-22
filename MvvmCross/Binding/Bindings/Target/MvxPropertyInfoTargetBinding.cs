// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Reflection;
using MvvmCross.Binding.Attributes;

namespace MvvmCross.Binding.Bindings.Target;

public abstract class MvxPropertyInfoTargetBinding : MvxConvertingTargetBinding
{
    private readonly PropertyInfo _targetPropertyInfo;

    protected MvxPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
        : base(target)
    {
        _targetPropertyInfo = targetPropertyInfo;
    }

    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            // if the target property should be set to NULL on dispose then we clear it here
            // this is a fix for the possible memory leaks discussion started https://github.com/slodge/MvvmCross/issues/17#issuecomment-8527392
            var setToNullAttribute = TargetPropertyInfo.GetCustomAttribute<MvxSetToNullAfterBindingAttribute>(true);
            if (setToNullAttribute != null)
            {
                SetValue(null);
            }
        }

        base.Dispose(isDisposing);
    }

    public override Type TargetValueType => TargetPropertyInfo.PropertyType;

    protected PropertyInfo TargetPropertyInfo => _targetPropertyInfo;

    protected override void SetValueImpl(object target, object? value)
    {
        var setMethod = TargetPropertyInfo.GetSetMethod();
        setMethod?.Invoke(target, [value]);
    }
}

public abstract class MvxPropertyInfoTargetBinding<T>(
        object target, PropertyInfo targetPropertyInfo)
    : MvxPropertyInfoTargetBinding(target, targetPropertyInfo)
    where T : class
{
    protected T? View => Target as T;
}
