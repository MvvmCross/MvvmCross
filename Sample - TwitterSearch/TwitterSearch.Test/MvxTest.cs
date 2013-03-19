using System;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Test.Core;
using Cirrious.MvvmCross.Views;
using NUnit.Framework;
using TwitterSearch.Test.Mocks;

namespace TwitterSearch.Test
{
    public class MvxTest : MvxIoCSupportingTest
    {
        protected override void AdditionalSetup()
        {
            Ioc.RegisterSingleton<IMvxMainThreadDispatcherProvider>(new MockMvxMainThreadDispatcherProvider());
            base.AdditionalSetup();
        }

        protected MockMvxViewDispatcher CreateMockNavigation()
        {
            var mockNavigation = new MockMvxViewDispatcher();
            var mockNavigationProvider = new MockMvxViewDispatcherProvider();
            mockNavigationProvider.ViewDispatcher = mockNavigation;
            Ioc.RegisterSingleton<IMvxViewDispatcherProvider>(mockNavigationProvider);
            return mockNavigation;
        }
    }
}