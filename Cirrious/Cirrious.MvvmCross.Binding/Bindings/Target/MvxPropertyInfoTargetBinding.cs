// MvxPropertyInfoTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding.Attributes;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public class MvxPropertyInfoTargetBinding : MvxConvertingTargetBinding
    {
        private readonly PropertyInfo _targetPropertyInfo;

        public MvxPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
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
                var setToNullAttribute = Attribute.GetCustomAttribute(TargetPropertyInfo,
                                                                      typeof (MvxSetToNullAfterBindingAttribute), true);
                if (setToNullAttribute != null)
                {
                    SetValue(null);
                }
            }

            base.Dispose(isDisposing);
        }

        public override Type TargetType
        {
            get { return TargetPropertyInfo.PropertyType; }
        }

        protected PropertyInfo TargetPropertyInfo
        {
            get { return _targetPropertyInfo; }
        }

        protected override void SetValueImpl(object target, object value)
        {
#warning Check this is Unity compatible :/
            var setMethod = TargetPropertyInfo.GetSetMethod();
            setMethod.Invoke(target, new object[] { value });
        }
    }

    public class MvxPropertyInfoTargetBinding<T> : MvxPropertyInfoTargetBinding
        where T : class
    {
        public MvxPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected T View
        {
            get { return base.Target as T; }
        }
    }
}