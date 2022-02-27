// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Reflection;
using MvvmCross.Exceptions;

namespace MvvmCross.Plugin.JsonLocalization
{
    [Preserve(AllMembers = true)]
    public class MvxEmbeddedJsonDictionaryTextProvider
        : MvxJsonDictionaryTextProvider
    {
        public MvxEmbeddedJsonDictionaryTextProvider(bool maskErrors = true)
            : base(maskErrors)
        {
        }

        public override void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath)
        {
            var json = GetTextFromEmbeddedResource(namespaceKey, resourcePath);
            if (string.IsNullOrEmpty(json))
                throw new FileNotFoundException("Unable to find resource file " + resourcePath);
            LoadJsonFromText(namespaceKey, typeKey, json);
        }

        protected virtual string GetTextFromEmbeddedResource(string namespaceKey, string resourcePath)
        {
            string path = namespaceKey + "." + GenerateResourceNameFromPath(resourcePath);

            try
            {
                var assembly = Assembly.Load(new AssemblyName(namespaceKey));

                using Stream stream = assembly.GetManifestResourceStream(path);
                if (stream == null)
                    return null;

                using var textReader = new StreamReader(stream);
                return textReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex.MvxWrap("Cannot load resource {0}", path);
            }
        }
    }
}
