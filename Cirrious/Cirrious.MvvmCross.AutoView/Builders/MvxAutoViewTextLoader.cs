namespace Cirrious.MvvmCross.AutoView.Builders
{
    public class MvxAutoViewTextLoader : MvxCompositeAutoViewTextLoader
    {
        public MvxAutoViewTextLoader()
        {
            // note - order is important here...
            Add(new MvxResourceAutoViewTextLoader());
            Add(new MvxEmbeddedAutoViewTextLoader());
        }
    }
}