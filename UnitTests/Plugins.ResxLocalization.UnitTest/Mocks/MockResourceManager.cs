// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Linq;
using System.Resources;

namespace MvvmCross.Plugin.ResxLocalization.UnitTest.Mocks
{
    internal class MockResourceManager : ResourceManager
    {
        public static readonly string LocalizationNamespace = "DummyLocalizationNamespace";
        public static readonly string TypeKey = "DummyTypeKey";
        public static readonly string DummyName = "DummyName";

        private static readonly string[] ValidKeys = { $"{LocalizationNamespace}.{TypeKey}.{DummyName}", $"{LocalizationNamespace}.{DummyName}", $"{TypeKey}.{DummyName}", DummyName };

        public override string GetString(string name) => ValidKeys.SingleOrDefault(key => key.Equals(name));

        public override string GetString(string name, CultureInfo culture) => GetString(name);
    }
}
