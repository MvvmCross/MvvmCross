using System;

namespace Cirrious.MvvmCross.Binding.Binders
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
                    return typeof (object);

                return Description.Literal.GetType();
            }
        }

        protected override void SetSourceValue(object sourceValue)
        {
            // ignored - there is no way to set the source value
        }

        protected override bool TryGetSourceValue(out object value)
        {
            value = Description.Literal;
            return true;
        }
    }
}