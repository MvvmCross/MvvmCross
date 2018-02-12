using System;
using MvvmCross.Exceptions;
using MvvmCross.Plugin;
using MvvmCross.Test;
using MvvmCross.UnitTest.Mocks.Plugin;
using Xunit;

namespace MvvmCross.UnitTest.Plugin
{
    public class MvxPluginManagerTests
    {
        public abstract class MvxPluginManagerTest : MvxIoCSupportingTest
        {
            protected MvxPluginManager PluginManager { get; }

            protected MvxPluginManagerTest()
            {
                PluginManager = new MvxPluginManager(ConfigurationSource);

                Setup();
            }

            protected IMvxPluginConfiguration ConfigurationSource(Type arg) => null;
        }

        public class TheEnsurePluginLoadedMethod : MvxPluginManagerTest
        {
            [Fact]
            public void CallsLoadOnTheTypeProvidedIfItInheritsFromIMvxPlugin()
            {
                var type = typeof(PluginMock1);

                PluginManager.EnsurePluginLoaded(type);

                Assert.Equal(PluginMock1.LoadCount, 1);
            }

            [Fact]
            public void DoesNotCallLoadTwiceIfThePluginIsAlreadyLoaded()
            {
                var type = typeof(PluginMock2);

                PluginManager.EnsurePluginLoaded(type);
                PluginManager.EnsurePluginLoaded(type);

                Assert.Equal(PluginMock2.LoadCount, 1);
            }

            [Fact]
            public void ThrowsIfTheTypeIsNotAValidIMvxPlugin()
            {
                var type = typeof(Int32);

                Action callingEnsurePluginLoadedWithInvalidType =
                    () =>  PluginManager.EnsurePluginLoaded(type);
                
                Assert.Throws<MvxException>(callingEnsurePluginLoadedWithInvalidType);
            }
        }

        public class TheIsPluginLoadedMethod : MvxPluginManagerTest
        {
            [Fact]
            public void ReturnsTrueIfThePluginHasAlreadyBeenLoaded()
            {
                var type = typeof(PluginMock3);

                PluginManager.EnsurePluginLoaded(type);

                Assert.True(PluginManager.IsPluginLoaded(type));
            }

            [Fact]
            public void ReturnsFalseIfThePluginHasNotBeenLoaded()
            {
                var type = typeof(PluginMock4);

                Assert.False(PluginManager.IsPluginLoaded(type));
            }
        }

        public class TheTryEnsurePluginLoadedMethod : MvxPluginManagerTest
        {
            [Fact]
            public void CallsLoadOnTheTypeProvidedIfItInheritsFromIMvxPluginAndReturnsTrue()
            {
                var type = typeof(PluginMock5);

                var isLoaded = PluginManager.TryEnsurePluginLoaded(type);

                Assert.Equal(PluginMock5.LoadCount, 1);
                Assert.True(isLoaded);
            }

            [Fact]
            public void DoesNotCallLoadTwiceIfThePluginIsAlreadyLoadedButStillReturnsTrue()
            {
                var type = typeof(PluginMock6);

                PluginManager.TryEnsurePluginLoaded(type);
                var isLoaded = PluginManager.TryEnsurePluginLoaded(type);

                Assert.Equal(PluginMock6.LoadCount, 1);
                Assert.True(isLoaded);
            }

            [Fact]
            public void DoesNotThrowsIfTheTypeIsNotAValidIMvxPlugin()
            {
                var type = typeof(Int32);

                var isLoaded = PluginManager.TryEnsurePluginLoaded(type);

                Assert.False(isLoaded);
            }
        }
    }
}
