namespace Cirrious.MvvmCross.Binding.Binders
{
    public abstract class MvxTypedSourceStepFactory<T>
        : IMvxSourceStepFactory
        where T : MvxSourceStepDescription
    {
        public IMvxSourceStep Create(MvxSourceStepDescription description)
        {
            return TypedCreate((T)description);
        }

        protected abstract IMvxSourceStep TypedCreate(T description);
    }
}