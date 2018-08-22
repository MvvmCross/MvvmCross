// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.IoC;
using Xunit;

namespace MvvmCross.UnitTest.Base
{
    [Collection("MvxTest")]
    public class MvxIocPropertyInjectionTest
    {
        public interface IA
        {
        }

        public interface IB 
        {
        }

        public interface IC
        {
        }

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

        public class B : IB 
        {
        }

        public class C : IC
        {
        }

        [Fact]
        public void TryResolve_WithNoInjection_NothingGetsInjected()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            Mvx.IoCProvider.RegisterType<IA, A>();
            Mvx.IoCProvider.RegisterType<IB, B>();
            Mvx.IoCProvider.RegisterType<IC, C>();

            IA a;
            var result = Mvx.IoCProvider.TryResolve(out a);
            Assert.True(result);
            Assert.NotNull(a);
            Assert.IsType<A>(a);
            var castA = (A)a;
            Assert.Null(castA.B);
            Assert.Null(castA.C);
            Assert.Null(castA.BNever);
            Assert.Null(castA.CNever);
        }

        [Fact]
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
            var instance = MvxIoCProvider.Initialize(options);

            Mvx.IoCProvider.RegisterType<IA, A>();
            Mvx.IoCProvider.RegisterType<IB, B>();
            Mvx.IoCProvider.RegisterType<IC, C>();

            IA a;
            var result = Mvx.IoCProvider.TryResolve(out a);
            Assert.True(result);
            Assert.NotNull(a);
            Assert.IsType<A>(a);
            var castA = (A)a;
            Assert.NotNull(castA.B);
            Assert.IsType<B>(castA.B);
            Assert.Null(castA.C);
            Assert.Null(castA.BNever);
            Assert.Null(castA.CNever);
        }

        [Fact]
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
            var instance = MvxIoCProvider.Initialize(options);

            Mvx.IoCProvider.RegisterType<IA, A>();
            Mvx.IoCProvider.RegisterType<IB, B>();
            Mvx.IoCProvider.RegisterType<IC, C>();

            IA a;
            var result = Mvx.IoCProvider.TryResolve(out a);
            Assert.True(result);
            Assert.NotNull(a);
            Assert.IsType<A>(a);
            var castA = (A)a;
            Assert.NotNull(castA.B);
            Assert.IsType<B>(castA.B);
            Assert.NotNull(castA.C);
            Assert.IsType<C>(castA.C);
            Assert.Null(castA.BNever);
            Assert.Null(castA.CNever);
        }
    }
}
