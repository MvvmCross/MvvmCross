// MvxNotifyPropertyChangedTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Test.Core;
using Cirrious.MvvmCross.Test.Mocks.Dispatchers;
using Cirrious.MvvmCross.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Cirrious.MvvmCross.Test.ViewModels
{
    [TestFixture]
    public class MvxNotifyPropertyChangedTest : MvxIoCSupportingTest
    {
        public class TestInpc : MvxNotifyPropertyChanged
        {
            public string Foo { get; set; }
        }

        [Test]
        public void Test_RaisePropertyChangedForExpression()
        {
            ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            t.RaisePropertyChanged(() => t.Foo);

            Assert.That(notified.Count == 1);
            Assert.That(notified[0] == "Foo");
        }

        [Test]
        public void Test_RaisePropertyChangedForName()
        {
            ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            t.RaisePropertyChanged("Foo");

            Assert.That(notified.Count == 1);
            Assert.That(notified[0] == "Foo");
        }

        [Test]
        public void Test_RaisePropertyChangedDirect()
        {
            ClearAll();
            var dispatcher = new InlineMockMainThreadDispatcher();
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.That(notified.Count == 1);
            Assert.That(notified[0] == "Foo");
        }

        [Test]
        public void Test_TurnOffUIThread()
        {
            ClearAll();
            var dispatcher = new CountingMockMainThreadDispatcher();
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            t.ShouldAlwaysRaiseInpcOnUserInterfaceThread(false);
            t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.That(dispatcher.Count == 0);
            Assert.That(notified.Count == 1);
            Assert.That(notified[0] == "Foo");

            t.ShouldAlwaysRaiseInpcOnUserInterfaceThread(true);
            t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.AreEqual(1, dispatcher.Count);
            Assert.That(notified.Count == 1);
            Assert.That(notified[0] == "Foo");
        }

        public class Interceptor : IMvxInpcInterceptor
        {
            public Func<IMvxNotifyPropertyChanged, PropertyChangedEventArgs, MvxInpcInterceptionResult> Handler;

            public MvxInpcInterceptionResult Intercept(IMvxNotifyPropertyChanged sender, PropertyChangedEventArgs args)
            {
                return Handler(sender, args);
            }
        }

        [Test]
        public void Test_Interceptor()
        {
            ClearAll();

            var dispatcher = new CountingMockMainThreadDispatcher();
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var interceptor = new Interceptor();
            Ioc.RegisterSingleton<IMvxInpcInterceptor>(interceptor);

            var notified = new List<string>();
            var t = new TestInpc();
            t.PropertyChanged += (sender, args) => notified.Add(args.PropertyName);
            interceptor.Handler = (s, e) => MvxInpcInterceptionResult.RaisePropertyChanged;
            t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.That(dispatcher.Count == 1);

            interceptor.Handler = (s, e) => MvxInpcInterceptionResult.DoNotRaisePropertyChanged;
            t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.That(dispatcher.Count == 1);

            interceptor.Handler = (s, e) => MvxInpcInterceptionResult.RaisePropertyChanged;
            t.RaisePropertyChanged(new PropertyChangedEventArgs("Foo"));

            Assert.That(dispatcher.Count == 2);
        }
    }
}