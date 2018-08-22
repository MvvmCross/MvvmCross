// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MvvmCross.Core;
using MvvmCross.Tests;
using Xunit;

namespace MvvmCross.UnitTest.Platform
{
    [Collection("MvxTest")]
    public class MvxSimplePropertyDictionaryExtensionMethodsTests
    {
        private readonly NavigationTestFixture _fixture;

        public MvxSimplePropertyDictionaryExtensionMethodsTests(NavigationTestFixture fixture)
        {
            _fixture = fixture;
            fixture.Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());
        }

        [Fact]
        public void Read_ObjectHasValidPropertiesInBaseClass_BaseClassPropertiesAreDeserialized()
        {
            const string value = "42";
            var dictionary = new Dictionary<string, string>
            {
                { "ChildProperty", value },
                { "BasePropertyInternalSet", value },
                { "BasePropertyPublicSet", value }
            };

            var deserialized = dictionary.Read<ObjectWithValidPropertiesInBaseClass>();

            Assert.Equal(value, deserialized.ChildProperty);
            Assert.Equal(value, deserialized.BasePropertyInternalSet);
            Assert.Equal(value, deserialized.BasePropertyPublicSet);
        }

        private class ObjectWithValidPropertiesInBaseClass : ObjectWithValidPropertiesInBaseClassBase
        {
            public string ChildProperty { get; set; }
        }

        private abstract class ObjectWithValidPropertiesInBaseClassBase
        {
            public string BasePropertyInternalSet { get; internal set; }
            public string BasePropertyPublicSet { get; set; }
        }
    }
}
