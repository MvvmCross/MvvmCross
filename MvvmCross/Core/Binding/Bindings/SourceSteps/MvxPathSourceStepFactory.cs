// MvxPathSourceStepFactory.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxPathSourceStepFactory : MvxTypedSourceStepFactory<MvxPathSourceStepDescription>
    {
        protected override IMvxSourceStep TypedCreate(MvxPathSourceStepDescription description)
        {
            return new MvxPathSourceStep(description);
        }
    }
}