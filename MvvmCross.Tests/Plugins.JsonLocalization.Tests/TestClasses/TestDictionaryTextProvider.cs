// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

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
