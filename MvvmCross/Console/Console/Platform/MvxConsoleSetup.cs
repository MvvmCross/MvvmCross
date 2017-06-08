// MvxConsoleSetup.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Console.Platform
{
    using Views;
    using Core.Platform;
    using Core.ViewModels;
    using Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.Plugins;
    using Core.Platform;
    using Core.ViewModels;
    using Core.Views;

    public abstract class MvxConsoleSetup
        : MvxSetup
    {
        protected override IMvxTrace CreateDebugTrace()
        {
            return new MvxDebugTrace();
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View");
        }

        public virtual void InitializeMessagePump()
        {
            var messagePump = new MvxConsoleMessagePump();
            Mvx.RegisterSingleton<IMvxMessagePump>(messagePump);
            Mvx.RegisterSingleton<IMvxConsoleCurrentView>(messagePump);
        }

        protected override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateConsoleContainer();
            Mvx.RegisterSingleton<IMvxConsoleNavigation>(container);
            return container;
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxConsoleViewDispatcher();
        }

        protected virtual MvxBaseConsoleContainer CreateConsoleContainer()
        {
            return new MvxConsoleContainer();
        }

        protected override void InitializeLastChance()
        {
            InitializeMessagePump();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            // Console is 'full .net' - so uses the same plugins as Wpf
            return new MvxFilePluginManager(".Wpf", string.Empty);
        }
    }
}