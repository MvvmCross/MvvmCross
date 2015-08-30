// MvxTextProviderBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Localization;

namespace Cirrious.MvvmCross.Plugins.JsonLocalisation
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

        public IMvxTextProvider TextProvider { get { return _textProvider; } }

        public void LoadResources(string whichLocalisationFolder)
        {
            foreach (var kvp in ResourceFiles)
            {
                try
                {
                    _textLoader.LoadJsonFromResource(_generalNamespaceKey, kvp.Key,
                                                      GetResourceFilePath(whichLocalisationFolder, kvp.Value));
                }
                catch (Exception exception)
                {
                    MvxTrace.Warning( "Language file could not be loaded for {0}.{1} - {2}",
                                   whichLocalisationFolder, kvp.Key, exception.ToLongString());
                }
            }
        }

        protected virtual string GetResourceFilePath(string whichLocalisationFolder, string whichFile)
        {
            if (string.IsNullOrEmpty(whichLocalisationFolder))
                return string.Format("{0}/{1}.json", _rootFolderForResources, whichFile);
            else
                return string.Format("{0}/{1}/{2}.json", _rootFolderForResources, whichLocalisationFolder, whichFile);
        }
    }
}