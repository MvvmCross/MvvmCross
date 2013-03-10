// MvxTextProviderBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Localization.Interfaces;

namespace Cirrious.MvvmCross.Plugins.JsonLocalisation
{
    public abstract class MvxTextProviderBuilder
        : IMvxTextProviderBuilder
    {
        private readonly string _generalNamespaceKey;
        private readonly string _rootFolderForResources;

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
            TextProvider = TextProvider;
            LoadResources(string.Empty);
        }

        protected abstract IDictionary<string, string> ResourceFiles { get; }

        public IMvxTextProvider TextProvider { get; private set; }

        private IMvxJsonDictionaryTextLoader _textLoader;

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
                    MvxTrace.Trace(MvxTraceLevel.Warning, "Language file could not be loaded for {0}.{1} - {2}",
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