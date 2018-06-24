// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Runtime;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public abstract class MvxAndroidTargetBinding
        : MvxConvertingTargetBinding
    {
        private IMvxAndroidGlobals _androidGlobals;

        protected MvxAndroidTargetBinding(object target)
            : base(target)
        {
        }

        protected IMvxAndroidGlobals AndroidGlobals
            => _androidGlobals ?? (_androidGlobals = Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>());

        protected override bool ShouldSkipSetValueForPlatformSpecificReasons(object target, object value)
        {
            return TargetIsInvalid(target);
        }

        public static bool TargetIsInvalid(object target)
        {
            var javaTarget = target as IJavaObject;
            if (javaTarget != null && javaTarget.Handle == IntPtr.Zero)
            {
                MvxBindingLog.Warning("Weak Target has been GCed by Android {0}", javaTarget.GetType().Name);
                return true;
            }
            return false;
        }
    }

    public abstract class MvxAndroidTargetBinding<TTarget, TValue>
        : MvxConvertingTargetBinding<TTarget, TValue>
        where TTarget : class
    {
        private IMvxAndroidGlobals _androidGlobals;

        protected MvxAndroidTargetBinding(TTarget target)
            : base(target)
        {
        }

        protected IMvxAndroidGlobals AndroidGlobals
            => _androidGlobals ?? (_androidGlobals = Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>());

        protected override bool ShouldSkipSetValueForPlatformSpecificReasons(TTarget target, TValue value)
        {
            return TargetIsInvalid(target);
        }

        public static bool TargetIsInvalid(TTarget target)
        {
            var javaTarget = target as IJavaObject;
            if (javaTarget != null && javaTarget.Handle == IntPtr.Zero)
            {
                MvxBindingLog.Warning("Weak Target has been GCed by Android {0}", javaTarget.GetType().Name);
                return true;
            }
            return false;
        }
    }
}
