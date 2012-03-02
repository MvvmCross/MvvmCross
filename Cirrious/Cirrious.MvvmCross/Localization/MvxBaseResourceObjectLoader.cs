#region Copyright
// <copyright file="MvxBaseResourceObjectLoader.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections.Generic;
using System.IO;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Localization
{
    public abstract class MvxBaseResourceObjectLoader<TResource>
        : MvxBaseResourceProvider
          , IMvxResourceObjectLoaderConfiguration<TResource>
          , IMvxResourceObjectLoader<TResource>
          , IMvxServiceConsumer<IMvxResourceLoader>
        where TResource : IMvxResourceObject
    {
        private readonly Dictionary<string, string> _rootLocations = new Dictionary<string, string>();
        private string _generalRootLocation = @"MvxResources";

        #region Implementation of IMvxSoundEffectLoaderConfiguration

        public void SetRootLocation(string location)
        {
            _generalRootLocation = location;
        }

        public void SetRootLocation(string namespaceKey, string typeKey, string location)
        {
            _rootLocations[MakeLookupKey(namespaceKey, typeKey)] = location;
        }

        #endregion

        #region Implementation of IMvxResourceObjectLoader<TResource>

        public TResource Load(string namespaceKey, string typeKey, string entryKey)
        {
            var streamLocation = GetStreamLocation(namespaceKey, typeKey, entryKey);
            var resourceLoader = this.GetService<IMvxResourceLoader>();
            TResource resource = default(TResource);
            resourceLoader.GetResourceStream(streamLocation, (stream) =>
                                                                 {
                                                                     if (stream != null)
                                                                         resource = Load(stream);
                                                                 });
            return resource;
        }

        protected abstract TResource Load(Stream stream);

        private string GetStreamLocation(string namespaceKey, string typeKey, string entryKey)
        {
            string specificRootLocation;
            if (!_rootLocations.TryGetValue(MakeLookupKey(namespaceKey, typeKey), out specificRootLocation))
            {
                specificRootLocation = string.Format("{0}/{1}/{2}",
                                                     _generalRootLocation,
                                                     namespaceKey,
                                                     typeKey);
            }

            var streamLocation = string.Format("{0}/{1}",
                                               specificRootLocation,
                                               entryKey);
            return streamLocation;
        }

        #endregion
    }
}