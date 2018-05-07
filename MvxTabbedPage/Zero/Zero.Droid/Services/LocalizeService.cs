// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using System.Diagnostics;
using System.Globalization;

namespace Zero.Droid.Services
{
    public class LocalizeService : Core.Services.ILocalizeService
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netLanguage = androidLocale.ToString().Replace("_", "-"); // turns pt_BR into pt-BR
            try
            {
                return new CultureInfo(netLanguage);
            }
            catch (CultureNotFoundException e)
            {
                Debug.WriteLine(e.Message);
            }

            return new CultureInfo("en");
        }
    }
}