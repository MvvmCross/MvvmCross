// MvxLiteralSourceStepDescription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxLiteralSourceStepDescription : MvxSourceStepDescription
    {
        public object Literal { get; set; }

        public override string ToString()
        {
            return Literal == null ? "-null-" : Literal.ToString();
        }
    }
}