using Android.Content;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.ViewModels;

namespace $rootnamespace$
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
    }
}
