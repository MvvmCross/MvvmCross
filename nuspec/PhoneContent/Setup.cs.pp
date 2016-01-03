using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;

namespace $rootnamespace$
{
    public class Setup : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame) : base(rootFrame)
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
