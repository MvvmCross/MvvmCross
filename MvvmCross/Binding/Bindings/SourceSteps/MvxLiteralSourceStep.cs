// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Binding.Bindings.SourceSteps
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
