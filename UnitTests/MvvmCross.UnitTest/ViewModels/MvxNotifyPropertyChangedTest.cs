// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.Dispatchers;
using MvvmCross.ViewModels;

namespace MvvmCross.UnitTest.ViewModels
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
    public class MvxNotifyPropertyChangedTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxNotifyPropertyChangedTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        public class TestInpc : MvxNotifyPropertyChanged
        {
            private string _foo;

            public string Foo { get => _foo; set => SetProperty(ref _foo, value); }
        }

        public class TestInpc2 : MvxNotifyPropertyChanged
        {
            public int TestActionValue;

            private string _foo;

            public string Foo
            {
                get => _foo;
                set => SetProperty(ref _foo, value, () => TestActionValue++);
            }
        }

        public class TestInpc3 : MvxNotifyPropertyChanged
        {
            public int TestActionValue;

            private string _foo;

            public string Foo
            {
                get => _foo;
                set => SetProperty(ref _foo, value, (setPropertyResult) =>
                {
                    if (setPropertyResult)
                    {
                        TestActionValue++;
                    }
                });
            }
        }

        public class TestInpc4 : MvxNotifyPropertyChanged
        {
            public int TestActionValue;

            private string _foo;

            public Action<bool> IncreaseTestActionValue = null;

            public string Foo
            {
                get => _foo;
                set => SetProperty(ref _foo, value, IncreaseTestActionValue);
            }
        }

        [Test]
        public async Task Test_RaisePropertyChangingForExpression()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            var newValue = string.Empty;
            t.PropertyChanging += (sender, args) =>
            {
                notified.Add(args.PropertyName);
                newValue = (args as MvxPropertyChangingEventArgs<string>)?.NewValue;
            };
            t.RaisePropertyChanging("Foobar", () => t.Foo);

            await Assert.That(notified.Count == 1).IsTrue();
            await Assert.That(notified[0] == "Foo").IsTrue();
            await Assert.That(newValue).IsEqualTo("Foobar");
        }

        [Test]
        public async Task Test_RaisePropertyChangingForName()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            var newValue = string.Empty;
            t.PropertyChanging += (sender, args) =>
            {
                notified.Add(args.PropertyName);
                newValue = (args as MvxPropertyChangingEventArgs<string>)?.NewValue;
            };
            t.RaisePropertyChanging("Foobar", "Foo");

            await Assert.That(notified.Count == 1).IsTrue();
            await Assert.That(notified[0] == "Foo").IsTrue();
            await Assert.That(newValue).IsEqualTo("Foobar");
        }

        [Test]
        public async Task Test_RaisePropertyChangingDirect()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            var newValue = string.Empty;
            t.PropertyChanging += (sender, args) =>
            {
                notified.Add(args.PropertyName);
                newValue = (args as MvxPropertyChangingEventArgs<string>)?.NewValue;
            };
            t.RaisePropertyChanging(new MvxPropertyChangingEventArgs<string>("Foo", "Foobar"));

            await Assert.That(notified.Count == 1).IsTrue();
            await Assert.That(notified[0] == "Foo").IsTrue();
            await Assert.That(newValue).IsEqualTo("Foobar");
        }

        [Test]
        public async Task Test_RaisePropertyChangedForExpression()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            await t.RaisePropertyChanged(() => t.Foo);

            await Assert.That(notified.Count == 1).IsTrue();
            await Assert.That(notified[0] == "Foo").IsTrue();
        }

        [Test]
        public async Task Test_RaisePropertyChangedForName()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            await t.RaisePropertyChanged("Foo");

            await Assert.That(notified.Count == 1).IsTrue();
            await Assert.That(notified[0] == "Foo").IsTrue();
        }

        [Test]
        public async Task Test_RaisePropertyChangedDirect()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            await t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            await Assert.That(notified.Count == 1).IsTrue();
            await Assert.That(notified[0] == "Foo").IsTrue();
        }

        [Test]
        public async Task Test_SetPropertyChangeValue()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            t.Foo = "Foobar";

            await Assert.That(notified.Count == 1).IsTrue();
            await Assert.That(notified[0] == "Foo").IsTrue();
            await Assert.That(t.Foo).IsEqualTo("Foobar");
        }

        [Test]
        public async Task Test_SetPropertyNoValueChange()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.Foo = "Foobar";
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            t.Foo = "Foobar";

            await Assert.That(notified.Count == 0).IsTrue();
            await Assert.That(t.Foo).IsEqualTo("Foobar");
        }

        [Test]
        public async Task Test_SetPropertyChangeValueCancelled()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.Foo = "Default value";
            t.PropertyChanging += (sender, args) => (args as MvxPropertyChangingEventArgs<string>).Cancel = true;
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            t.Foo = "Foobar";

            await Assert.That(notified.Count == 0).IsTrue();
            await Assert.That(t.Foo).IsEqualTo("Default value");
        }

        [Test]
        public async Task Test_TurnOffUIThread()
        {
            _fixture.ClearAll();
            var dispatcher = new CountingMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            t.ShouldAlwaysRaiseInpcOnUserInterfaceThread(false);
            await t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            await Assert.That(dispatcher.Count == 0).IsTrue();
            await Assert.That(notified.Count == 1).IsTrue();
            await Assert.That(notified[0] == "Foo").IsTrue();

            t.ShouldAlwaysRaiseInpcOnUserInterfaceThread(true);
            await t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            await Assert.That(dispatcher.Count).IsEqualTo(1);
            await Assert.That(notified.Count == 2).IsTrue();
            await Assert.That(notified[0] == "Foo").IsTrue();
        }

        [Test]
        public async Task Test_SetPropertyAfterActionCalledAfterPropertyChanged()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var t2 = new TestInpc2();
            await Assert.That(t2.TestActionValue).IsEqualTo(0);
            t2.Foo = "Foo";
            await Assert.That(t2.TestActionValue).IsEqualTo(1);
            t2.Foo = "Foo";
            await Assert.That(t2.TestActionValue).IsEqualTo(1);
            t2.Foo = "Foobar";
            await Assert.That(t2.TestActionValue).IsEqualTo(2);
            await Assert.That(t2.TestActionValue).IsNotEqualTo(0);

            var t3 = new TestInpc3();
            await Assert.That(t3.TestActionValue).IsEqualTo(0);
            t3.Foo = "Foo";
            await Assert.That(t3.TestActionValue).IsEqualTo(1);
            t3.Foo = "Foo";
            await Assert.That(t3.TestActionValue).IsEqualTo(1);
            t3.Foo = "Foobar";
            await Assert.That(t3.TestActionValue).IsEqualTo(2);
            await Assert.That(t3.TestActionValue).IsNotEqualTo(0);

            var t4 = new TestInpc4();
            await Assert.That(t4.TestActionValue).IsEqualTo(0);
            Assert.Throws<ArgumentException>(() => t4.Foo = "Foo");
            t4.IncreaseTestActionValue = setPropertyResult =>
            {
                if (setPropertyResult)
                {
                    t4.TestActionValue++;
                }
            };
            t4.Foo = "Foo";
            await Assert.That(t4.TestActionValue).IsEqualTo(1);
            t4.Foo = "Foo";
            await Assert.That(t4.TestActionValue).IsEqualTo(1);
            t4.Foo = "Foobar";
            await Assert.That(t4.TestActionValue).IsEqualTo(2);
            await Assert.That(t4.TestActionValue).IsNotEqualTo(0);
        }

        public class Interceptor : IMvxInpcInterceptor
        {
            public Func<IMvxNotifyPropertyChanged, PropertyChangedEventArgs, MvxInpcInterceptionResult> Handler;
            public Func<IMvxNotifyPropertyChanged, PropertyChangingEventArgs, MvxInpcInterceptionResult> ChangingHandler;

            public MvxInpcInterceptionResult Intercept(IMvxNotifyPropertyChanged sender, PropertyChangedEventArgs args)
            {
                return Handler(sender, args);
            }

            public MvxInpcInterceptionResult Intercept(IMvxNotifyPropertyChanged sender, PropertyChangingEventArgs args)
            {
                return ChangingHandler(sender, args);
            }
        }

        [Test]
        public async Task Test_Interceptor()
        {
            _fixture.ClearAll();

            var dispatcher = new CountingMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var interceptor = new Interceptor();
            _fixture.Ioc.RegisterSingleton<IMvxInpcInterceptor>(interceptor);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            interceptor.ChangingHandler = (s, e) => MvxInpcInterceptionResult.RaisePropertyChanging;
            interceptor.Handler = (s, e) => MvxInpcInterceptionResult.RaisePropertyChanged;
            await t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            await Assert.That(dispatcher.Count == 1).IsTrue();

            interceptor.Handler = (s, e) => MvxInpcInterceptionResult.DoNotRaisePropertyChanged;
            interceptor.ChangingHandler = (s, e) => MvxInpcInterceptionResult.RaisePropertyChanging;
            await t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            await Assert.That(dispatcher.Count == 1).IsTrue();

            interceptor.Handler = (s, e) => MvxInpcInterceptionResult.RaisePropertyChanged;
            interceptor.ChangingHandler = (s, e) => MvxInpcInterceptionResult.RaisePropertyChanging;
            await t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            await Assert.That(dispatcher.Count == 2).IsTrue();
        }
    }
}
