namespace MvvmCross.Binding.Test.ExtensionMethods
{
    using System;

    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.ExtensionMethods;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Converters;
    using MvvmCross.Test.Core;

    using NUnit.Framework;

    [TestFixture]
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

        [Test]
        public void TestIntValues()
        {
            ClearAll();
            MvxBindingSingletonCache.Initialize();
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.AreEqual(0, typeof(int).MakeSafeValue(0));
            Assert.AreEqual(0, typeof(int).MakeSafeValue(null));
            Assert.AreEqual(1, typeof(int).MakeSafeValue(1));
            Assert.AreEqual(0, typeof(int?).MakeSafeValue(0));
            Assert.AreEqual(null, typeof(int?).MakeSafeValue(null));
            Assert.AreEqual(1, typeof(int?).MakeSafeValue(1));
            Assert.AreEqual(0, typeof(int?).MakeSafeValue(0.0));
            Assert.AreEqual(1, typeof(int?).MakeSafeValue(1.0));
        }

        [Test]
        public void TestDoubleValues()
        {
            ClearAll();
            MvxBindingSingletonCache.Initialize();
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.AreEqual(0.0, typeof(double).MakeSafeValue(0.0));
            Assert.AreEqual(0.0, typeof(double).MakeSafeValue(null));
            Assert.AreEqual(1.0, typeof(double).MakeSafeValue(1.0));
            Assert.AreEqual(0.0, typeof(double?).MakeSafeValue(0.0));
            Assert.AreEqual(null, typeof(double?).MakeSafeValue(null));
            Assert.AreEqual(1.0, typeof(double?).MakeSafeValue(1.0));
            Assert.AreEqual(1.0, typeof(double).MakeSafeValue(1));
            Assert.AreEqual(0.0, typeof(double?).MakeSafeValue(0));
        }

        [Test]
        public void TestFloatValues()
        {
            ClearAll();
            MvxBindingSingletonCache.Initialize();
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.AreEqual(0.0f, typeof(double).MakeSafeValue(0.0f));
            Assert.AreEqual(0.0f, typeof(double).MakeSafeValue(null));
            Assert.AreEqual(1.0f, typeof(double).MakeSafeValue(1.0f));
            Assert.AreEqual(0.0f, typeof(double?).MakeSafeValue(0.0f));
            Assert.AreEqual(null, typeof(double?).MakeSafeValue(null));
            Assert.AreEqual(1.0f, typeof(double?).MakeSafeValue(1.0f));
            Assert.AreEqual(1.0f, typeof(double).MakeSafeValue(1f));
            Assert.AreEqual(0.0f, typeof(double?).MakeSafeValue(0f));
        }

        public class MyTest
        {
            public override string ToString()
            {
                return "Hello";
            }
        }

        [Test]
        public void TestStringValues()
        {
            ClearAll();
            MvxBindingSingletonCache.Initialize();
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.AreEqual("0", typeof(string).MakeSafeValue(0.0));
            Assert.AreEqual(null, typeof(string).MakeSafeValue(null));
            Assert.AreEqual("1", typeof(string).MakeSafeValue(1.0));
            Assert.AreEqual("Hello", typeof(string).MakeSafeValue("Hello"));
            Assert.AreEqual("Hello", typeof(string).MakeSafeValue(new MyTest()));
        }

        public enum SampleEnum
        {
            Defaulto,
            Uno,
            Dos
        }

        [Test]
        public void TestEnumValues()
        {
            ClearAll();
            MvxBindingSingletonCache.Initialize();
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.AreEqual(SampleEnum.Defaulto, typeof(SampleEnum).MakeSafeValue(0));
            Assert.AreEqual(SampleEnum.Defaulto, typeof(SampleEnum).MakeSafeValue(null));
            Assert.AreEqual(SampleEnum.Uno, typeof(SampleEnum).MakeSafeValue(1));
            Assert.AreEqual(SampleEnum.Dos, typeof(SampleEnum).MakeSafeValue("Dos"));
            Assert.AreEqual(SampleEnum.Dos, typeof(SampleEnum).MakeSafeValue("dOs"));
        }

        [Test]
        public void TestBoolValues()
        {
            ClearAll();
            MvxBindingSingletonCache.Initialize();
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(new MockAutoValueConverters());

            Assert.AreEqual(false, typeof(bool).MakeSafeValue(0));
            Assert.AreEqual(false, typeof(bool).MakeSafeValue(null));
            Assert.AreEqual(true, typeof(bool).MakeSafeValue(1));
            Assert.AreEqual(true, typeof(bool).MakeSafeValue(-1.0));
            Assert.AreEqual(true, typeof(bool).MakeSafeValue(1.0));
            Assert.AreEqual(true, typeof(bool).MakeSafeValue("Dos"));
            Assert.AreEqual(true, typeof(bool).MakeSafeValue("dOs"));

            Assert.AreEqual(false, typeof(bool?).MakeSafeValue(0));
            Assert.AreEqual(null, typeof(bool?).MakeSafeValue(null));
            Assert.AreEqual(true, typeof(bool?).MakeSafeValue(1));
            Assert.AreEqual(true, typeof(bool?).MakeSafeValue(-1.0));
            Assert.AreEqual(true, typeof(bool?).MakeSafeValue(1.0));
            Assert.AreEqual(true, typeof(bool?).MakeSafeValue("Dos"));
            Assert.AreEqual(true, typeof(bool?).MakeSafeValue("dOs"));
        }

        public class FooBase
        {
        }

        public class Foo : FooBase
        {
        }

        [Test]
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