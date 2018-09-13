// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.IO;
using MvvmCross.Base;

namespace MvvmCross.Plugin.JsonLocalization
{
    public class MvxContentJsonDictionaryTextProvider
        : MvxJsonDictionaryTextProvider
    {
        private IMvxResourceLoader _resourceLoader;

        protected IMvxResourceLoader ResourceLoader
        {
            get
            {
                _resourceLoader = _resourceLoader ?? Mvx.IoCProvider.Resolve<IMvxResourceLoader>();
                return _resourceLoader;
            }
        }

        public MvxContentJsonDictionaryTextProvider(bool maskErrors = true)
            : base(maskErrors)
        {
        }

        public override void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath)
        {
            var service = ResourceLoader;
            var json = service.GetTextResource(resourcePath);
            if (string.IsNullOrEmpty(json))
                throw new FileNotFoundException("Unable to find resource file " + resourcePath);
            LoadJsonFromText(namespaceKey, typeKey, json);
        }
    }
}
