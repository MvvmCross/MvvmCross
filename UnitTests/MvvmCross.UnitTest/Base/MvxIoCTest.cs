// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using MvvmCross.IoC;
using Xunit;

namespace MvvmCross.UnitTest.Base
{
    [Collection("MvxTest")]
    public class MvxIocTest : IDisposable
    {
        private IMvxIoCProvider _iocProvider;

        public MvxIocTest()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            _iocProvider = CreateIoCProvider();
        }

        protected virtual IMvxIoCProvider CreateIoCProvider(IMvxIocOptions options = null)
        {
            return MvxIoCProvider.Initialize(options);
        }

        public void Dispose()
        {
            _iocProvider = null;
            MvxSingleton.ClearAllSingletons();
        }

        #region Test data

        protected interface IA
        {
            IB B { get; }
        }

        protected interface IB
        {
            IC C { get; }
        }

        protected interface IC
        {
        }

        protected interface ID
        {
        }

        protected interface IE
        {
        }

        protected interface IOG<T>
        {
        }

        protected interface IOG2<T, T2>
        {
        }

        protected interface IHasOGParameter
        {
            IOG<C> OpenGeneric { get; }
        }

        protected interface IHasOneParameter
        {
            IA A { get; }
        }

        protected interface IHasTwoParameters : IHasOneParameter
        {
            IB B { get; }
        }

        protected interface IHasThreeParameters : IHasTwoParameters
        {
            IC C { get; }
        }

        protected interface IHasFourParameters : IHasThreeParameters
        {
            ID D { get; }
        }

        protected interface IHasFiveParameters : IHasFourParameters
        {
            IE E { get; }
        }

        protected class A : IA
        {
            public A(IB b)
            {
                B = b;
            }

            public IB B { get; set; }
        }

        protected class B : IB
        {
            public B(IC c)
            {
                C = c;
            }

            public IC C { get; set; }
        }

        protected class C : IC
        {
            public C(IA a)
            {
            }
        }

        protected class C2 : IC
        {
            public C2()
            {
            }
        }

        protected class D : ID
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

        protected class E : IE
        {
        }

        protected class F
        {
            private readonly IC _c;
            private readonly string _first;
            private readonly int _second;

            public F(IC c, string first, int second)
            {
                _c = c;
                _first = first;
                _second = second;
            }

            public F(string first)
            {
                _first = first;
            }

            public string First => _first;
            public int Second => _second;
            public IC C => _c;
        }

        protected class COdd : IC
        {
            public static bool FirstTime = true;

            public COdd()
            {
                if (FirstTime)
                {
                    FirstTime = false;
                    Mvx.IoCProvider.Resolve<IA>();
                }
            }
        }

        protected class OG<T> : IOG<T>
        {
        }

        protected class OG2<T, T2> : IOG2<T, T2>
        {
        }

        protected class HasOGParameter : IHasOGParameter
        {
            public HasOGParameter(IOG<C> openGeneric)
            {
                this.OpenGeneric = openGeneric;
            }

            public IOG<C> OpenGeneric { get; }
        }

        protected class HasMultipleConstructors : IHasFiveParameters
        {
            public IA A { get; }

            public IB B { get; }

            public IC C { get; }

            public ID D { get; }

            public IE E { get; }

            public HasMultipleConstructors(IA a)
            {
                A = a;
            }

            public HasMultipleConstructors(IA a, IB b) : this(a)
            {
                B = b;
            }

            public HasMultipleConstructors(IA a, IB b, IC c) : this(a, b)
            {
                C = c;
            }

            public HasMultipleConstructors(IA a, IB b, IC c, ID d) : this(a, b, c)
            {
                D = d;
            }

            public HasMultipleConstructors(IA a, IB b, IC c, ID d, IE e) : this(a, b, c, d)
            {
                E = e;
            }
        }

        #endregion

        [Fact]
        public virtual void TryResolve_CircularButSafeDynamicWithOptionOff_ReturnsTrue()
        {
            COdd.FirstTime = true;

            Dispose();
            _iocProvider = CreateIoCProvider(new MvxIocOptions { TryToDetectDynamicCircularReferences = false });

            _iocProvider.RegisterType<IA, A>();
            _iocProvider.RegisterType<IB, B>();
            _iocProvider.RegisterType<IC, COdd>();

            var result = _iocProvider.TryResolve(out IA a);
            Assert.True(result);
            Assert.NotNull(a);
        }

        [Fact]
        public virtual void TryResolve_CircularButSafeDynamicWithOptionOn_ReturnsFalse()
        {
            COdd.FirstTime = true;

            _iocProvider.RegisterType<IA, A>();
            _iocProvider.RegisterType<IB, B>();
            _iocProvider.RegisterType<IC, COdd>();

            var result = _iocProvider.TryResolve(out IA a);
            Assert.False(result);
            Assert.Null(a);
        }

        [Fact]
        public virtual void TryResolve_CircularLazyRegistration_ReturnsFalse()
        {
            _iocProvider.LazyConstructAndRegisterSingleton<IA, A>();
            _iocProvider.LazyConstructAndRegisterSingleton<IB, B>();
            _iocProvider.LazyConstructAndRegisterSingleton<IC, C>();

            var result = _iocProvider.TryResolve(out IA a);
            Assert.False(result);
            Assert.Null(a);
        }

        [Fact]
        public virtual void TryResolve_NonCircularRegistration_ReturnsTrue()
        {
            _iocProvider.LazyConstructAndRegisterSingleton<IA, A>();
            _iocProvider.LazyConstructAndRegisterSingleton<IB, B>();
            _iocProvider.LazyConstructAndRegisterSingleton<IC, C2>();

            var result = _iocProvider.TryResolve(out IA a);
            Assert.True(result);
            Assert.NotNull(a);
        }

        [Fact]
        public virtual void TryResolve_LazySingleton_ReturnsSameSingletonEachTime()
        {
            _iocProvider.LazyConstructAndRegisterSingleton<IA, A>();
            _iocProvider.LazyConstructAndRegisterSingleton<IB, B>();
            _iocProvider.LazyConstructAndRegisterSingleton<IC, C2>();

            var result = _iocProvider.TryResolve(out IA a0);
            Assert.True(result);
            Assert.NotNull(a0);

            for (var i = 0; i < 100; i++)
            {
                result = _iocProvider.TryResolve(out IA a1);
                Assert.True(result);
                Assert.Equal(a0, a1);
            }
        }

        [Fact]
        public virtual void TryResolve_NonLazySingleton_ReturnsSameSingletonEachTime()
        {
            _iocProvider.LazyConstructAndRegisterSingleton<IB, B>();
            _iocProvider.LazyConstructAndRegisterSingleton<IC, C2>();
            _iocProvider.ConstructAndRegisterSingleton<IA, A>();

            var result = _iocProvider.TryResolve(out IA a0);
            Assert.True(result);
            Assert.NotNull(a0);

            for (var i = 0; i < 100; i++)
            {
                result = _iocProvider.TryResolve(out IA a1);
                Assert.True(result);
                Assert.Equal(a0, a1);
            }
        }

        [Fact]
        public virtual void TryResolve_Dynamic_ReturnsDifferentInstanceEachTime()
        {
            _iocProvider.LazyConstructAndRegisterSingleton<IB, B>();
            _iocProvider.LazyConstructAndRegisterSingleton<IC, C2>();
            _iocProvider.RegisterType<IA, A>();

            var previous = new Dictionary<IA, bool>();

            for (var i = 0; i < 100; i++)
            {
                var result = _iocProvider.TryResolve(out IA a1);
                Assert.True(result);
                Assert.False(previous.ContainsKey(a1));
                Assert.Equal(i, previous.Count);
                previous.Add(a1, true);
            }
        }

        [Fact]
        public virtual void TryResolve_ParameterConstructors_CreatesParametersUsingIocResolution()
        {
            _iocProvider.RegisterType<IB, B>();
            _iocProvider.LazyConstructAndRegisterSingleton<IC, C2>();
            _iocProvider.RegisterType<IA, A>();

            var result = _iocProvider.TryResolve(out IA a1);
            Assert.True(result);
            Assert.NotNull(a1);
            Assert.NotNull(a1.B);
            Assert.IsType<B>(a1.B);
        }

        [Fact]
        public virtual void RegisterType_with_constructor_creates_different_objects()
        {
            _iocProvider.RegisterType<IC>(() => new C2());

            var c1 = _iocProvider.Resolve<IC>();
            var c2 = _iocProvider.Resolve<IC>();

            Assert.NotNull(c1);
            Assert.NotNull(c2);

            Assert.NotEqual(c1, c2);
        }

        [Fact]
        public virtual void RegisterType_with_no_reflection_creates_different_objects()
        {
            _iocProvider.RegisterType<IA, IB>(b => new A(b));
            _iocProvider.RegisterType<IB, IC>(c => new B(c));
            _iocProvider.RegisterType<IC>(() => new C2());

            var typesToResolve = new[] { typeof(IA), typeof(IB), typeof(IC) };
            foreach (var type in typesToResolve)
            {
                var obj1 = _iocProvider.Resolve(type);
                var obj2 = _iocProvider.Resolve(type);

                Assert.NotNull(obj1);
                Assert.NotNull(obj2);

                Assert.NotEqual(obj1, obj2);
            }
        }

        [Fact]
        public virtual void RegisterType_with_no_reflection_with_up_to_5_parameters()
        {
            _iocProvider.RegisterType<IA, IB>(b => new A(b));
            _iocProvider.RegisterType<IB, IC>(c => new B(c));
            _iocProvider.RegisterType<IC>(() => new C2());
            _iocProvider.RegisterType<ID>(() => new D("Test", "Test subtitle", "Description"));
            _iocProvider.RegisterType<IE>(() => new E());

            _iocProvider.RegisterType<IHasFiveParameters, IA, IB, IC, ID, IE>((a, b, c, d, e) => new HasMultipleConstructors(a, b, c, d, e));
            _iocProvider.RegisterType<IHasFourParameters, IA, IB, IC, ID>((a, b, c, d) => new HasMultipleConstructors(a, b, c, d));
            _iocProvider.RegisterType<IHasThreeParameters, IA, IB, IC>((a, b, c) => new HasMultipleConstructors(a, b, c));
            _iocProvider.RegisterType<IHasTwoParameters, IA, IB>((a, b) => new HasMultipleConstructors(a, b));
            _iocProvider.RegisterType<IHasOneParameter, IA>(a => new HasMultipleConstructors(a));

            var obj1 = _iocProvider.Resolve<IHasOneParameter>();
            var obj2 = _iocProvider.Resolve<IHasTwoParameters>();
            var obj3 = _iocProvider.Resolve<IHasThreeParameters>();
            var obj4 = _iocProvider.Resolve<IHasFourParameters>();
            var obj5 = _iocProvider.Resolve<IHasFiveParameters>();

            Assert.NotNull(obj1);
            Assert.NotNull(obj1.A);
            Assert.NotNull(obj2);
            Assert.NotNull(obj2.B);
            Assert.NotNull(obj3);
            Assert.NotNull(obj3.C);
            Assert.NotNull(obj4);
            Assert.NotNull(obj4.D);
            Assert.NotNull(obj5);
            Assert.NotNull(obj5.E);
        }

        [Fact]
        public virtual void Non_generic_RegisterType_with_constructor_creates_different_objects()
        {
            _iocProvider.RegisterType(typeof(IC), () => new C2());

            var c1 = _iocProvider.Resolve<IC>();
            var c2 = _iocProvider.Resolve<IC>();

            Assert.NotNull(c1);
            Assert.NotNull(c2);

            Assert.NotEqual(c1, c2);
        }

        [Fact]
        public virtual void Non_generic_RegisterType_with_constructor_throws_if_constructor_returns_incompatible_reference()
        {
            _iocProvider.RegisterType(typeof(IC), () => "Fail");

            Assert.Throws<MvxIoCResolveException>(() => { _iocProvider.Resolve<IC>(); });
        }

        [Fact]
        public virtual void Non_generic_RegisterType_with_constructor_throws_if_constructor_returns_incompatible_value()
        {
            _iocProvider.RegisterType(typeof(IC), () => 36);

            Assert.Throws<MvxIoCResolveException>(() => { _iocProvider.Resolve<IC>(); });
        }

        #region Open-Generics

        [Fact]
        public virtual void Resolves_successfully_when_registered_open_generic_with_one_generic_type_parameter()
        {
            _iocProvider.RegisterType(typeof(IOG<>), typeof(OG<>));

            _iocProvider.TryResolve(out IOG<C2> toResolve);

            Assert.NotNull(toResolve);
            Assert.Contains(toResolve.GetType().GetTypeInfo().ImplementedInterfaces, i => i == typeof(IOG<C2>));
            Assert.True(toResolve.GetType() == typeof(OG<C2>));
        }

        [Fact]
        public virtual void Resolves_successfully_when_registered_closed_generic_with_one_generic_type_parameter()
        {
            _iocProvider.RegisterType(typeof(IOG<C2>), typeof(OG<C2>));

            _iocProvider.TryResolve(out IOG<C2> toResolve);

            Assert.NotNull(toResolve);
            Assert.Contains(toResolve.GetType().GetTypeInfo().ImplementedInterfaces, i => i == typeof(IOG<C2>));
            Assert.True(toResolve.GetType() == typeof(OG<C2>));
        }

        [Fact]
        public virtual void Resolves_successfully_when_registered_open_generic_with_two_generic_type_parameter()
        {
            _iocProvider.RegisterType(typeof(IOG2<,>), typeof(OG2<,>));

            _iocProvider.TryResolve(out IOG2<C2, C> toResolve);

            Assert.NotNull(toResolve);
            Assert.Contains(toResolve.GetType().GetTypeInfo().ImplementedInterfaces, i => i == typeof(IOG2<C2, C>));
            Assert.True(toResolve.GetType() == typeof(OG2<C2, C>));
        }

        [Fact]
        public virtual void Resolves_successfully_when_registered_closed_generic_with_two_generic_type_parameter()
        {
            _iocProvider.RegisterType(typeof(IOG2<C2, C>), typeof(OG2<C2, C>));

            _iocProvider.TryResolve(out IOG2<C2, C> toResolve);

            Assert.NotNull(toResolve);
            Assert.Contains(toResolve.GetType().GetTypeInfo().ImplementedInterfaces, i => i == typeof(IOG2<C2, C>));
            Assert.True(toResolve.GetType() == typeof(OG2<C2, C>));
        }

        [Fact]
        public virtual void Resolves_unsuccessfully_when_registered_open_generic_with_one_generic_parameter_that_was_not_registered()
        {
            var isResolved = _iocProvider.TryResolve(out IOG<C2> toResolve);

            Assert.False(isResolved);
            Assert.Null(toResolve);
        }

        [Fact]
        public virtual void Resolves_successfully_when_resolving_entity_that_has_injected_an_open_generic_parameter()
        {
            _iocProvider.RegisterType<IHasOGParameter, HasOGParameter>();
            _iocProvider.RegisterType(typeof(IOG<>), typeof(OG<>));

            _iocProvider.TryResolve(out IHasOGParameter toResolve);

            Assert.NotNull(toResolve);
            Assert.Contains(toResolve.GetType().GetTypeInfo().ImplementedInterfaces, i => i == typeof(IHasOGParameter));
            Assert.True(toResolve.GetType() == typeof(HasOGParameter));
            Assert.Contains(toResolve.OpenGeneric.GetType().GetTypeInfo().ImplementedInterfaces, i => i == typeof(IOG<C>));
            Assert.True(toResolve.OpenGeneric.GetType() == typeof(OG<C>));
        }

        #endregion Open-Generics

        #region Child Container

        [Fact]
        public virtual void Resolves_successfully_when_using_childcontainer()
        {
            _iocProvider.RegisterType<IC, C2>();
            var childContainer = _iocProvider.CreateChildContainer();
            childContainer.RegisterType<IB, B>();

            var b = childContainer.Create<IB>();

            Assert.True(_iocProvider.CanResolve<IC>());
            Assert.False(_iocProvider.CanResolve<IB>());
            Assert.True(childContainer.CanResolve<IC>());
            Assert.True(childContainer.CanResolve<IB>());

            Assert.NotNull(b);
            Assert.NotNull(b.C);
        }

        #endregion Child Container

        [Fact]
        public virtual void IocConstruct_WithDictionaryArguments_CreatesObject()
        {
            var c = new C2();
            var arguments = new Dictionary<string, object> { ["c"] = c };
            _iocProvider.IoCConstruct<B>(arguments);
        }

        [Fact]
        public virtual void IocConstruct_WithAnonymousTypeArguments_CreatesObject()
        {
            var c = new C2();
            _iocProvider.IoCConstruct<B>(new { c });
        }

        [Fact]
        public virtual void IocConstruct_WithMultipleDictionaryArguments_CreatesObject()
        {
            var title = "The title";
            var subtitle = "The subtitle";
            var description = "The description";

            var arguments = new Dictionary<string, object> { ["title"] = title, ["subtitle"] = subtitle, ["description"] = description };
            var d = _iocProvider.IoCConstruct<D>(arguments);

            Assert.Equal(title, d.Title);
            Assert.Equal(subtitle, d.Subtitle);
            Assert.Equal(description, d.Description);
        }

        [Fact]
        public virtual void IocConstruct_WithMultipleAnonymousArguments_CreatesObject()
        {
            var title = "The title";
            var subtitle = "The subtitle";
            var description = "The description";

            var arguments = new { title, subtitle, description };
            var d = _iocProvider.IoCConstruct<D>(arguments);

            Assert.Equal(title, d.Title);
            Assert.Equal(subtitle, d.Subtitle);
            Assert.Equal(description, d.Description);
        }

        [Fact]
        public virtual void IocConstruct_WithMultipleTypedArguments_CreatesObject()
        {
            var title = "The title";
            var amount = 5;
            var enabled = true;

            var d = _iocProvider.IoCConstruct<D>(title, amount, enabled);

            Assert.Equal(title, d.Title);
            Assert.Equal(amount, d.Amount);
            Assert.Equal(enabled, d.Enabled);
        }

        [Fact]
        public virtual void IocConstruct_WithMultipleTypedArguments_ThrowsFailedToFindCtor()
        {
            var title = "The title";
            var subtitle = "The subtitle";
            var enabled = true;

            Assert.Throws<MvxIoCResolveException>(() => { _iocProvider.IoCConstruct<D>(title, subtitle, enabled); });
        }

        [Fact]
        public virtual void IocConstruct_WithMultipleTypedArgumentsAndInjectedArgument_CreatesObject()
        {
            _iocProvider.RegisterType<IC, C2>();
            var first = "first";
            var second = 2;

            var f = _iocProvider.IoCConstruct<F>(first, second);

            Assert.NotNull(f);
            Assert.NotNull(f.C);
            Assert.Equal(first, f.First);
            Assert.Equal(second, f.Second);
        }

        [Fact]
        public virtual void MvxIocProvider_NonLazySingleton_ReturnsSameSingleton()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IB, B>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IC, C2>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IA, A>();

            var result = Mvx.IoCProvider.TryResolve(out IA a0);
            Assert.True(result);
            Assert.NotNull(a0);

            for (var i = 0; i < 100; i++)
            {
                result = Mvx.IoCProvider.TryResolve(out IA a1);
                Assert.True(result);
                Assert.Equal(a0, a1);
            }
        }

        // TODO - there are so many tests we could and should do here!
    }
}
