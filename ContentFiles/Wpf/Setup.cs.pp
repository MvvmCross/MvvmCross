using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using System.Windows.Controls;
using System.Windows.Threading;

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
    }
}
