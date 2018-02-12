using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;
using MvvmCross.Core;
using MvvmCross.Plugin;
using Xunit;

namespace MvvmCross.UnitTest.Core
{
    public class MvxSetupTests
    {
        public abstract class MvxSetupPluginTest
        {
            public Mock<IMvxPluginManager> PluginManager { get; } = new Mock<IMvxPluginManager>();

            protected Mock<MvxSetup> SetupUnderTest { get; }

            protected MvxSetupPluginTest()
            {
                SetupUnderTest  = new Mock<MvxSetup> { CallBase = true };
            }
        }

        public class TheGetPluginsAssemblyMethod : MvxSetupPluginTest
        {
            [Fact]
            public void ReturnsAListOfAllAssembliesThatReferenceMvvmCross()
            {
                var assemblies = SetupUnderTest.Object.GetPluginAssemblies();

                Assert.Equal(assemblies.Count(), 1);
            }
        }

        public class TheLoadPluginsMethod : MvxSetupPluginTest
        {
            [Fact]
            public void LoadssEveryTypeAnnotatedWithTheMvxSetupAttibute()
            {
                SetupUnderTest.Object.LoadPlugins(PluginManager.Object);

                PluginManager
                    .Verify(manager => manager.EnsurePluginLoaded(It.IsAny<Type>()), Times.Exactly(3));
            }
        }
    }
}
