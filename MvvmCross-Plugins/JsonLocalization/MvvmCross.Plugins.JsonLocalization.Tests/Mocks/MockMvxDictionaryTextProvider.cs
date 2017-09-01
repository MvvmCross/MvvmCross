namespace MvvmCross.Plugins.JsonLocalization.Tests.Mocks
{
    internal class MockMvxDictionaryTextProvider : MvxDictionaryTextProvider
    {
        public static readonly string LocalizationNamespace = "MvxLocalizationTests";
        public static readonly string TypeKey = "MockMvxDictionaryTextProvider";

        public MockMvxDictionaryTextProvider(bool maskErrors) : base(maskErrors)
        {
        }

        public static MockMvxDictionaryTextProvider CreateAndInitializeWithDummyData(bool maskErrors)
        {
            var textProvider = new MockMvxDictionaryTextProvider(maskErrors);
            textProvider.AddOrReplace(LocalizationNamespace, TypeKey, $"DummyKey", $"DummyValue");

            return textProvider;
        }
    }
}
