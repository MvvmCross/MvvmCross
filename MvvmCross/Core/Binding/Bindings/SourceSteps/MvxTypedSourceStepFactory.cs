// MvxTypedSourceStepFactory.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    public abstract class MvxTypedSourceStepFactory<T>
        : IMvxSourceStepFactory
        where T : MvxSourceStepDescription
    {
        public IMvxSourceStep Create(MvxSourceStepDescription description)
        {
            return this.TypedCreate((T)description);
        }

        protected abstract IMvxSourceStep TypedCreate(T description);
    }
}