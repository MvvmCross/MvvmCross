﻿// MvxWindowsTargetBindingFactoryRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


namespace MvvmCross.BindingEx.WindowsPhone.MvxBinding

{
    using global::MvvmCross.Binding;
    using global::MvvmCross.Binding.Bindings.Target;
    using global::MvvmCross.Binding.Bindings.Target.Construction;
    using global::MvvmCross.Platform.Platform;

    using MvvmCross.BindingEx.WindowsPhone.MvxBinding.Target;

    public class MvxWindowsTargetBindingFactoryRegistry : MvxTargetBindingFactoryRegistry
    {
        protected override bool TryCreateReflectionBasedBinding(object target, string targetName,
                                                                out IMvxTargetBinding binding)
        {
            if (this.TryCreatePropertyDependencyBasedBinding(target, targetName, out binding))
            {
                return true;
            }

            return base.TryCreateReflectionBasedBinding(target, targetName, out binding);
        }

        private bool TryCreatePropertyDependencyBasedBinding(object target, string targetName,
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