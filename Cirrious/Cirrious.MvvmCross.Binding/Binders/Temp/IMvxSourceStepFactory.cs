namespace Cirrious.MvvmCross.Binding.Binders
{
    public interface IMvxSourceStepFactory
    {
        IMvxSourceStep Create(MvxSourceStepDescription description);
    }
}