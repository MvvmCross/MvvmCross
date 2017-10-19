using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public partial class ModuleWeaver
{
    // Will log an MessageImportance.High message to MSBuild. OPTIONAL
    public Action<string> LogInfo { get; set; }

    // Will log an warning message to MSBuild. OPTIONAL
    public Action<string> LogWarning { get; set; }

    // Will log an error message to MSBuild. OPTIONAL
    public Action<string> LogError { get; set; }

    // Will log an warning message to MSBuild at a specific point in the code. OPTIONAL
    public Action<string, SequencePoint> LogWarningPoint { get; set; }

    // Will log an error message to MSBuild at a specific point in the code. OPTIONAL
    public Action<string, SequencePoint> LogErrorPoint { get; set; }

    private const string ReturnMustBeVoid = "You can only use the RunOnMainThreadAttribute on void methods";

    private const string MvxTypeName = "Mvx";
    private const string ActionTypeName = "Action";
    private const string MvxResolveName = "Resolve";
    private const string SystemAssemblyName = "System";
    private const string ConstructorMethodName = ".ctor";
    private const string InvokeOnMainThreadName = "InvokeOnMainThread";
    private const string MvvmCrossPlatformAssemblyName = "MvvmCross.Platform";
    private const string RequestMainThreadActionName = "RequestMainThreadAction";
    private const string MvxMainThreadDispatcherName = "IMvxMainThreadDispatcher";
    private const string MvvmCrossPlatformCoreNamespaceName = "MvvmCross.Platform.Core";
    private const string RunOnMainThreadAttributeTypeName = "RunOnMainThreadAttribute";
    private const string MvxMainThreadDispatchingObjectName = "MvxMainThreadDispatchingObject";
    private const string MvvmCrossWeaversMainThreadAssemblyName = "MvvmCross.Weavers.MainThread";

    public ModuleDefinition ModuleDefinition { get; set; }

    private static AssemblyNameReference _mvvmCrossPlatform;
    private static AssemblyNameReference _mvvmCrossWeaversMainThread;

    private TypeReference _mvx;
    private TypeReference _bool;
    private TypeReference _void;
    private TypeReference _action;
    private TypeReference _mainThreadAttribute;
    private TypeReference _mainThreadDispatcher;
    private TypeReference _mvxMainThreadDispatchingObject;

    private GenericInstanceMethod _mvxResolve;
    private MethodReference _actionConstructor;
    private MethodReference _invokeOnMainThread;
    private MethodReference _requestMainThreadAction;

    public void Execute()
    {
        Debug.WriteLine("Weaving file: " + ModuleDefinition.FileName);

        InitializeDefinitions();

        var typesToWeave =
            ModuleDefinition
                .GetTypes()
                .SelectMany(MethodsToWeave)
                .GroupBy(method => method.DeclaringType);

        foreach (var grouping in typesToWeave)
        {
            var declaringType = grouping.Key;
            
            foreach (var method in grouping.Where(ShouldWeave))
            {
                WeaveInvokeOnMainThread(method, declaringType);

                //Removes the reference to the RunOnMainThread attribute
                method.CustomAttributes.Remove(method.CustomAttributes
                    .First(a => a.AttributeType.Name == RunOnMainThreadAttributeTypeName));
            }
        }
    }

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

    private AssemblyNameReference FindAssembly(string assemblyName)
        => ModuleDefinition.AssemblyReferences.SingleOrDefault(a => a.Name == assemblyName);

    private IEnumerable<MethodDefinition> MethodsToWeave(TypeDefinition type)
        => type.Methods.Where(MethodHasMainThreadAttribute);

    private bool MethodHasMainThreadAttribute(MethodDefinition method)
        => method.CustomAttributes.Any(a => a.AttributeType.Name == RunOnMainThreadAttributeTypeName);
}