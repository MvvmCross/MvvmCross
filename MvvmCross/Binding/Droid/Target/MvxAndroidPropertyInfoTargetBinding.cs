using System.Reflection;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Binding.Droid.Target
{
    public abstract class MvxAndroidPropertyInfoTargetBinding : MvxPropertyInfoTargetBinding
    {
        protected MvxAndroidPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            
        }

        protected override bool ShouldSkipSetValueForPlatformSpecificReasons(object target, object value)
        {
            return MvxAndroidTargetBinding.TargetIsInvalid(target);
        }
    }

    public abstract class MvxAndroidPropertyInfoTargetBinding<TView>
        : MvxPropertyInfoTargetBinding<TView> where TView : class
    {
        protected MvxAndroidPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override bool ShouldSkipSetValueForPlatformSpecificReasons(object target, object value)
        {
            return MvxAndroidTargetBinding.TargetIsInvalid(target);
        }
    }
}