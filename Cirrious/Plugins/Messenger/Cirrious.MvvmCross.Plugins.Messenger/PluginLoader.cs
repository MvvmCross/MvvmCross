﻿// PluginLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.Plugins;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ExtensionMethods;

namespace Cirrious.MvvmCross.Plugins.Messenger
{
    public class PluginLoader
        : IMvxPluginLoader
          , IMvxServiceProducer
    {
        public static readonly PluginLoader Instance = new PluginLoader();

        private bool _loaded;

        #region Implementation of IMvxPluginLoader

        public void EnsureLoaded()
        {
            if (_loaded)
            {
                return;
            }

            this.RegisterServiceInstance<IMessenger>(new MessengerHub());
            _loaded = true;
        }

        #endregion
    }
}