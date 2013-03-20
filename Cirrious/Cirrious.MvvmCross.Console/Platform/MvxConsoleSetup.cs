// MvxConsoleSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Console.Views;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Console.Platform
{
    public abstract class MvxConsoleSetup
        : MvxSetup

    {
        protected override void InitializeDebugServices()
        {
            Mvx.RegisterSingleton<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        public virtual void InitializeMessagePump()
        {
            var messagePump = new MvxConsoleMessagePump();
            Mvx.RegisterSingleton<IMvxMessagePump>(messagePump);
            Mvx.RegisterSingleton<IMvxConsoleCurrentView>(messagePump);
        }

        protected override MvxViewsContainer CreateViewsContainer()
        {
            var container = CreateConsoleContainer();
            Mvx.RegisterSingleton<IMvxConsoleNavigation>(container);
            return container;
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxConsoleDispatcher();
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
            return new MvxConsolePluginManager();
        }

        protected override IDictionary<System.Type, System.Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().Assembly, typeof (IMvxConsoleView));
        }
    }
}