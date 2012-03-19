#region Copyright
// <copyright file="MvxTextProviderBuilder.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Localization
{
    public abstract class MvxTextProviderBuilder 
        : IMvxTextProviderBuilder
    {
        private readonly string _generalNamespaceKey;
        private readonly string _rootFolderForResources;

        protected MvxTextProviderBuilder(string generalNamespaceKey, string rootFolderForResources)
        {
#warning Error masking turned on by default - check this is OK
            _generalNamespaceKey = generalNamespaceKey;
            _rootFolderForResources = rootFolderForResources;

            TextProvider = new MvxJsonDictionaryTextProvider(true);
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
                    TextProvider.LoadJsonFromResource(_generalNamespaceKey, kvp.Key, GetResourceFilePath(whichLocalisationFolder, kvp.Value));
                }
#if !NETFX_CORE
                catch (ThreadAbortException)
                {
                    throw;
                }
#endif
                catch (Exception exception)
                {
                    MvxTrace.Trace(MvxTraceLevel.Warning, "Language file could not be loaded for {0}.{1} - {2}", whichLocalisationFolder, kvp.Key, exception.ToLongString());
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