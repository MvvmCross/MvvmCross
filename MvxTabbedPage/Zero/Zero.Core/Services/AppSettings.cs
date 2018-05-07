// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using Plugin.Settings.Abstractions;

namespace Zero.Core.Services
{
    public class AppSettings : IAppSettings
    {
        public const string SuperNumberKey = "SuperNumberKey";

        public bool IsLogin = false;

        public const int SuperNumberDefaultValue = 1;

        private readonly ISettings _settings;

        public AppSettings(ISettings settings)
        {
            _settings = settings;
        }

        public int SuperNumber
        {
            get { return _settings.GetValueOrDefault(SuperNumberKey, 1); }
            set { _settings.AddOrUpdateValue(SuperNumberKey, value); }
        }
    }
}
