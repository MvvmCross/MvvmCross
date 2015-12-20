// MvxFormFactorSpecificAttribute.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Touch.Views
{
    using System;

    using MvvmCross.Platform;
    using MvvmCross.Platform.IoC;
    using MvvmCross.Touch.Platform;

    [AttributeUsage(AttributeTargets.Class)]
    public class MvxFormFactorSpecificAttribute
        : MvxConditionalConventionalAttribute
    {
        public MvxFormFactorSpecificAttribute(MvxTouchFormFactor target)
        {
            this.Target = target;
        }

        public MvxTouchFormFactor Target { get; private set; }

        public override bool IsConditionSatisfied
        {
            get
            {
                var properties = Mvx.Resolve<IMvxTouchPlatformProperties>();
                return (properties.FormFactor == this.Target);
            }
        }
    }
}