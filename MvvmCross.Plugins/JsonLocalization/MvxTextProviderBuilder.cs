// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Localization;
using MvvmCross.Base.Exceptions;
using MvvmCross.Base.Logging;
using MvvmCross.Base.Platform;

namespace MvvmCross.Plugin.JsonLocalization
{
    public abstract class MvxTextProviderBuilder
        : IMvxTextProviderBuilder
    {
        protected readonly string _generalNamespaceKey;
        protected readonly string _rootFolderForResources;
        protected readonly IMvxJsonDictionaryTextLoader _textLoader;
        protected readonly IMvxTextProvider _textProvider;

        protected MvxTextProviderBuilder(string generalNamespaceKey, string rootFolderForResources)
            : this(generalNamespaceKey, rootFolderForResources, new MvxContentJsonDictionaryTextProvider())
        {
        }

        protected MvxTextProviderBuilder(string generalNamespaceKey, string rootFolderForResources, MvxJsonDictionaryTextProvider provider)
            : this(generalNamespaceKey, rootFolderForResources, provider, provider)
        {
        }

        protected MvxTextProviderBuilder(string generalNamespaceKey, string rootFolderForResources, IMvxJsonDictionaryTextLoader textLoader, IMvxTextProvider textProvider)
        {
            _generalNamespaceKey = generalNamespaceKey;
            _rootFolderForResources = rootFolderForResources;
            _textLoader = textLoader;
            _textProvider = textProvider;
            LoadResources(string.Empty);
        }

        protected abstract IDictionary<string, string> ResourceFiles { get; }

        public IMvxTextProvider TextProvider => _textProvider;

        public virtual void LoadResources(string whichLocalizationFolder)
        {
            foreach (var kvp in ResourceFiles)
            {
                try
                {
                    _textLoader.LoadJsonFromResource(_generalNamespaceKey, kvp.Key,
                                                      GetResourceFilePath(whichLocalizationFolder, kvp.Value));
                }
                catch (Exception exception)
                {
                    MvxPluginLog.Instance.Warn("Language file could not be loaded for {0}.{1} - {2}",
                                   whichLocalizationFolder, kvp.Key, exception.ToLongString());
                }
            }
        }

        protected virtual string GetResourceFilePath(string whichLocalizationFolder, string whichFile)
        {
            if (string.IsNullOrEmpty(whichLocalizationFolder))
                return $"{_rootFolderForResources}/{whichFile}.json";
            else
                return $"{_rootFolderForResources}/{whichLocalizationFolder}/{whichFile}.json";
        }
    }
}
