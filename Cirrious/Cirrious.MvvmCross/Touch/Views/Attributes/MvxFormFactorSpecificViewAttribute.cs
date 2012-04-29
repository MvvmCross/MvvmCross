#region Copyright
// <copyright file="MvxFormFactorSpecificViewAttribute.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Views.Attributes;

namespace Cirrious.MvvmCross.Touch.Views.Attributes
{
    public class MvxFormFactorSpecificViewAttribute
        : MvxConditionalConventionalViewAttribute
          , IMvxServiceConsumer<IMvxTouchPlatformProperties>
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