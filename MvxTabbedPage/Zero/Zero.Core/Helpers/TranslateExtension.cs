// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using Zero.Core.Resources;
using Zero.Core.Services;

namespace Zero.Core.Helpers
{
    public static class TranslateExtension
    {
        public static string Translate(this ILocalizeService localizeService, string str)
        {
            var tranlation = AppResources.ResourceManager.GetString(str, localizeService.GetCurrentCultureInfo());
            return string.IsNullOrEmpty(tranlation) ? str : tranlation;
        }
    }
}
