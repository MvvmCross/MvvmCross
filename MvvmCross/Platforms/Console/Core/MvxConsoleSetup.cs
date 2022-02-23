// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Core;
using MvvmCross.IoC;
using MvvmCross.Platforms.Console.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Console.Core
{
#nullable enable
    public abstract class MvxConsoleSetup
        : MvxSetup
    {
        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View");
        }

        public virtual void InitializeMessagePump(IMvxIoCProvider iocProvider)
        {
            if (iocProvider == null)
                throw new ArgumentNullException(nameof(iocProvider));

            var messagePump = new MvxConsoleMessagePump();
            iocProvider.RegisterSingleton<IMvxMessagePump>(messagePump);
            iocProvider.RegisterSingleton<IMvxConsoleCurrentView>(messagePump);
        }

        protected override IMvxViewsContainer CreateViewsContainer(IMvxIoCProvider iocProvider)
        {
            if (iocProvider == null)
                throw new ArgumentNullException(nameof(iocProvider));

            var container = CreateConsoleContainer();
            iocProvider.RegisterSingleton<IMvxConsoleNavigation>(container);
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

        protected override void InitializeLastChance(IMvxIoCProvider iocProvider)
        {
            InitializeMessagePump(iocProvider);
        }
    }

    public abstract class MvxConsoleSetup<TApplication> : MvxConsoleSetup
        where TApplication : class, IMvxApplication, new()
    {
        protected override IMvxApplication CreateApp(IMvxIoCProvider iocProvider) =>
            iocProvider.IoCConstruct<TApplication>();

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
#nullable restore
}
