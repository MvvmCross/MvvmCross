// MvxFormFactorSpecificViewAttribute.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Views.Attributes;

namespace Cirrious.MvvmCross.Touch.Views.Attributes
{
    public class MvxFormFactorSpecificViewAttribute
        : MvxConditionalConventionalViewAttribute
          , IMvxServiceConsumer
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
				var properties = this.GetService<IMvxTouchPlatformProperties>();
                return (properties.FormFactor == Target);
            }
        }
    }
}