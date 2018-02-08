// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using MvvmCross.Base.Core;
using MvvmCross.Core.ViewModels;
using MvvmCross.Test;
using MvvmCross.UnitTest.Mocks.Dispatchers;
using Xunit;

namespace MvvmCross.UnitTest.ViewModels
{
    [Collection("MvxTest")]
    public class MvxNotifyPropertyChangedTest
    {
        private readonly MvxTestFixture _fixture;

        public MvxNotifyPropertyChangedTest(MvxTestFixture fixture)
        {
            _fixture = fixture;
        }

        public class TestInpc : MvxNotifyPropertyChanged
        {
            public string Foo { get; set; }
        }

        [Fact]
        public void Test_RaisePropertyChangedForExpression()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            t.RaisePropertyChanged(() => t.Foo);

            Assert.True(notified.Count == 1);
            Assert.True(notified[0] == "Foo");
        }

        [Fact]
        public void Test_RaisePropertyChangedForName()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            t.RaisePropertyChanged("Foo");

            Assert.True(notified.Count == 1);
            Assert.True(notified[0] == "Foo");
        }

        [Fact]
        public void Test_RaisePropertyChangedDirect()
        {
            _fixture.ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.True(notified.Count == 1);
            Assert.True(notified[0] == "Foo");
        }

        [Fact]
        public void Test_TurnOffUIThread()
        {
            _fixture.ClearAll();
            var dispatcher = new CountingMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            t.ShouldAlwaysRaiseInpcOnUserInterfaceThread(false);
            t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.True(dispatcher.Count == 0);
            Assert.True(notified.Count == 1);
            Assert.True(notified[0] == "Foo");

            t.ShouldAlwaysRaiseInpcOnUserInterfaceThread(true);
            t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.Equal(1, dispatcher.Count);
            Assert.True(notified.Count == 1);
            Assert.True(notified[0] == "Foo");
        }

        public class Interceptor : IMvxInpcInterceptor
        {
            public Func<IMvxNotifyPropertyChanged, PropertyChangedEventArgs, MvxInpcInterceptionResult> Handler;

            public MvxInpcInterceptionResult Intercept(IMvxNotifyPropertyChanged sender, PropertyChangedEventArgs args)
            {
                return Handler(sender, args);
            }
        }

        [Fact]
        public void Test_Interceptor()
        {
            _fixture.ClearAll();

            var dispatcher = new CountingMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var interceptor = new Interceptor();
            _fixture.Ioc.RegisterSingleton<IMvxInpcInterceptor>(interceptor);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            interceptor.Handler = (s, e) => MvxInpcInterceptionResult.RaisePropertyChanged;
            t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.True(dispatcher.Count == 1);

            interceptor.Handler = (s, e) => MvxInpcInterceptionResult.DoNotRaisePropertyChanged;
            t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.True(dispatcher.Count == 1);

            interceptor.Handler = (s, e) => MvxInpcInterceptionResult.RaisePropertyChanged;
            t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.True(dispatcher.Count == 2);
        }
    }
}
