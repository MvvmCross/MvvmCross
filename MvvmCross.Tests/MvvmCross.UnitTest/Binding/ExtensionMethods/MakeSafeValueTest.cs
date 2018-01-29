// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Test.Core;
using Xunit;

namespace MvvmCross.Binding.Test.ExtensionMethods
{
    
    public class MakeSafeValueTest : MvxIoCSupportingTest
    {
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
            ClearAll();
            MvxBindingSingletonCache.Initialize();
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.Equal(0, typeof(int).MakeSafeValue(0));
            Assert.Equal(0, typeof(int).MakeSafeValue(null));
            Assert.Equal(1, typeof(int).MakeSafeValue(1));
            Assert.Equal(0, typeof(int?).MakeSafeValue(0));
            Assert.Equal(null, typeof(int?).MakeSafeValue(null));
            Assert.Equal(1, typeof(int?).MakeSafeValue(1));
            Assert.Equal(0, typeof(int?).MakeSafeValue(0.0));
            Assert.Equal(1, typeof(int?).MakeSafeValue(1.0));
        }

        [Fact]
        public void TestDoubleValues()
        {
            ClearAll();
            MvxBindingSingletonCache.Initialize();
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.Equal(0.0, typeof(double).MakeSafeValue(0.0));
            Assert.Equal(0.0, typeof(double).MakeSafeValue(null));
            Assert.Equal(1.0, typeof(double).MakeSafeValue(1.0));
            Assert.Equal(0.0, typeof(double?).MakeSafeValue(0.0));
            Assert.Equal(null, typeof(double?).MakeSafeValue(null));
            Assert.Equal(1.0, typeof(double?).MakeSafeValue(1.0));
            Assert.Equal(1.0, typeof(double).MakeSafeValue(1));
            Assert.Equal(0.0, typeof(double?).MakeSafeValue(0));
        }

        [Fact]
        public void TestFloatValues()
        {
            ClearAll();
            MvxBindingSingletonCache.Initialize();
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.Equal(0.0f, typeof(double).MakeSafeValue(0.0f));
            Assert.Equal(0.0f, typeof(double).MakeSafeValue(null));
            Assert.Equal(1.0f, typeof(double).MakeSafeValue(1.0f));
            Assert.Equal(0.0f, typeof(double?).MakeSafeValue(0.0f));
            Assert.Equal(null, typeof(double?).MakeSafeValue(null));
            Assert.Equal(1.0f, typeof(double?).MakeSafeValue(1.0f));
            Assert.Equal(1.0f, typeof(double).MakeSafeValue(1f));
            Assert.Equal(0.0f, typeof(double?).MakeSafeValue(0f));
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
            ClearAll();
            MvxBindingSingletonCache.Initialize();
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.Equal("0", typeof(string).MakeSafeValue(0.0));
            Assert.Equal(null, typeof(string).MakeSafeValue(null));
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
            ClearAll();
            MvxBindingSingletonCache.Initialize();
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.Equal(SampleEnum.Defaulto, typeof(SampleEnum).MakeSafeValue(0));
            Assert.Equal(SampleEnum.Defaulto, typeof(SampleEnum).MakeSafeValue(null));
            Assert.Equal(SampleEnum.Uno, typeof(SampleEnum).MakeSafeValue(1));
            Assert.Equal(SampleEnum.Dos, typeof(SampleEnum).MakeSafeValue("Dos"));
            Assert.Equal(SampleEnum.Dos, typeof(SampleEnum).MakeSafeValue("dOs"));
        }

        [Fact]
        public void TestBoolValues()
        {
            ClearAll();
            MvxBindingSingletonCache.Initialize();
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.Equal(false, typeof(bool).MakeSafeValue(0));
            Assert.Equal(false, typeof(bool).MakeSafeValue(null));
            Assert.Equal(true, typeof(bool).MakeSafeValue(1));
            Assert.Equal(true, typeof(bool).MakeSafeValue(-1.0));
            Assert.Equal(true, typeof(bool).MakeSafeValue(1.0));
            Assert.Equal(true, typeof(bool).MakeSafeValue("Dos"));
            Assert.Equal(true, typeof(bool).MakeSafeValue("dOs"));

            Assert.Equal(false, typeof(bool?).MakeSafeValue(0));
            Assert.Equal(null, typeof(bool?).MakeSafeValue(null));
            Assert.Equal(true, typeof(bool?).MakeSafeValue(1));
            Assert.Equal(true, typeof(bool?).MakeSafeValue(-1.0));
            Assert.Equal(true, typeof(bool?).MakeSafeValue(1.0));
            Assert.Equal(true, typeof(bool?).MakeSafeValue("Dos"));
            Assert.Equal(true, typeof(bool?).MakeSafeValue("dOs"));
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
            ClearAll();
            MvxBindingSingletonCache.Initialize();
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            var foo = new Foo();
            Assert.AreSame(foo, typeof(FooBase).MakeSafeValue(foo));
            var fooBase = new FooBase();
            Assert.AreSame(fooBase, typeof(FooBase).MakeSafeValue(fooBase));
            fooBase = null;
            Assert.IsNull(typeof(FooBase).MakeSafeValue(null));
        }
    }
}