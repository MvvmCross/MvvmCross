// MvxContentJsonDictionaryTextProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using System.IO;

namespace MvvmCross.Plugins.JsonLocalisation
{
    public class MvxContentJsonDictionaryTextProvider
        : MvxJsonDictionaryTextProvider
    {
        private IMvxResourceLoader _resourceLoader;

        protected IMvxResourceLoader ResourceLoader
        {
            get
            {
                _resourceLoader = _resourceLoader ?? Mvx.Resolve<IMvxResourceLoader>();
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