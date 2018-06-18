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
using Xunit;

namespace MvvmCross.UnitTest.ViewModels
{
    [Collection("MvxTest")]
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

        [Fact]
        public void Test_RaisePropertyChangingForExpression()
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

            Assert.True(notified.Count == 1);
            Assert.True(notified[0] == "Foo");
            Assert.Equal("Foobar", newValue);
        }

        [Fact]
        public void Test_RaisePropertyChangingForName()
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

            Assert.True(notified.Count == 1);
            Assert.True(notified[0] == "Foo");
            Assert.Equal("Foobar", newValue);
        }

        [Fact]
        public void Test_RaisePropertyChangingDirect()
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

            Assert.True(notified.Count == 1);
            Assert.True(notified[0] == "Foo");
            Assert.Equal("Foobar", newValue);
        }

        [Fact]
        public async Task Test_RaisePropertyChangedForExpression()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            await t.RaisePropertyChanged(() => t.Foo);

            Assert.True(notified.Count == 1);
            Assert.True(notified[0] == "Foo");
        }

        [Fact]
        public async Task Test_RaisePropertyChangedForName()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            await t.RaisePropertyChanged("Foo");

            Assert.True(notified.Count == 1);
            Assert.True(notified[0] == "Foo");
        }

        [Fact]
        public async Task Test_RaisePropertyChangedDirect()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            await t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.True(notified.Count == 1);
            Assert.True(notified[0] == "Foo");
        }

        [Fact]
        public void Test_SetPropertyChangeValue()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            t.Foo = "Foobar";

            Assert.True(notified.Count == 1);
            Assert.True(notified[0] == "Foo");
            Assert.Equal("Foobar", t.Foo);
        }

        [Fact]
        public void Test_SetPropertyNoValueChange()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.Foo = "Foobar";
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            t.Foo = "Foobar";

            Assert.True(notified.Count == 0);
            Assert.Equal("Foobar", t.Foo);
        }

        [Fact]
        public void Test_SetPropertyChangeValueCancelled()
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

            Assert.True(notified.Count == 0);
            Assert.Equal("Default value", t.Foo);
        }

        [Fact]
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

            Assert.True(dispatcher.Count == 0);
            Assert.True(notified.Count == 1);
            Assert.True(notified[0] == "Foo");

            t.ShouldAlwaysRaiseInpcOnUserInterfaceThread(true);
            await t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.Equal(1, dispatcher.Count);
            Assert.True(notified.Count == 2);
            Assert.True(notified[0] == "Foo");
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

        [Fact]
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

            Assert.True(dispatcher.Count == 1);

            interceptor.Handler = (s, e) => MvxInpcInterceptionResult.DoNotRaisePropertyChanged;
            interceptor.ChangingHandler = (s, e) => MvxInpcInterceptionResult.RaisePropertyChanging;
            await t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.True(dispatcher.Count == 1);

            interceptor.Handler = (s, e) => MvxInpcInterceptionResult.RaisePropertyChanged;
            interceptor.ChangingHandler = (s, e) => MvxInpcInterceptionResult.RaisePropertyChanging;
            await t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.True(dispatcher.Count == 2);
        }
    }
}
