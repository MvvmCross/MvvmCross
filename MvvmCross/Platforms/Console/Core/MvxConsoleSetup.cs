// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Plugin;
using MvvmCross.Core;
using MvvmCross.Platforms.Console.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace MvvmCross.Platforms.Console.Core
{
    public abstract class MvxConsoleSetup
        : MvxSetup
    {
        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View");
        }

        public virtual Task InitializeMessagePump()
        {
            return Task.Run(() =>
            {
                var messagePump = new MvxConsoleMessagePump();
                Mvx.IoCProvider.RegisterSingleton<IMvxMessagePump>(messagePump);
                Mvx.IoCProvider.RegisterSingleton<IMvxConsoleCurrentView>(messagePump);
            });
        }

        protected override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateConsoleContainer();
            Mvx.IoCProvider.RegisterSingleton<IMvxConsoleNavigation>(container);
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

        protected override Task InitializeLastChance()
        {
            return InitializeMessagePump();
        }
    }

    public class MvxConsoleSetup<TApplication> : MvxConsoleSetup
        where TApplication : class, IMvxApplication, new()
    {
        protected override IMvxApplication CreateApp() => Mvx.IoCProvider.IoCConstruct<TApplication>();

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}
