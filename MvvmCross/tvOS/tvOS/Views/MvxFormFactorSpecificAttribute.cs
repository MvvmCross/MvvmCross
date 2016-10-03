// MvxFormFactorSpecificAttribute.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.tvOS.Views
{
    using System;

    using MvvmCross.Platform;
    using MvvmCross.Platform.IoC;
    using MvvmCross.tvOS.Platform;

    [AttributeUsage(AttributeTargets.Class)]
    public class MvxFormFactorSpecificAttribute
        : MvxConditionalConventionalAttribute
    {
        public MvxFormFactorSpecificAttribute(MvxTvosFormFactor target)
        {
            this.Target = target;
        }

        public MvxTvosFormFactor Target { get; private set; }

        public override bool IsConditionSatisfied
        {
            get
            {
                var properties = Mvx.Resolve<IMvxTvosPlatformProperties>();
                return (properties.FormFactor == this.Target);
            }
        }
    }
}