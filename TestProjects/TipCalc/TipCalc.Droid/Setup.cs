using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform.Logging;

namespace TipCalc.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context appContext) : base(appContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override MvxLogProviderType GetDefaultLogProviderType()
        {
            return MvxLogProviderType.None;
        }
    }
}