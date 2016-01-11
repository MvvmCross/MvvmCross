using MvvmCross.Binding.Droid;
using MvvmCross.Binding.Droid.Binders;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public class MvxAppCompatBindingBuilder : MvxAndroidBindingBuilder
    {
        protected override IMvxAndroidViewFactory CreateAndroidViewFactory()
        {
            return new MvxAppCompatViewFactory();
        }
    }
}