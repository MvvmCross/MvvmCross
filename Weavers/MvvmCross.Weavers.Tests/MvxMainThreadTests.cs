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
    private Assembly wovenAssembly;

    [OneTimeSetUp]
    public void Setup()
    {
        var assemblyPath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"../../../MvxMainThread.AssemblyToProcess/bin/Debug/MvxMainThread.AssemblyToProcess.dll"));
#if (!DEBUG)
        assemblyPath = assemblyPath.Replace("Debug", "Release");
#endif

        var wovenAssemblyPath = assemblyPath.Replace(".dll", "Woven.dll");
        File.Copy(assemblyPath, wovenAssemblyPath, true);

        var moduleDefinition = ModuleDefinition.ReadModule(assemblyPath);
        var weavingTask = new ModuleWeaver { ModuleDefinition = moduleDefinition };
       
        weavingTask.Execute();
        moduleDefinition.Write(wovenAssemblyPath);

        wovenAssembly = Assembly.LoadFile(wovenAssemblyPath);

        InitializeSingletonsIfNeeded();
    }

    private void InitializeSingletonsIfNeeded()
    {
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

    [Test]
    public void MethodsAnnotedWithTheRunOnMainThreadAttributeCallTheMainThreadDispatcher()
    {
        var dispatcher = Mvx.Resolve<IMvxMainThreadDispatcher>() as CountingDispatcher;
        var calls = dispatcher.Calls;

        var instance = (dynamic)Activator.CreateInstance(wovenAssembly.GetType(nameof(ToBeWoven)));
        instance.SomeWovenMainThreadMethod();

        Assert.Greater(dispatcher.Calls, calls);
    }
}