using Android.Content;
using Cirrious.MvvmCross.Binding.Droid.Simple;

namespace SimpleDroid
{
    public class Setup 
        : MvxSimpleAndroidBindingSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext, typeof(Converters.Converters))
        {
        }
    }
}