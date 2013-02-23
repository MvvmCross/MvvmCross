using Android.Content;
using Cirrious.MvvmCross.Droid.Simple;

namespace SimpleDroid
{
    public class Setup 
        : MvxSimpleAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext, typeof(Converters.Converters))
        {
        }
    }
}