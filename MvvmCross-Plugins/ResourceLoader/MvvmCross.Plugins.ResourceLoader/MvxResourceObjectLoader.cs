// MvxResourceObjectLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using System.Collections.Generic;
using System.IO;

namespace MvvmCross.Plugins.ResourceLoader
{
    public abstract class MvxResourceObjectLoader<TResource>
        : MvxResourceProvider
          , IMvxResourceObjectLoaderConfiguration<TResource>
          , IMvxResourceObjectLoader<TResource>

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

        #endregion Implementation of IMvxSoundEffectLoaderConfiguration

        #region Implementation of IMvxResourceObjectLoader<TResource>

        public TResource Load(string namespaceKey, string typeKey, string entryKey)
        {
            var streamLocation = GetStreamLocation(namespaceKey, typeKey, entryKey);
            var resourceLoader = Mvx.Resolve<IMvxResourceLoader>();
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
                specificRootLocation = $"{_generalRootLocation}/{namespaceKey}/{typeKey}";
            }

            var streamLocation = $"{specificRootLocation}/{entryKey}";
            return streamLocation;
        }

        #endregion Implementation of IMvxResourceObjectLoader<TResource>
    }
}