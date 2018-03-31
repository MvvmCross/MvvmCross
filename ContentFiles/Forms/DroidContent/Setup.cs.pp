using Android.Content;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Platforms.Android;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;

namespace $rootnamespace$
{
    public class Setup : MvxFormsAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override MvxFormsApplication CreateFormsApplication()
        {
            return new Core.App();
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.CoreApp();
        }
    }
}
