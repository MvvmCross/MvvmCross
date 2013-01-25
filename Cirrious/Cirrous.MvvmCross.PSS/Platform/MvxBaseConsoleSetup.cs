#region Copyright
// <copyright file="MvxBasePssSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Cirrious.MvvmCross.Pss.Interfaces;
using Cirrious.MvvmCross.Pss.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Plugins;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Pss.Platform
{
    public class MvxPssPluginManager : MvxFileBasedPluginManager
    {
        public MvxPssPluginManager()
            : base("Pss")
        {
        }

        protected override Assembly LoadAssembly(Type toLoad)
        {
            MvxTrace.Trace("Loading plugin for {0}", toLoad.AssemblyQualifiedName);
            var fileName = GetPluginAssemblyNameFrom(toLoad);
            MvxTrace.Trace("- plugin assembly is {0}", fileName);
            var assembly = Assembly.LoadFrom(fileName);
            return assembly;
        }

        protected override string GetPluginAssemblyNameFrom(Type toLoad)
        {
            var fileName = base.GetPluginAssemblyNameFrom(toLoad);
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(location, fileName); 
        }
    }

    public abstract class MvxBasePssSetup 
        : MvxBaseSetup        
        , IMvxServiceProducer
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void InitializeDebugServices()
        {
            this.RegisterServiceInstance<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        public virtual void InitializeMessagePump()
        {
            var messagePump = new MvxPssMessagePump();
            this.RegisterServiceInstance<IMvxMessagePump>(messagePump);
            this.RegisterServiceInstance<IMvxPssCurrentView>(messagePump);
        }

        protected override MvxViewsContainer CreateViewsContainer()
        {
            var container = CreatePssContainer();
            this.RegisterServiceInstance<IMvxPssNavigation>(container);
            return container;
        }

        protected override IMvxViewDispatcherProvider CreateViewDispatcherProvider()
        {
            return new MvxPssDispatcherProvider();
        }

        protected virtual MvxBasePssContainer CreatePssContainer()
        {
            return new MvxPssContainer();
        }

        protected override void InitializeLastChance()
        {
            InitializeMessagePump();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            return new MvxPssPluginManager();
        }

        protected override IDictionary<System.Type, System.Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().Assembly, typeof(IMvxPssView));
        }
    }
}