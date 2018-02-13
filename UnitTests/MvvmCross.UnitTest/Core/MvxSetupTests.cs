using System;
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
                var assemblies =
                    SetupUnderTest.Object.GetPluginAssemblies()
                        .Where(AssemblyIsNotProxy);
                
                //2 == MvvmCross.UnitTest && MvvmCross.Test
                Assert.Equal(assemblies.Count(), 2);

                //Remove assemblies added by Moq
                bool AssemblyIsNotProxy(Assembly assembly)
                    => assembly.GetName().Name != "DynamicProxyGenAssembly2";
            }
        }

        public class TheLoadPluginsMethod : MvxSetupPluginTest
        {
            [Fact]
            public void LoadssEveryTypeAnnotatedWithTheMvxSetupAttibute()
            {
                SetupUnderTest.Object.LoadPlugins(PluginManager.Object);

                PluginManager
                    .Verify(manager => manager.EnsurePluginLoaded(It.IsAny<Type>(), It.IsAny<bool>()), Times.Exactly(10));
            }
        }
    }
}
