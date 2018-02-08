using MvvmCross.Core.ViewModels;
using System.Windows.Controls;
using System.Windows.Threading;
using MvvmCross.Platform.Wpf.Core;

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
