using System.Windows.Controls;
using System.Windows.Threading;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Wpf.Platform;
using MvvmCross.Wpf.Views;

namespace $rootnamespace$
{
    public class Setup
        : MvxWpfSetup
    {
        public Setup(Dispatcher uiThreadDispatcher, ContentControl root)
            : base(uiThreadDispatcher, root)
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
