using System.Globalization;
using System.Linq;
using System.Resources;

namespace MvvmCross.Plugins.ResxLocalization.Tests.Mocks
{
    internal class MockResourceManager : ResourceManager
    {
        public static readonly string LocalizationNamespace = "DummyLocalizationNamespace";
        public static readonly string TypeKey = "DummyTypeKey";
        public static readonly string DummyName = "DummyName";

        private static readonly string[] ValidKeys = { $"{LocalizationNamespace}.{TypeKey}.{DummyName}", $"{LocalizationNamespace}.{DummyName}", $"{TypeKey}.{DummyName}", DummyName};

        public override string GetString(string name) => ValidKeys.SingleOrDefault(key => key.Equals(name));

        public override string GetString(string name, CultureInfo culture) => GetString(name);
    }
}
