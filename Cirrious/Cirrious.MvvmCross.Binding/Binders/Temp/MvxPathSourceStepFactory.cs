namespace Cirrious.MvvmCross.Binding.Binders
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