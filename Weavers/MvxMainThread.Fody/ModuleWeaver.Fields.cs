using System;
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
}