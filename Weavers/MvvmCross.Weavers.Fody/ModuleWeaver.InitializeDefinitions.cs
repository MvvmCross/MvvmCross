using System;
using System.Linq;
using Mono.Cecil;

public partial class ModuleWeaver
{
    private void InitializeDefinitions()
    {
        //Assemblies
        _mvvmCrossPlatform = FindAssembly(MvvmCrossPlatformAssemblyName);
        _mvvmCrossWeaversMainThread = FindAssembly(MvvmCrossWeaversMainThreadAssemblyName);

        //Types
        _void = ModuleDefinition.TypeSystem.Void;
        _bool = ModuleDefinition.TypeSystem.Boolean;

        _mainThreadDispatcher = new TypeReference(
            MvvmCrossPlatformCoreNamespaceName,
            MvxMainThreadDispatcherName, ModuleDefinition, _mvvmCrossPlatform);

        _mvx = new TypeReference(
            MvvmCrossPlatformAssemblyName, MvxTypeName,
            ModuleDefinition, _mvvmCrossPlatform);

        _action = new TypeReference(
            SystemAssemblyName,
            ActionTypeName,
            ModuleDefinition, ModuleDefinition.TypeSystem.CoreLibrary);

        _mainThreadAttribute = new TypeReference(
            MvvmCrossWeaversMainThreadAssemblyName,
            RunOnMainThreadAttributeTypeName,
            ModuleDefinition, _mvvmCrossWeaversMainThread);

        _mvxMainThreadDispatchingObject = new TypeReference(
            MvvmCrossPlatformCoreNamespaceName,
            MvxMainThreadDispatchingObjectName,
            ModuleDefinition, _mvvmCrossPlatform);

        //Methods
        _mvxResolve = new GenericInstanceMethod(new MethodReference(MvxResolveName, _mainThreadDispatcher, _mvx));
        _mvxResolve.GenericArguments.Add(_mainThreadDispatcher);

        _actionConstructor = ModuleDefinition.ImportReference(typeof(Action).GetConstructors().Single());

        _invokeOnMainThread = new MethodReference(
            InvokeOnMainThreadName,
            _void, _mvxMainThreadDispatchingObject) { HasThis = true };
        _invokeOnMainThread.Parameters.Add(new ParameterDefinition(_action));

        _requestMainThreadAction = new MethodReference(
            RequestMainThreadActionName,
            _bool, _mainThreadDispatcher) { HasThis = true };
        _requestMainThreadAction.Parameters.Add(new ParameterDefinition(_action));
        _requestMainThreadAction.Parameters.Add(new ParameterDefinition(_bool));
    }
}