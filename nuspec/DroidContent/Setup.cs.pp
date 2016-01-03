using Android.Content;
using MvvmCross.Core.Platform;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;

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

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}
