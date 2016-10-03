// MvxPropertyInfoTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Target
{
    using System;
    using System.Reflection;

    using MvvmCross.Binding.Attributes;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;

    public class MvxPropertyInfoTargetBinding : MvxConvertingTargetBinding
    {
        private readonly PropertyInfo _targetPropertyInfo;

        public MvxPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target)
        {
            this._targetPropertyInfo = targetPropertyInfo;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // if the target property should be set to NULL on dispose then we clear it here
                // this is a fix for the possible memory leaks discussion started https://github.com/slodge/MvvmCross/issues/17#issuecomment-8527392
                var setToNullAttribute = this.TargetPropertyInfo.GetCustomAttribute<MvxSetToNullAfterBindingAttribute>(true);
                if (setToNullAttribute != null)
                {
                    this.SetValue(null);
                }
            }

            base.Dispose(isDisposing);
        }

        public override Type TargetType => this.TargetPropertyInfo.PropertyType;

        protected PropertyInfo TargetPropertyInfo => this._targetPropertyInfo;

        protected override void SetValueImpl(object target, object value)
        {
#warning Check this is Unity compatible :/
            var setMethod = this.TargetPropertyInfo.GetSetMethod();

            if (setMethod != null)
            {
                setMethod.Invoke(target, new object[] { value });
            }
            else
            {
                throw new MvxException("No Set method found for property {0}. Check property not read-only", this.TargetPropertyInfo.Name);
            }
        }
    }

    public class MvxPropertyInfoTargetBinding<T> : MvxPropertyInfoTargetBinding
        where T : class
    {
        public MvxPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected T View => base.Target as T;
    }
}