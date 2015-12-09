// MvxPathSourceStepFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxPathSourceStepFactory : MvxTypedSourceStepFactory<MvxPathSourceStepDescription>
    {
        protected override IMvxSourceStep TypedCreate(MvxPathSourceStepDescription description)
        {
            var toReturn = new MvxPathSourceStep(description);
            return toReturn;
        }
    }
}