// MvxAndroidTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Runtime;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Binding.Droid.Target
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
            => _androidGlobals ?? (_androidGlobals = Mvx.Resolve<IMvxAndroidGlobals>());

        protected override bool ShouldSkipSetValueForPlatformSpecificReasons(object target, object value)
        {
            return TargetIsInvalid(target);
        }

        public static bool TargetIsInvalid(object target)
        {
            var javaTarget = target as IJavaObject;
            if (javaTarget != null && javaTarget.Handle == IntPtr.Zero)
            {
                MvxLog.Instance.Warn("Weak Target has been GCed by Android {0}", javaTarget.GetType().Name);
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
            => _androidGlobals ?? (_androidGlobals = Mvx.Resolve<IMvxAndroidGlobals>());

        protected override bool ShouldSkipSetValueForPlatformSpecificReasons(TTarget target, TValue value)
        {
            return TargetIsInvalid(target);
        }

        public static bool TargetIsInvalid(TTarget target)
        {
            var javaTarget = target as IJavaObject;
            if (javaTarget != null && javaTarget.Handle == IntPtr.Zero)
            {
                MvxLog.Instance.Warn("Weak Target has been GCed by Android {0}", javaTarget.GetType().Name);
                return true;
            }
            return false;
        }
    }
}