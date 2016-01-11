using Cirrious.MvvmCross.Binding.Droid;
using Cirrious.MvvmCross.Binding.Droid.Binders;

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