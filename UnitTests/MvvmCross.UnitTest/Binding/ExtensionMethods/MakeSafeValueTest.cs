// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.Extensions;
using MvvmCross.Converters;
using MvvmCross.Tests;
using Xunit;

namespace MvvmCross.UnitTest.Binding.ExtensionMethods
{
    [Collection("MvxTest")]
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

        [Fact]
        public void TestIntValues()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.Equal(0, typeof(int).MakeSafeValue(0));
            Assert.Equal(0, typeof(int).MakeSafeValue(null));
            Assert.Equal(1, typeof(int).MakeSafeValue(1));
            Assert.Equal(0, typeof(int?).MakeSafeValue(0));
            Assert.Null(typeof(int?).MakeSafeValue(null));
            Assert.Equal(1, typeof(int?).MakeSafeValue(1));
            Assert.Equal(0, typeof(int?).MakeSafeValue(0.0));
            Assert.Equal(1, typeof(int?).MakeSafeValue(1.0));
        }

        [Fact]
        public void TestDoubleValues()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.Equal(0.0, typeof(double).MakeSafeValue(0.0));
            Assert.Equal(0.0, typeof(double).MakeSafeValue(null));
            Assert.Equal(1.0, typeof(double).MakeSafeValue(1.0));
            Assert.Equal(0.0, typeof(double?).MakeSafeValue(0.0));
            Assert.Null(typeof(double?).MakeSafeValue(null));
            Assert.Equal(1.0, typeof(double?).MakeSafeValue(1.0));
            Assert.Equal(1.0, typeof(double).MakeSafeValue(1));
            Assert.Equal(0.0, typeof(double?).MakeSafeValue(0));
        }

        [Fact]
        public void TestFloatValues()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.Equal(0.0f, typeof(float).MakeSafeValue(0.0f));
            Assert.Equal(0.0f, typeof(float).MakeSafeValue(null));
            Assert.Equal(1.0f, typeof(float).MakeSafeValue(1.0f));
            Assert.Equal(0.0f, typeof(float?).MakeSafeValue(0.0f));
            Assert.Null(typeof(double?).MakeSafeValue(null));
            Assert.Equal(1.0f, typeof(float?).MakeSafeValue(1.0f));
            Assert.Equal(1.0f, typeof(float).MakeSafeValue(1f));
            Assert.Equal(0.0f, typeof(float?).MakeSafeValue(0f));
        }

        public class MyTest
        {
            public override string ToString()
            {
                return "Hello";
            }
        }

        [Fact]
        public void TestStringValues()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.Equal("0", typeof(string).MakeSafeValue(0.0));
            Assert.Null(typeof(string).MakeSafeValue(null));
            Assert.Equal("1", typeof(string).MakeSafeValue(1.0));
            Assert.Equal("Hello", typeof(string).MakeSafeValue("Hello"));
            Assert.Equal("Hello", typeof(string).MakeSafeValue(new MyTest()));
        }

        public enum SampleEnum
        {
            Defaulto,
            Uno,
            Dos
        }

        [Fact]
        public void TestEnumValues()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.Equal(SampleEnum.Defaulto, typeof(SampleEnum).MakeSafeValue(0));
            Assert.Equal(SampleEnum.Defaulto, typeof(SampleEnum).MakeSafeValue(null));
            Assert.Equal(SampleEnum.Uno, typeof(SampleEnum).MakeSafeValue(1));
            Assert.Equal(SampleEnum.Dos, typeof(SampleEnum).MakeSafeValue("Dos"));
            Assert.Equal(SampleEnum.Dos, typeof(SampleEnum).MakeSafeValue("dOs"));
        }

        [Fact]
        public void TestBoolValues()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.False((bool)typeof(bool).MakeSafeValue(0));
            Assert.False((bool)typeof(bool).MakeSafeValue(null));
            Assert.True((bool)typeof(bool).MakeSafeValue(1));
            Assert.True((bool)typeof(bool).MakeSafeValue(-1.0));
            Assert.True((bool)typeof(bool).MakeSafeValue(1.0));
            Assert.True((bool)typeof(bool).MakeSafeValue("Dos"));
            Assert.True((bool)typeof(bool).MakeSafeValue("dOs"));

            Assert.False((bool?)typeof(bool?).MakeSafeValue(0));
            Assert.Null((bool?)typeof(bool?).MakeSafeValue(null));
            Assert.True((bool?)typeof(bool?).MakeSafeValue(1));
            Assert.True((bool?)typeof(bool?).MakeSafeValue(-1.0));
            Assert.True((bool?)typeof(bool?).MakeSafeValue(1.0));
            Assert.True((bool?)typeof(bool?).MakeSafeValue("Dos"));
            Assert.True((bool?)typeof(bool?).MakeSafeValue("dOs"));
        }

        public class FooBase
        {
        }

        public class Foo : FooBase
        {
        }

        [Fact]
        public void TestObjectValues()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            var foo = new Foo();
            Assert.Equal(foo, typeof(FooBase).MakeSafeValue(foo));
            var fooBase = new FooBase();
            Assert.Equal(fooBase, typeof(FooBase).MakeSafeValue(fooBase));
            fooBase = null;
            Assert.Null(typeof(FooBase).MakeSafeValue(null));
        }
    }
}
