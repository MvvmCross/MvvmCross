namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxCombinerSourceStepFactory : MvxTypedSourceStepFactory<MvxCombinerSourceStepDescription>
    {
        protected override IMvxSourceStep TypedCreate(MvxCombinerSourceStepDescription description)
        {
            var toReturn = new MvxCombinerSourceStep(description);
            return toReturn;
        }
    }
}