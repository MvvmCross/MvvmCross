// MvxLiteralSourceStepFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxLiteralSourceStepFactory : MvxTypedSourceStepFactory<MvxLiteralSourceStepDescription>
    {
        protected override IMvxSourceStep TypedCreate(MvxLiteralSourceStepDescription description)
        {
            var toReturn = new MvxLiteralSourceStep(description);
            return toReturn;
        }
    }
}