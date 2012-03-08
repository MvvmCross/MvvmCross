using System.Reflection;
using Android.Content;
using Cirrious.MvvmCross.Binding.Android.Simple;

namespace SimpleDroid
{
    public class Setup 
        : MvxSimpleAndroidBindingSetup
    {
        private static Setup _singleton;

        public static void EnsureInitialised(Context applicationContext)
        {
            if (_singleton != null)
                return;
            _singleton = new Setup(applicationContext);
            _singleton.Initialize();
        }

        private Setup(Context applicationContext)
            : base(applicationContext, typeof(Converters.Converters))
        {
        }
    }
}