// MvxPropertyInfoTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using MvvmCross.Binding.Attributes;
using MvvmCross.Platform;

namespace MvvmCross.Binding.Bindings.Target
{
    public class MvxPropertyInfoTargetBinding : MvxConvertingTargetBinding
    {
        public MvxPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target)
        {
            TargetPropertyInfo = targetPropertyInfo;
        }

        public override Type TargetType => TargetPropertyInfo.PropertyType;

        protected PropertyInfo TargetPropertyInfo { get; }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // if the target property should be set to NULL on dispose then we clear it here
                // this is a fix for the possible memory leaks discussion started https://github.com/slodge/MvvmCross/issues/17#issuecomment-8527392
                var setToNullAttribute = TargetPropertyInfo.GetCustomAttribute<MvxSetToNullAfterBindingAttribute>(true);
                if (setToNullAttribute != null)
                    SetValue(null);
            }

            base.Dispose(isDisposing);
        }

        protected override void SetValueImpl(object target, object value)
        {
#warning Check this is Unity compatible :/
            var setMethod = TargetPropertyInfo.GetSetMethod();
            setMethod.Invoke(target, new[] {value});
        }
    }

    public class MvxPropertyInfoTargetBinding<T> : MvxPropertyInfoTargetBinding
        where T : class
    {
        public MvxPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected T View => Target as T;
    }
}