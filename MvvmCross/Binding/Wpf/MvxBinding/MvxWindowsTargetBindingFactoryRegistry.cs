// MvxWindowsTargetBindingFactoryRegistry.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platform.Platform;

#if WINDOWS_COMMON
using MvvmCross.BindingEx.WindowsCommon.MvxBinding.Target;

namespace MvvmCross.BindingEx.WindowsCommon.MvxBinding
#endif

#if WINDOWS_WPF
using MvvmCross.BindingEx.Wpf.MvxBinding.Target;

namespace MvvmCross.BindingEx.Wpf.MvxBinding
#endif
{
    public class MvxWindowsTargetBindingFactoryRegistry : MvxTargetBindingFactoryRegistry
    {
        protected override bool TryCreateReflectionBasedBinding(object target, string targetName,
                                                                out IMvxTargetBinding binding)
        {
            if (TryCreatePropertyDependencyBasedBinding(target, targetName, out binding))
            {
                return true;
            }

            return base.TryCreateReflectionBasedBinding(target, targetName, out binding);
        }

        private static bool TryCreatePropertyDependencyBasedBinding(object target, string targetName,
                                                             out IMvxTargetBinding binding)
        {
            if (target == null)
            {
                binding = null;
                return false;
            }

            if (string.IsNullOrEmpty(targetName))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Empty binding target passed to MvxWindowsTargetBindingFactoryRegistry");
                binding = null;
                return false;
            }

            var dependencyProperty = target.GetType().FindDependencyProperty(targetName);
            if (dependencyProperty == null)
            {
                binding = null;
                return false;
            }

            var actualProperty = target.GetType().FindActualProperty(targetName);
            var actualPropertyType = actualProperty?.PropertyType ?? typeof(object);

            binding = new MvxDependencyPropertyTargetBinding(target, targetName, dependencyProperty, actualPropertyType);
            return true;
        }
    }
}