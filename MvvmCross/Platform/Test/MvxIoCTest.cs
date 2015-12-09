// MvxIocTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Test
{
    using System.Collections.Generic;

    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.IoC;

    using NUnit.Framework;

    [TestFixture]
    public class MvxIocTest
    {
        public interface IA { IB B { get; } }

        public interface IB { }

        public interface IC { }

        public class A : IA
        {
            public A(IB b)
            {
                this.B = b;
            }

            public IB B { get; set; }
        }

        public class B : IB

        {
            public B(IC c)
            {
            }
        }

        public class C : IC

        {
            public C(IA a)
            {
            }
        }

        public class C2 : IC

        {
            public C2()
            {
            }
        }

        public class COdd : IC
        {
            public static bool FirstTime = true;

            public COdd()
            {
                if (FirstTime)
                {
                    FirstTime = false;
                    var a = Mvx.Resolve<IA>();
                }
            }
        }

        [Test]
        public void TryResolve_CircularButSafeDynamicWithOptionOff_ReturnsTrue()
        {
            COdd.FirstTime = true;
            MvxSingleton.ClearAllSingletons();
            var options = new MvxIocOptions()
            {
                TryToDetectDynamicCircularReferences = false
            };
            var instance = MvxSimpleIoCContainer.Initialize(options);

            Mvx.RegisterType<IA, A>();
            Mvx.RegisterType<IB, B>();
            Mvx.RegisterType<IC, COdd>();

            IA a;
            var result = Mvx.TryResolve(out a);
            Assert.IsTrue(result);
            Assert.IsNotNull(a);
        }

        [Test]
        public void TryResolve_CircularButSafeDynamicWithOptionOn_ReturnsFalse()
        {
            COdd.FirstTime = true;
            MvxSingleton.ClearAllSingletons();
            var instance = MvxSimpleIoCContainer.Initialize();

            Mvx.RegisterType<IA, A>();
            Mvx.RegisterType<IB, B>();
            Mvx.RegisterType<IC, COdd>();

            IA a;
            var result = Mvx.TryResolve(out a);
            Assert.IsFalse(result);
            Assert.IsNull(a);
        }

        [Test]
        public void TryResolve_CircularLazyRegistration_ReturnsFalse()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxSimpleIoCContainer.Initialize();

            Mvx.LazyConstructAndRegisterSingleton<IA, A>();
            Mvx.LazyConstructAndRegisterSingleton<IB, B>();
            Mvx.LazyConstructAndRegisterSingleton<IC, C>();

            IA a;
            var result = Mvx.TryResolve(out a);
            Assert.IsFalse(result);
            Assert.IsNull(a);
        }

        [Test]
        public void TryResolve_NonCircularRegistration_ReturnsTrue()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxSimpleIoCContainer.Initialize();

            Mvx.LazyConstructAndRegisterSingleton<IA, A>();
            Mvx.LazyConstructAndRegisterSingleton<IB, B>();
            Mvx.LazyConstructAndRegisterSingleton<IC, C2>();

            IA a;
            var result = Mvx.TryResolve(out a);
            Assert.IsTrue(result);
            Assert.IsNotNull(a);
        }

        [Test]
        public void TryResolve_LazySingleton_ReturnsSameSingletonEachTime()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxSimpleIoCContainer.Initialize();

            Mvx.LazyConstructAndRegisterSingleton<IA, A>();
            Mvx.LazyConstructAndRegisterSingleton<IB, B>();
            Mvx.LazyConstructAndRegisterSingleton<IC, C2>();

            IA a0;
            var result = Mvx.TryResolve(out a0);
            Assert.IsTrue(result);
            Assert.IsNotNull(a0);

            for (int i = 0; i < 100; i++)
            {
                IA a1;
                result = Mvx.TryResolve(out a1);
                Assert.IsTrue(result);
                Assert.AreSame(a0, a1);
            }
        }

        [Test]
        public void TryResolve_NonLazySingleton_ReturnsSameSingletonEachTime()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxSimpleIoCContainer.Initialize();

            Mvx.LazyConstructAndRegisterSingleton<IB, B>();
            Mvx.LazyConstructAndRegisterSingleton<IC, C2>();
            Mvx.ConstructAndRegisterSingleton<IA, A>();

            IA a0;
            var result = Mvx.TryResolve(out a0);
            Assert.IsTrue(result);
            Assert.IsNotNull(a0);

            for (int i = 0; i < 100; i++)
            {
                IA a1;
                result = Mvx.TryResolve(out a1);
                Assert.IsTrue(result);
                Assert.AreSame(a0, a1);
            }
        }

        [Test]
        public void TryResolve_Dynamic_ReturnsDifferentInstanceEachTime()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxSimpleIoCContainer.Initialize();

            Mvx.LazyConstructAndRegisterSingleton<IB, B>();
            Mvx.LazyConstructAndRegisterSingleton<IC, C2>();
            Mvx.RegisterType<IA, A>();

            var previous = new Dictionary<IA, bool>();

            for (int i = 0; i < 100; i++)
            {
                IA a1;
                var result = Mvx.TryResolve(out a1);
                Assert.IsTrue(result);
                Assert.IsFalse(previous.ContainsKey(a1));
                Assert.AreEqual(i, previous.Count);
                previous.Add(a1, true);
            }
        }

        [Test]
        public void TryResolve_ParameterConstructors_CreatesParametersUsingIocResolution()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxSimpleIoCContainer.Initialize();

            Mvx.RegisterType<IB, B>();
            Mvx.LazyConstructAndRegisterSingleton<IC, C2>();
            Mvx.RegisterType<IA, A>();

            IA a1;
            var result = Mvx.TryResolve(out a1);
            Assert.IsTrue(result);
            Assert.IsNotNull(a1);
            Assert.IsNotNull(a1.B);
            Assert.IsInstanceOf<B>(a1.B);
        }

        [Test]
        public void RegisterType_with_constructor_creates_different_objects()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxSimpleIoCContainer.Initialize();

            instance.RegisterType<IC>(() => new C2());

            var c1 = Mvx.Resolve<IC>();
            var c2 = Mvx.Resolve<IC>();

            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);

            Assert.AreNotEqual(c1, c2);
        }

        [Test]
        public void Non_generic_RegisterType_with_constructor_creates_different_objects()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxSimpleIoCContainer.Initialize();

            instance.RegisterType(typeof(IC), () => new C2());

            var c1 = Mvx.Resolve<IC>();
            var c2 = Mvx.Resolve<IC>();

            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);

            Assert.AreNotEqual(c1, c2);
        }

        [Test]
        [ExpectedException(typeof(MvxIoCResolveException))]
        public void Non_generic_RegisterType_with_constructor_throws_if_constructor_returns_incompatible_reference()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxSimpleIoCContainer.Initialize();

            instance.RegisterType(typeof(IC), () => "Fail");

            var c1 = Mvx.Resolve<IC>();
        }

        [Test]
        [ExpectedException(typeof(MvxIoCResolveException))]
        public void Non_generic_RegisterType_with_constructor_throws_if_constructor_returns_incompatible_value()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxSimpleIoCContainer.Initialize();

            instance.RegisterType(typeof(IC), () => 36);

            var c1 = Mvx.Resolve<IC>();
        }

        // TODO - there are so many tests we could and should do here!
    }
}