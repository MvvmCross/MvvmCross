namespace MvvmCross.Plugins.JsonLocalization.Tests.Mocks
{
    internal class TestDictionaryTextProvider : MvxDictionaryTextProvider
    {
        public static readonly string LocalizationNamespace = "MvxLocalizationTests";
        public static readonly string TypeKey = "TestDictionaryTextProvider";

        public TestDictionaryTextProvider(bool maskErrors) : base(maskErrors)
        {
        }

        public static TestDictionaryTextProvider CreateAndInitializeWithDummyData(bool maskErrors)
        {
            var textProvider = new TestDictionaryTextProvider(maskErrors);
            textProvider.AddOrReplace(LocalizationNamespace, TypeKey, $"DummyKey", $"DummyValue");

            return textProvider;
        }
    }
}
