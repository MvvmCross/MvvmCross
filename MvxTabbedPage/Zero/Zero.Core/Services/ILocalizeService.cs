// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using System.Globalization;

namespace Zero.Core.Services
{
    public interface ILocalizeService
    {
        CultureInfo GetCurrentCultureInfo();
    }
}
