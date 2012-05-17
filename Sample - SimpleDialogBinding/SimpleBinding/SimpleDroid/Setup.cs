using System.Reflection;
using Android.Content;
using Cirrious.MvvmCross.Binding.Android.Simple;

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