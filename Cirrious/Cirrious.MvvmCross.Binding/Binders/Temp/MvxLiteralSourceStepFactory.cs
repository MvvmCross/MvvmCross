namespace Cirrious.MvvmCross.Binding.Binders
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