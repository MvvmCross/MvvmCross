// MvxFormFactorSpecificViewAttribute.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.Views
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxFormFactorSpecificViewAttribute
        : MvxConditionalConventionalViewAttribute
    {
        public MvxFormFactorSpecificViewAttribute(MvxTouchFormFactor target)
        {
            Target = target;
        }

        public MvxTouchFormFactor Target { get; private set; }

        public override bool IsConventional
        {
            get
            {
                var properties = Mvx.Resolve<IMvxTouchPlatformProperties>();
                return (properties.FormFactor == Target);
            }
        }
    }
}