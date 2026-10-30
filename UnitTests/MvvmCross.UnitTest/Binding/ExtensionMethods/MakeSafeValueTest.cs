// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.Extensions;
using MvvmCross.Converters;
using MvvmCross.Tests;

namespace MvvmCross.UnitTest.Binding.ExtensionMethods
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
    public class MakeSafeValueTest
    {
        private readonly NavigationTestFixture _fixture;

        public MakeSafeValueTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        public class MockAutoValueConverters : IMvxAutoValueConverters
        {
            public IMvxValueConverter Find(Type viewModelType, Type viewType)
            {
                return null;
            }

            public void Register(Type viewModelType, Type viewType, IMvxValueConverter converter)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public async Task TestIntValues()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            await Assert.That(typeof(int).MakeSafeValue(0)).IsEqualTo(0);
            await Assert.That(typeof(int).MakeSafeValue(null)).IsEqualTo(0);
            await Assert.That(typeof(int).MakeSafeValue(1)).IsEqualTo(1);
            await Assert.That(typeof(int?).MakeSafeValue(0)).IsEqualTo(0);
            await Assert.That(typeof(int?).MakeSafeValue(null)).IsNull();
            await Assert.That(typeof(int?).MakeSafeValue(1)).IsEqualTo(1);
            await Assert.That(typeof(int?).MakeSafeValue(0.0)).IsEqualTo(0);
            await Assert.That(typeof(int?).MakeSafeValue(1.0)).IsEqualTo(1);
        }

        [Test]
        public async Task TestDoubleValues()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            await Assert.That(typeof(double).MakeSafeValue(0.0)).IsEqualTo(0.0);
            await Assert.That(typeof(double).MakeSafeValue(null)).IsEqualTo(0.0);
            await Assert.That(typeof(double).MakeSafeValue(1.0)).IsEqualTo(1.0);
            await Assert.That(typeof(double?).MakeSafeValue(0.0)).IsEqualTo(0.0);
            await Assert.That(typeof(double?).MakeSafeValue(null)).IsNull();
            await Assert.That(typeof(double?).MakeSafeValue(1.0)).IsEqualTo(1.0);
            await Assert.That(typeof(double).MakeSafeValue(1)).IsEqualTo(1.0);
            await Assert.That(typeof(double?).MakeSafeValue(0)).IsEqualTo(0.0);
        }

        [Test]
        public async Task TestFloatValues()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            await Assert.That(typeof(float).MakeSafeValue(0.0f)).IsEqualTo(0.0f);
            await Assert.That(typeof(float).MakeSafeValue(null)).IsEqualTo(0.0f);
            await Assert.That(typeof(float).MakeSafeValue(1.0f)).IsEqualTo(1.0f);
            await Assert.That(typeof(float?).MakeSafeValue(0.0f)).IsEqualTo(0.0f);
            await Assert.That(typeof(double?).MakeSafeValue(null)).IsNull();
            await Assert.That(typeof(float?).MakeSafeValue(1.0f)).IsEqualTo(1.0f);
            await Assert.That(typeof(float).MakeSafeValue(1f)).IsEqualTo(1.0f);
            await Assert.That(typeof(float?).MakeSafeValue(0f)).IsEqualTo(0.0f);
        }

        public class MyTest
        {
            public override string ToString()
            {
                return "Hello";
            }
        }

        [Test]
        public async Task TestStringValues()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            await Assert.That(typeof(string).MakeSafeValue(0.0)).IsEqualTo("0");
            await Assert.That(typeof(string).MakeSafeValue(null)).IsNull();
            await Assert.That(typeof(string).MakeSafeValue(1.0)).IsEqualTo("1");
            await Assert.That(typeof(string).MakeSafeValue("Hello")).IsEqualTo("Hello");
            await Assert.That(typeof(string).MakeSafeValue(new MyTest())).IsEqualTo("Hello");
        }

        public enum SampleEnum
        {
            Defaulto,
            Uno,
            Dos
        }

        [Test]
        public async Task TestEnumValues()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            await Assert.That(typeof(SampleEnum).MakeSafeValue(0)).IsEqualTo(SampleEnum.Defaulto);
            await Assert.That(typeof(SampleEnum).MakeSafeValue(null)).IsEqualTo(SampleEnum.Defaulto);
            await Assert.That(typeof(SampleEnum).MakeSafeValue(1)).IsEqualTo(SampleEnum.Uno);
            await Assert.That(typeof(SampleEnum).MakeSafeValue("Dos")).IsEqualTo(SampleEnum.Dos);
            await Assert.That(typeof(SampleEnum).MakeSafeValue("dOs")).IsEqualTo(SampleEnum.Dos);
        }

        [Test]
        public async Task TestBoolValues()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            await Assert.That((bool)typeof(bool).MakeSafeValue(0)).IsFalse();
            await Assert.That((bool)typeof(bool).MakeSafeValue(null)).IsFalse();
            await Assert.That((bool)typeof(bool).MakeSafeValue(1)).IsTrue();
            await Assert.That((bool)typeof(bool).MakeSafeValue(-1.0)).IsTrue();
            await Assert.That((bool)typeof(bool).MakeSafeValue(1.0)).IsTrue();
            await Assert.That((bool)typeof(bool).MakeSafeValue("Dos")).IsTrue();
            await Assert.That((bool)typeof(bool).MakeSafeValue("dOs")).IsTrue();

            await Assert.That((bool?)typeof(bool?).MakeSafeValue(0)).IsFalse();
            await Assert.That((bool?)typeof(bool?).MakeSafeValue(null)).IsNull();
            await Assert.That((bool?)typeof(bool?).MakeSafeValue(1)).IsTrue();
            await Assert.That((bool?)typeof(bool?).MakeSafeValue(-1.0)).IsTrue();
            await Assert.That((bool?)typeof(bool?).MakeSafeValue(1.0)).IsTrue();
            await Assert.That((bool?)typeof(bool?).MakeSafeValue("Dos")).IsTrue();
            await Assert.That((bool?)typeof(bool?).MakeSafeValue("dOs")).IsTrue();
        }

        public class FooBase
        {
        }

        public class Foo : FooBase
        {
        }

        [Test]
        public async Task TestObjectValues()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            var foo = new Foo();
            await Assert.That(typeof(FooBase).MakeSafeValue(foo)).IsEqualTo(foo);
            var fooBase = new FooBase();
            await Assert.That(typeof(FooBase).MakeSafeValue(fooBase)).IsEqualTo(fooBase);
            fooBase = null;
            await Assert.That(typeof(FooBase).MakeSafeValue(null)).IsNull();
        }
    }
}
