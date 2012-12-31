#region Copyright

// <copyright file="MvxResourceJsonDictionaryTextProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.IO;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Plugins.ResourceLoader;

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