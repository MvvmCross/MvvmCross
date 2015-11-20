// MvxIocPropertyInjectionTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.IoC;
using NUnit.Framework;

namespace Cirrious.CrossCore.Test
{
    [TestFixture]
    public class MvxIocPropertyInjectionTest
    {
        public interface IA { }

        public interface IB { }

        public interface IC { }

        public class A : IA
        {
            public A()
            {
            }

            [MvxInject]
            public IB B { get; set; }

            public IC C { get; set; }

            public B BNever { get; set; }

            [MvxInject]
            public C CNever { get; set; }
        }

        public class B : IB { }

        public class C : IC { }

        [Test]
        public void TryResolve_WithNoInjection_NothingGetsInjected()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxSimpleIoCContainer.Initialize();

            Mvx.RegisterType<IA, A>();
            Mvx.RegisterType<IB, B>();
            Mvx.RegisterType<IC, C>();

            IA a;
            var result = Mvx.TryResolve(out a);
            Assert.IsTrue(result);
            Assert.IsNotNull(a);
            Assert.IsInstanceOf<A>(a);
            var castA = (A)a;
            Assert.IsNull(castA.B);
            Assert.IsNull(castA.C);
            Assert.IsNull(castA.BNever);
            Assert.IsNull(castA.CNever);
        }

        [Test]
        public void TryResolve_WithAttrInjection_AttrMarkedProperiesGetInjected()
        {
            MvxSingleton.ClearAllSingletons();
            var options = new MvxIocOptions
            {
                PropertyInjectorOptions = new MvxPropertyInjectorOptions()
                {
                    InjectIntoProperties = MvxPropertyInjection.MvxInjectInterfaceProperties
                }
            };
            var instance = MvxSimpleIoCContainer.Initialize(options);

            Mvx.RegisterType<IA, A>();
            Mvx.RegisterType<IB, B>();
            Mvx.RegisterType<IC, C>();

            IA a;
            var result = Mvx.TryResolve(out a);
            Assert.IsTrue(result);
            Assert.IsNotNull(a);
            Assert.IsInstanceOf<A>(a);
            var castA = (A)a;
            Assert.IsNotNull(castA.B);
            Assert.IsInstanceOf<B>(castA.B);
            Assert.IsNull(castA.C);
            Assert.IsNull(castA.BNever);
            Assert.IsNull(castA.CNever);
        }

        [Test]
        public void TryResolve_WithFullInjection_AllInterfaceProperiesGetInjected()
        {
            MvxSingleton.ClearAllSingletons();
            var options = new MvxIocOptions
            {
                PropertyInjectorOptions = new MvxPropertyInjectorOptions()
                {
                    InjectIntoProperties = MvxPropertyInjection.AllInterfaceProperties
                }
            };
            var instance = MvxSimpleIoCContainer.Initialize(options);

            Mvx.RegisterType<IA, A>();
            Mvx.RegisterType<IB, B>();
            Mvx.RegisterType<IC, C>();

            IA a;
            var result = Mvx.TryResolve(out a);
            Assert.IsTrue(result);
            Assert.IsNotNull(a);
            Assert.IsInstanceOf<A>(a);
            var castA = (A)a;
            Assert.IsNotNull(castA.B);
            Assert.IsInstanceOf<B>(castA.B);
            Assert.IsNotNull(castA.C);
            Assert.IsInstanceOf<C>(castA.C);
            Assert.IsNull(castA.BNever);
            Assert.IsNull(castA.CNever);
        }
    }
}