// MvxBaseTextProviderBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Plugins.JsonLocalisation
{
    public abstract class MvxTextProviderBuilder
        : IMvxTextProviderBuilder
    {
        private readonly string _generalNamespaceKey;
        private readonly string _rootFolderForResources;

        protected MvxTextProviderBuilder(string generalNamespaceKey, string rootFolderForResources)
            : this(generalNamespaceKey, rootFolderForResources, new MvxResourceJsonDictionaryTextProvider(true))
        {
        }

        protected MvxTextProviderBuilder(string generalNamespaceKey, string rootFolderForResources,
                                         MvxJsonDictionaryTextProvider provider)
        {
            _generalNamespaceKey = generalNamespaceKey;
            _rootFolderForResources = rootFolderForResources;

            TextProvider = provider;
            LoadResources(string.Empty);
        }

        protected abstract IDictionary<string, string> ResourceFiles { get; }

        #region IMvxTextProviderBuilder Members

        public MvxJsonDictionaryTextProvider TextProvider { get; private set; }

        public void LoadResources(string whichLocalisationFolder)
        {
            foreach (var kvp in ResourceFiles)
            {
                try
                {
                    TextProvider.LoadJsonFromResource(_generalNamespaceKey, kvp.Key,
                                                      GetResourceFilePath(whichLocalisationFolder, kvp.Value));
                }
                catch (Exception exception)
                {
                    MvxTrace.Trace(MvxTraceLevel.Warning, "Language file could not be loaded for {0}.{1} - {2}",
                                   whichLocalisationFolder, kvp.Key, exception.ToLongString());
                }
            }
        }

        #endregion

        protected virtual string GetResourceFilePath(string whichLocalisationFolder, string whichFile)
        {
            if (string.IsNullOrEmpty(whichLocalisationFolder))
                return string.Format("{0}/{1}.json", _rootFolderForResources, whichFile);
            else
                return string.Format("{0}/{1}/{2}.json", _rootFolderForResources, whichLocalisationFolder, whichFile);
        }
    }
}