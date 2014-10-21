using System.Collections.Generic;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Test.Core;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Test.Platform
{
    [TestFixture]
    public class MvxSimplePropertyDictionaryExtensionMethodsTests : MvxIoCSupportingTest
    {
        [SetUp]
        public void SetUp()
        {
            ClearAll();
            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());
        }

        [Test]
        public void Read_ObjectHasValidPropertiesInBaseClass_BaseClassPropertiesAreDeserialized()
        {
            const string value = "42";
            var dictionary = new Dictionary<string, string>
            {
                {"ChildProperty", value},
                {"BasePropertyInternalSet", value},
                {"BasePropertyPublicSet", value}
            };

            var deserialized = dictionary.Read<ObjectWithValidPropertiesInBaseClass>();

            Assert.AreEqual(value, deserialized.ChildProperty);
            Assert.AreEqual(value, deserialized.BasePropertyInternalSet);
            Assert.AreEqual(value, deserialized.BasePropertyPublicSet);
        }

        class ObjectWithValidPropertiesInBaseClass : ObjectWithValidPropertiesInBaseClassBase
        {
            public string ChildProperty { get; set; }
        }

        abstract class ObjectWithValidPropertiesInBaseClassBase
        {
            public string BasePropertyInternalSet { get; internal set; }
            public string BasePropertyPublicSet { get; set; }
        }
    }
}