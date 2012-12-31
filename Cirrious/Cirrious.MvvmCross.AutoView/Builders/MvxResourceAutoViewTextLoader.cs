#region Copyright

// <copyright file="MvxResourceAutoViewTextLoader.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Plugins.ResourceLoader;

namespace Cirrious.MvvmCross.AutoView.Builders
{
    public class MvxResourceAutoViewTextLoader : IMvxServiceConsumer, IMvxAutoViewTextLoader
    {
        private static bool _ensureLoadedCalled;

        private static void EnsureLoaded()
        {
            if (_ensureLoadedCalled)
                return;

            Cirrious.MvvmCross.Plugins.ResourceLoader.PluginLoader.Instance.EnsureLoaded();
            _ensureLoadedCalled = true;
        }

        public bool HasDefinition(Type viewModelType, string key)
        {
            EnsureLoaded();
            var service = this.GetService<IMvxResourceLoader>();
            var path = PathForView(viewModelType.Name, key);
            return service.ResourceExists(path);
        }

        public string GetDefinition(Type viewModelType, string key)
        {
            EnsureLoaded();
            var service = this.GetService<IMvxResourceLoader>();
            var path = PathForView(viewModelType.Name, key);
            try
            {
                return service.GetTextResource(path);
            }
            catch (MvxException)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "Definition file not loaded {0}", path);
                return null;
            }
        }

        private static string PathForView(string viewType, string key)
        {
            var path = string.Format("DefaultViews/{0}/{1}.json", viewType, key);
            return path;
        }
    }
}