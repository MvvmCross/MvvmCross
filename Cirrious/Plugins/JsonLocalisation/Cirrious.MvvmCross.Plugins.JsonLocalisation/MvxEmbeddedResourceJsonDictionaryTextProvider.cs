using System;
using System.IO;
using System.Reflection;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Plugins.ResourceLoader;

namespace Cirrious.MvvmCross.Plugins.JsonLocalisation
{
#warning  MvxEmbeddedResourceJsonDictionaryTextProvider not yet implemented
#if false
    public class MvxEmbeddedResourceJsonDictionaryTextProvider
        : MvxJsonDictionaryTextProvider
    {
        public MvxEmbeddedResourceJsonDictionaryTextProvider(bool maskErrors)
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
            GetTextFromEmbeddedResource(namespaceKey, typeKey, json);
        }

        #endregion

        private string GetTextFromEmbeddedResource(string namespaceKey, string resourcePath)
        {
            string path = namespaceKey + "." + resourcePath.Replace("/", ".");
            try
            {
                string text = null;
                // TODO
                Stream stream = Assembly.Load(namespaceKey).GetManifestResourceStream(path);
                if (stream == null)
                    return null;

                using (var textReader = new StreamReader(stream))
                {
                    text = textReader.ReadToEnd();
                }

                return text;
            }
            catch (Exception ex)
            {
                throw ex.MvxWrap("Cannot load resource {0}", path);
            }
        }
    }
#endif
}