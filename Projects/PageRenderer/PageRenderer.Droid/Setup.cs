
using Android.Content;

using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using PageRendererExample;

namespace PageRendererExample.UI.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            Mvx.LazyConstructAndRegisterSingleton<IImageHolder, ImageHolder>();
            return new CoreApp();
        }
    }
}