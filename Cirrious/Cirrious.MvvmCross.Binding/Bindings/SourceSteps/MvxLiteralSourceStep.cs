// MvxLiteralSourceStep.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxLiteralSourceStep : MvxSourceStep<MvxLiteralSourceStepDescription>
    {
        public MvxLiteralSourceStep(MvxLiteralSourceStepDescription description)
            : base(description)
        {
        }

        public override Type SourceType
        {
            get
            {
                if (Description.Literal == null)
                    return typeof(object);

                return Description.Literal.GetType();
            }
        }

        protected override void SetSourceValue(object sourceValue)
        {
            // ignored - there is no way to set the source value
        }

        protected override object GetSourceValue()
        {
            return Description.Literal;
        }
    }
}