// MvxTextProviderBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using MvvmCross.Localization;
using System;
using System.Collections.Generic;

namespace MvvmCross.Plugins.JsonLocalization
{
    public abstract class MvxTextProviderBuilder
        : IMvxTextProviderBuilder
    {
        private readonly string _generalNamespaceKey;
        private readonly string _rootFolderForResources;
        private readonly IMvxJsonDictionaryTextLoader _textLoader;
        private readonly IMvxTextProvider _textProvider;

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

        public void LoadResources(string whichLocalizationFolder)
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
                    MvxTrace.Warning("Language file could not be loaded for {0}.{1} - {2}",
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