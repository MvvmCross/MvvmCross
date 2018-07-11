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
using MvvmCross.Presenters;
using MvvmCross.IoC;

namespace MvvmCross.Platforms.Console.Core
{
    public abstract class MvxConsoleSetup
        : MvxSetup
    {
        protected override void RegisterViewPresenter()
        {
            // TODO: Should there be a console presenter?
            //Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewPresenter, MvxConsoleViewPresenter>();
            //Mvx.IoCProvider.CallbackWhenRegistered<IMvxViewPresenter>(presenter => Mvx.IoCProvider.RegisterSingleton((IMvxConsoleViewPresenter)presenter));
        }

        protected override void RegisterViewsContainer()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewsContainer, MvxConsoleContainer>();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxViewsContainer>(container => Mvx.IoCProvider.RegisterSingleton((IMvxConsoleNavigation)container));
        }

        protected override void RegisterViewDispatcher()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewDispatcher, MvxConsoleViewDispatcher>();
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View");
        }

        public virtual void InitializeMessagePump()
        {
            var messagePump = new MvxConsoleMessagePump();
            Mvx.IoCProvider.RegisterSingleton<IMvxMessagePump>(messagePump);
            Mvx.IoCProvider.RegisterSingleton<IMvxConsoleCurrentView>(messagePump);
        }

        protected override void InitializeLastChance()
        {
            InitializeMessagePump();
        }
    }

    public class MvxConsoleSetup<TApplication> : MvxConsoleSetup
        where TApplication : class, IMvxApplication, new()
    {
        protected override void RegisterApp()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxApplication, TApplication>();
        }

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}
