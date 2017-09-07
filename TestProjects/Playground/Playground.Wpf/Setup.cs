using MvvmCross.Core.ViewModels;
using MvvmCross.Wpf.Platform;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Playground.Wpf
{
    public class Setup : MvxWpfSetup
    {
        public Setup(Dispatcher uiThreadDispatcher, ContentControl root) : base(uiThreadDispatcher, root)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
    }
}
