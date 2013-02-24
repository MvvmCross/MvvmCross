// MvxResourceJsonDictionaryTextProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.IO;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform;

namespace Cirrious.MvvmCross.Plugins.JsonLocalisation
{
    public class MvxResourceJsonDictionaryTextProvider
        : MvxJsonDictionaryTextProvider
    {
        public MvxResourceJsonDictionaryTextProvider(bool maskErrors)
            : base(maskErrors)
        {
        }

        #region IMvxJsonDictionaryTextLoader Members

        public override void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath)
        {
            var service = this.GetService<IMvxResourceLoader>();
            var json = service.GetTextResource(resourcePath);
            if (string.IsNullOrEmpty(json))
                throw new FileNotFoundException("Unable to find resource file " + resourcePath);
            LoadJsonFromText(namespaceKey, typeKey, json);
        }

        #endregion
    }
}