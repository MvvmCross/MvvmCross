using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using MvvmCross.Core;
using MvvmCross.Core.Platform;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Platform;
using MvvmCross.Test.Core;
using NUnit.Framework;

[TestFixture]
public class WeaverTests
{
    Assembly assembly;
    string newAssemblyPath;
    string assemblyPath;

    [OneTimeSetUp]
    public void Setup()
    {
        assemblyPath = Path.GetFullPath("/users/will/Documents/Projects/MvvmCross/Weavers/MvxMainThread.AssemblyToProcess/bin/Debug/MvxMainThread.AssemblyToProcess.dll");

        //var projectPath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"../../../MvvmCross.Weavers.AssembliesToProcess\MvvmCross.Weavers.AssembliesToProcess.csproj123"));
        //assemblyPath = Path.Combine(Path.GetDirectoryName(projectPath), @"bin/Debug/MvvmCross.Weavers.AssembliesToProcess.dll");
#if (!DEBUG)
        assemblyPath = assemblyPath.Replace("Debug", "Release");
#endif

        newAssemblyPath = assemblyPath.Replace(".dll", "Woven.dll");
        File.Copy(assemblyPath, newAssemblyPath, true);

        var moduleDefinition = ModuleDefinition.ReadModule(assemblyPath);
        var weavingTask = new ModuleWeaver
        {
            ModuleDefinition = moduleDefinition
        };

        weavingTask.Execute();
        moduleDefinition.Write(newAssemblyPath);

        assembly = Assembly.LoadFile(newAssemblyPath);

        if (MvxSingleton<IMvxIoCProvider>.Instance != null) return;

        MvxSingletonCache.Initialize();
        MvxSimpleIoCContainer.Initialize();
        var dispatcher = new CountingDispatcher();
        var iocProvider = MvxSingleton<IMvxIoCProvider>.Instance;

        iocProvider.RegisterSingleton(iocProvider);
        iocProvider.RegisterSingleton<IMvxTrace>(new TestTrace());
        iocProvider.RegisterSingleton<IMvxSettings>(new MvxSettings());
        iocProvider.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

        MvxTrace.Initialize();
    }

    public class CountingDispatcher : MvxSingleton<IMvxMainThreadDispatcher>, IMvxMainThreadDispatcher
    {
        public int Calls { get; private set; }

        public bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            Calls++;
            action();
            return true;
        }
    }

    [Test]
    public void MarshallsToTheMainThread()
    {
        var dispatcher = Mvx.Resolve<IMvxMainThreadDispatcher>() as CountingDispatcher;
        var calls = dispatcher.Calls;

        var instance = (dynamic)Activator.CreateInstance(assembly.GetType(nameof(ToBeWoven)));
        instance.SomeWovenMainThreadMethod();

        Assert.Greater(dispatcher.Calls, calls);
    }
}