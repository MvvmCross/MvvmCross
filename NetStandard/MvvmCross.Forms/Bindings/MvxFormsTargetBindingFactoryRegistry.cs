// MvxFormsTargetBindingFactoryRegistry.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Bindings.Target;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Forms.Bindings
{
    public class MvxFormsTargetBindingFactoryRegistry : MvxTargetBindingFactoryRegistry
    {
        protected override bool TryCreateReflectionBasedBinding(object target, string targetName,
                                                                out IMvxTargetBinding binding)
        {
            if (TryCreateBindablePropertyBasedBinding(target, targetName, out binding))
            {
                return true;
            }

            return base.TryCreateReflectionBasedBinding(target, targetName, out binding);
        }

        private static bool TryCreateBindablePropertyBasedBinding(object target, string targetName,
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
                                      "Empty binding target passed to MvxFormsTargetBindingFactoryRegistry");
                binding = null;
                return false;
            }

            var bindableProperty = target.GetType().FindBindableProperty(targetName);
            if (bindableProperty == null)
            {
                binding = null;
                return false;
            }

            var actualProperty = target.GetType().FindActualProperty(targetName);
            var actualPropertyType = actualProperty?.PropertyType ?? typeof(object);

            binding = new MvxBindablePropertyTargetBinding(target, bindableProperty, actualPropertyType);
            return true;
        }
    }
}