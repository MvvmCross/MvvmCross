// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using MvvmCross.Exceptions;
using MvvmCross.Plugin;
using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.Plugin;
using Xunit;

namespace MvvmCross.UnitTest.Plugin
{
    public static class MvxPluginManagerTests
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

        [Collection("MvxTest")]
        public class TheEnsurePluginLoadedMethod : MvxPluginManagerTest
        {
            [Fact]
            public void CallsLoadOnTheTypeProvidedIfItInheritsFromIMvxPlugin()
            {
                var type = typeof(PluginMock1);

                PluginManager.EnsurePluginLoaded(type);

                Assert.Equal(1, PluginMock1.LoadCount);
            }

            [Fact]
            public void DoesNotCallLoadTwiceIfThePluginIsAlreadyLoaded()
            {
                var type = typeof(PluginMock2);

                PluginManager.EnsurePluginLoaded(type);
                PluginManager.EnsurePluginLoaded(type);

                Assert.Equal(1, PluginMock2.LoadCount);
            }

            [Fact]
            public void CallsLoadTwiceIfYouPassTheForceLoadParameter()
            {
                var type = typeof(PluginMock7);

                PluginManager.EnsurePluginLoaded(type);
                PluginManager.EnsurePluginLoaded(type, true);

                Assert.Equal(2, PluginMock7.LoadCount);
            }

            [Fact]
            public void ThrowsIfTheTypeIsNotAValidIMvxPlugin()
            {
                var type = typeof(Int32);

                Action callingEnsurePluginLoadedWithInvalidType =
                    () => PluginManager.EnsurePluginLoaded(type);

                Assert.Throws<MvxException>(callingEnsurePluginLoadedWithInvalidType);
            }
        }

        [Collection("MvxTest")]
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

        [Collection("MvxTest")]
        public class TheTryEnsurePluginLoadedMethod : MvxPluginManagerTest
        {
            [Fact]
            public void CallsLoadOnTheTypeProvidedIfItInheritsFromIMvxPluginAndReturnsTrue()
            {
                var type = typeof(PluginMock5);

                var isLoaded = PluginManager.TryEnsurePluginLoaded(type);

                Assert.Equal(1, PluginMock5.LoadCount);
                Assert.True(isLoaded);
            }

            [Fact]
            public void DoesNotCallLoadTwiceIfThePluginIsAlreadyLoadedButStillReturnsTrue()
            {
                var type = typeof(PluginMock6);

                PluginManager.TryEnsurePluginLoaded(type);
                var isLoaded = PluginManager.TryEnsurePluginLoaded(type);

                Assert.Equal(1, PluginMock6.LoadCount);
                Assert.True(isLoaded);
            }

            [Fact]
            public void CallsLoadTwiceIfYouPassTheForceLoadParameter()
            {
                var type = typeof(PluginMock8);

                PluginManager.TryEnsurePluginLoaded(type);
                PluginManager.TryEnsurePluginLoaded(type, true);

                Assert.Equal(2, PluginMock8.LoadCount);
            }

            [Fact]
            public void DoesNotThrowsIfTheTypeIsNotAValidIMvxPlugin()
            {
                var type = typeof(Int32);

                var isLoaded = PluginManager.TryEnsurePluginLoaded(type);

                Assert.False(isLoaded);
            }
        }

        [Collection("MvxTest")]
        public class TheLoadedPluginsProperty : MvxPluginManagerTest
        {
            [Fact]
            public void ReturnsAListOfAllLoadedPlugins()
            {
                var typesToLoad = new[] { typeof(PluginMock9), typeof(PluginMock10) };
                foreach (var type in typesToLoad)
                {
                    PluginManager.EnsurePluginLoaded(type);
                }

                Assert.Equal(2, PluginManager.LoadedPlugins.Count());
            }
        }
    }
}
