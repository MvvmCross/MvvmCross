// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Base;
using Xunit;
using MvvmCross.Tests;
using System.Linq;
using MvvmCross.Exceptions;
using MvvmCross.IoC;

namespace MvvmCross.UnitTest.Base
{
    [Collection("MvxTest")]   
    public class MvxIocTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxIocTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearAll();
        }

        public interface IA 
        { 
            IB B { get; } 
        }

        public interface IB
        {
            IC C { get; }
        }

        public interface IC
        {
        }

        public interface IOG<T>
        {
        }

        public interface IOG2<T, T2>
        {
        }

        public interface IHasOGParameter
        {
            IOG<C> OpenGeneric { get; }
        }

        public class A : IA
        {
            public A(IB b)
            {
                B = b;
            }

            public IB B { get; set; }
        }

        public class B : IB
        {
            public B(IC c)
            {
                C = c;
            }

            public IC C { get; set; }
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

        public class D
        {
            public string Title { get; }
            public string Subtitle { get; }
            public string Description { get; }
            public int Amount { get; }
            public bool Enabled { get; }

            public D(string title, string subtitle, string description)
            {
                Title = title;
                Subtitle = subtitle;
                Description = description;
            }

            public D(string title, int amount, bool enabled)
            {
                Title = title;
                Amount = amount;
                Enabled = enabled;
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

        public class OG<T> : IOG<T>
        {
        }

        public class OG2<T, T2> : IOG2<T, T2>
        {
        }

        public class HasOGParameter : IHasOGParameter
        {
            public HasOGParameter(IOG<C> openGeneric)
            {
                this.OpenGeneric = openGeneric;
            }

            public IOG<C> OpenGeneric { get; }
        }

        [Fact]
        public void TryResolve_CircularButSafeDynamicWithOptionOff_ReturnsTrue()
        {
            COdd.FirstTime = true;
            _fixture.ClearAll(new MvxIocOptions()
            {
                TryToDetectDynamicCircularReferences = false
            });

            _fixture.Ioc.RegisterType<IA, A>();
            _fixture.Ioc.RegisterType<IB, B>();
            _fixture.Ioc.RegisterType<IC, COdd>();

            var result = _fixture.Ioc.TryResolve(out IA a);
            Assert.True(result);
            Assert.NotNull(a);
        }

        [Fact]
        public void TryResolve_CircularButSafeDynamicWithOptionOn_ReturnsFalse()
        {
            COdd.FirstTime = true;
            _fixture.ClearAll();

            _fixture.Ioc.RegisterType<IA, A>();
            _fixture.Ioc.RegisterType<IB, B>();
            _fixture.Ioc.RegisterType<IC, COdd>();

            var result = _fixture.Ioc.TryResolve(out IA a);
            Assert.False(result);
            Assert.Null(a);
        }

        [Fact]
        public void TryResolve_CircularLazyRegistration_ReturnsFalse()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            Mvx.LazyConstructAndRegisterSingleton<IA, A>();
            Mvx.LazyConstructAndRegisterSingleton<IB, B>();
            Mvx.LazyConstructAndRegisterSingleton<IC, C>();

            IA a;
            var result = Mvx.TryResolve(out a);
            Assert.False(result);
            Assert.Null(a);
        }

        [Fact]
        public void TryResolve_NonCircularRegistration_ReturnsTrue()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            Mvx.LazyConstructAndRegisterSingleton<IA, A>();
            Mvx.LazyConstructAndRegisterSingleton<IB, B>();
            Mvx.LazyConstructAndRegisterSingleton<IC, C2>();

            IA a;
            var result = Mvx.TryResolve(out a);
            Assert.True(result);
            Assert.NotNull(a);
        }

        [Fact]
        public void TryResolve_LazySingleton_ReturnsSameSingletonEachTime()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            Mvx.LazyConstructAndRegisterSingleton<IA, A>();
            Mvx.LazyConstructAndRegisterSingleton<IB, B>();
            Mvx.LazyConstructAndRegisterSingleton<IC, C2>();

            IA a0;
            var result = Mvx.TryResolve(out a0);
            Assert.True(result);
            Assert.NotNull(a0);

            for (int i = 0; i < 100; i++)
            {
                IA a1;
                result = Mvx.TryResolve(out a1);
                Assert.True(result);
                Assert.Equal(a0, a1);
            }
        }

        [Fact]
        public void TryResolve_NonLazySingleton_ReturnsSameSingletonEachTime()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            Mvx.LazyConstructAndRegisterSingleton<IB, B>();
            Mvx.LazyConstructAndRegisterSingleton<IC, C2>();
            Mvx.ConstructAndRegisterSingleton<IA, A>();

            IA a0;
            var result = Mvx.TryResolve(out a0);
            Assert.True(result);
            Assert.NotNull(a0);

            for (int i = 0; i < 100; i++)
            {
                IA a1;
                result = Mvx.TryResolve(out a1);
                Assert.True(result);
                Assert.Equal(a0, a1);
            }
        }

        [Fact]
        public void TryResolve_Dynamic_ReturnsDifferentInstanceEachTime()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            Mvx.LazyConstructAndRegisterSingleton<IB, B>();
            Mvx.LazyConstructAndRegisterSingleton<IC, C2>();
            Mvx.RegisterType<IA, A>();

            var previous = new Dictionary<IA, bool>();

            for (int i = 0; i < 100; i++)
            {
                IA a1;
                var result = Mvx.TryResolve(out a1);
                Assert.True(result);
                Assert.False(previous.ContainsKey(a1));
                Assert.Equal(i, previous.Count);
                previous.Add(a1, true);
            }
        }

        [Fact]
        public void TryResolve_ParameterConstructors_CreatesParametersUsingIocResolution()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            Mvx.RegisterType<IB, B>();
            Mvx.LazyConstructAndRegisterSingleton<IC, C2>();
            Mvx.RegisterType<IA, A>();

            IA a1;
            var result = Mvx.TryResolve(out a1);
            Assert.True(result);
            Assert.NotNull(a1);
            Assert.NotNull(a1.B);
            Assert.IsType<B>(a1.B);
        }

        [Fact]
        public void RegisterType_with_constructor_creates_different_objects()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            instance.RegisterType<IC>(() => new C2());

            var c1 = Mvx.Resolve<IC>();
            var c2 = Mvx.Resolve<IC>();

            Assert.NotNull(c1);
            Assert.NotNull(c2);

            Assert.NotEqual(c1, c2);
        }

        [Fact]
        public void Non_generic_RegisterType_with_constructor_creates_different_objects()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            instance.RegisterType(typeof(IC), () => new C2());

            var c1 = Mvx.Resolve<IC>();
            var c2 = Mvx.Resolve<IC>();

            Assert.NotNull(c1);
            Assert.NotNull(c2);

            Assert.NotEqual(c1, c2);
        }

        [Fact]
        public void Non_generic_RegisterType_with_constructor_throws_if_constructor_returns_incompatible_reference()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            instance.RegisterType(typeof(IC), () => "Fail");

            Assert.Throws<MvxIoCResolveException>(() => {
                var c1 = Mvx.Resolve<IC>();
            });
        }

        [Fact]
        public void Non_generic_RegisterType_with_constructor_throws_if_constructor_returns_incompatible_value()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            instance.RegisterType(typeof(IC), () => 36);

            Assert.Throws<MvxIoCResolveException>(() => {
                var c1 = Mvx.Resolve<IC>();
            });
        }

        #region Open-Generics

        [Fact]
        public static void Resolves_successfully_when_registered_open_generic_with_one_generic_type_parameter()
        {
            var instance = MvxIoCProvider.Initialize();
            ((MvxIoCProvider)instance).CleanAllResolvers();

            instance.RegisterType(typeof(IOG<>), typeof(OG<>));

            IOG<C2> toResolve = null;
            Mvx.TryResolve<IOG<C2>>(out toResolve);

            Assert.NotNull(toResolve);
            Assert.Contains(toResolve.GetType().GetTypeInfo().ImplementedInterfaces, i => i == typeof(IOG<C2>));
            Assert.True(toResolve.GetType() == typeof(OG<C2>));
        }

        [Fact]
        public static void Resolves_successfully_when_registered_closed_generic_with_one_generic_type_parameter()
        {
            var instance = MvxIoCProvider.Initialize();
            ((MvxIoCProvider)instance).CleanAllResolvers();

            instance.RegisterType(typeof(IOG<C2>), typeof(OG<C2>));
            
            IOG<C2> toResolve = null;
            Mvx.TryResolve<IOG<C2>>(out toResolve);

            Assert.NotNull(toResolve);
            Assert.Contains(toResolve.GetType().GetTypeInfo().ImplementedInterfaces, i => i == typeof(IOG<C2>));
            Assert.True(toResolve.GetType() == typeof(OG<C2>));
        }

        [Fact]
        public static void Resolves_successfully_when_registered_open_generic_with_two_generic_type_parameter()
        {
            var instance = MvxIoCProvider.Initialize();
            ((MvxIoCProvider)instance).CleanAllResolvers();

            instance.RegisterType(typeof(IOG2<,>), typeof(OG2<,>));

            IOG2<C2, C> toResolve = null;
            Mvx.TryResolve<IOG2<C2,C>>(out toResolve);

            Assert.NotNull(toResolve);
            Assert.Contains(toResolve.GetType().GetTypeInfo().ImplementedInterfaces, i => i == typeof(IOG2<C2, C>));
            Assert.True(toResolve.GetType() == typeof(OG2<C2, C>));
        }

        [Fact]
        public static void Resolves_successfully_when_registered_closed_generic_with_two_generic_type_parameter()
        {
            var instance = MvxIoCProvider.Initialize();
            ((MvxIoCProvider)instance).CleanAllResolvers();

            instance.RegisterType(typeof(IOG2<C2,C>), typeof(OG2<C2,C>));

            IOG2<C2, C> toResolve = null;
            Mvx.TryResolve<IOG2<C2, C>>(out toResolve);

            Assert.NotNull(toResolve);
            Assert.Contains(toResolve.GetType().GetTypeInfo().ImplementedInterfaces, i => i == typeof(IOG2<C2, C>));
            Assert.True(toResolve.GetType() == typeof(OG2<C2, C>));
        }

        [Fact]
        public static void Resolves_unsuccessfully_when_registered_open_generic_with_one_generic_parameter_that_was_not_registered()
        {
            var instance = MvxIoCProvider.Initialize();
            ((MvxIoCProvider)instance).CleanAllResolvers();

            IOG<C2> toResolve = null;

            var isResolved = Mvx.TryResolve<IOG<C2>>(out toResolve);

            Assert.False(isResolved);
            Assert.Null(toResolve);
        }

        [Fact]
        public static void Resolves_successfully_when_resolving_entity_that_has_injected_an_open_generic_parameter()
        {
            var instance = MvxIoCProvider.Initialize();
            ((MvxIoCProvider)instance).CleanAllResolvers();

            instance.RegisterType<IHasOGParameter, HasOGParameter>();
            instance.RegisterType(typeof(IOG<>), typeof(OG<>));

            IHasOGParameter toResolve = null;
            Mvx.TryResolve<IHasOGParameter>(out toResolve);

            Assert.NotNull(toResolve);
            Assert.Contains(toResolve.GetType().GetTypeInfo().ImplementedInterfaces, i => i == typeof(IHasOGParameter));
            Assert.True(toResolve.GetType() == typeof(HasOGParameter));
            Assert.Contains(toResolve.OpenGeneric.GetType().GetTypeInfo().ImplementedInterfaces, i => i == typeof(IOG<C>));
            Assert.True(toResolve.OpenGeneric.GetType() == typeof(OG<C>));
        }

        #endregion

        #region Child Container

        [Fact]
        public static void Resolves_successfully_when_using_childcontainer()
        {
            var container = MvxIoCProvider.Initialize();
            ((MvxIoCProvider)container).CleanAllResolvers();

            container.RegisterType<IC, C2>();
            var childContainer = container.CreateChildContainer();
            childContainer.RegisterType<IB, B>();

            var b = childContainer.Create<IB>();

            Assert.True(container.CanResolve<IC>());
            Assert.False(container.CanResolve<IB>());
            Assert.True(childContainer.CanResolve<IC>());
            Assert.True(childContainer.CanResolve<IB>());

            Assert.NotNull(b);
            Assert.NotNull(b.C);
        }

        #endregion

        [Fact]
        public void IocConstruct_WithDictionaryArguments_CreatesObject()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            var c = new C2();
            var arguments = new Dictionary<string, object>
            {
                ["c"] = c
            };
            instance.IoCConstruct<B>(arguments);
        }

        [Fact]
        public void IocConstruct_WithAnonymousTypeArguments_CreatesObject()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            var c = new C2();
            instance.IoCConstruct<B>(new { c });
        }

        [Fact]
        public void IocConstruct_WithMultipleDictionaryArguments_CreatesObject()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            var title = "The title";
            var subtitle = "The subtitle";
            var description = "The description";

            var arguments = new Dictionary<string, object>
            {
                ["title"] = title,
                ["subtitle"] = subtitle,
                ["description"] = description
            };
            var d = instance.IoCConstruct<D>(arguments);

            Assert.Equal(title, d.Title);
            Assert.Equal(subtitle, d.Subtitle);
            Assert.Equal(description, d.Description);
        }

        [Fact]
        public void IocConstruct_WithMultipleAnonymousArguments_CreatesObject()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            var title = "The title";
            var subtitle = "The subtitle";
            var description = "The description";

            var arguments = new { title, subtitle, description };
            var d = instance.IoCConstruct<D>(arguments);

            Assert.Equal(title, d.Title);
            Assert.Equal(subtitle, d.Subtitle);
            Assert.Equal(description, d.Description);
        }

        [Fact]
        public void IocConstruct_WithMultipleTypedArguments_CreatesObject()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            var title = "The title";
            var amount = 5;
            var enabled = true;

            var d = instance.IoCConstruct<D>(title, amount, enabled);

            Assert.Equal(title, d.Title);
            Assert.Equal(amount, d.Amount);
            Assert.Equal(enabled, d.Enabled);
        }

        [Fact]
        public void IocConstruct_WithMultipleTypedArguments_ThrowsFailedToFindCtor()
        {
            MvxSingleton.ClearAllSingletons();
            var instance = MvxIoCProvider.Initialize();

            var title = "The title";
            var subtitle = "The subtitle";
            var enabled = true;

            Assert.Throws<MvxIoCResolveException>(() => {
                var d = instance.IoCConstruct<D>(title, subtitle, enabled);
            });
        }

        // TODO - there are so many tests we could and should do here!
    }
}
