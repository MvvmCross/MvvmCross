// MvxConditionalConventionalAttribute.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.IoC
{
    using System;

    public abstract class MvxConditionalConventionalAttribute : Attribute
    {
        public abstract bool IsConditionSatisfied { get; }
    }
}