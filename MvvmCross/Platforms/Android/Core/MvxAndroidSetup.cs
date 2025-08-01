// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Android.Content;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Converters;
using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.IoC;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Core;

public abstract class MvxAndroidSetup
    : MvxSetup, IMvxAndroidGlobals, IMvxAndroidSetup
{
    private MvxCurrentTopActivity? _currentTopActivity;
    private IMvxAndroidViewPresenter? _presenter;

    public void PlatformInitialize(Application application)
    {
        ArgumentNullException.ThrowIfNull(application);

        ApplicationContext = application;

        if (_currentTopActivity != null)
            return;

        _currentTopActivity = new MvxCurrentTopActivity();
        application.RegisterActivityLifecycleCallbacks(_currentTopActivity);
    }

    public virtual Assembly ExecutableAssembly => ViewAssemblies.FirstOrDefault() ?? GetType().Assembly;

    public Context? ApplicationContext { get; private set; }

    protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        InitializeLifetimeMonitor(iocProvider);
        InitializeAndroidCurrentTopActivity(iocProvider);
        RegisterPresenter(iocProvider);

        iocProvider.RegisterSingleton<IMvxAndroidGlobals>(this);

        var intentResultRouter = new MvxIntentResultSink();
        iocProvider.RegisterSingleton<IMvxIntentResultSink>(intentResultRouter);
        iocProvider.RegisterSingleton<IMvxIntentResultSource>(intentResultRouter);

        var viewModelTemporaryCache = new MvxSingleViewModelCache();
        iocProvider.RegisterSingleton<IMvxSingleViewModelCache>(viewModelTemporaryCache);

        var viewModelMultiTemporaryCache = new MvxMultipleViewModelCache();
        iocProvider.RegisterSingleton<IMvxMultipleViewModelCache>(viewModelMultiTemporaryCache);
        base.InitializeFirstChance(iocProvider);
    }

    protected virtual void InitializeAndroidCurrentTopActivity(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        var currentTopActivity = CreateAndroidCurrentTopActivity();
        iocProvider.RegisterSingleton(currentTopActivity);
    }

    protected virtual IMvxAndroidCurrentTopActivity CreateAndroidCurrentTopActivity()
    {
        if (_currentTopActivity == null)
            throw new InvalidOperationException($"Please call {nameof(PlatformInitialize)} first");

        return _currentTopActivity;
    }

    protected virtual void InitializeLifetimeMonitor(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        var lifetimeMonitor = CreateLifetimeMonitor();

        iocProvider.RegisterSingleton<IMvxAndroidActivityLifetimeListener>(lifetimeMonitor);
        iocProvider.RegisterSingleton<IMvxLifetime>(lifetimeMonitor);
    }

    protected virtual MvxAndroidLifetimeMonitor CreateLifetimeMonitor()
    {
        return new MvxAndroidLifetimeMonitor();
    }

    protected virtual void InitializeSavedStateConverter(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        var converter = CreateSavedStateConverter();
        iocProvider.RegisterSingleton(converter);
    }

    protected virtual IMvxSavedStateConverter CreateSavedStateConverter()
    {
        return new MvxSavedStateConverter();
    }

    protected override IMvxViewsContainer CreateViewsContainer(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        if (ApplicationContext == null)
            throw new InvalidOperationException("Cannot create Views Container without ApplicationContext");

        var container = CreateViewsContainer(ApplicationContext);
        iocProvider.RegisterSingleton<IMvxAndroidViewModelRequestTranslator>(container);
        iocProvider.RegisterSingleton<IMvxAndroidViewModelLoader>(container);
        if (container is not MvxViewsContainer viewsContainer)
            throw new MvxException("CreateViewsContainer must return an MvxViewsContainer");
        return viewsContainer;
    }

    protected virtual IMvxAndroidViewsContainer CreateViewsContainer(Context applicationContext)
    {
        return new MvxAndroidViewsContainer(applicationContext);
    }

    protected IMvxAndroidViewPresenter Presenter
    {
        get
        {
            _presenter ??= CreateViewPresenter();
            return _presenter;
        }
    }

    protected virtual IMvxAndroidViewPresenter CreateViewPresenter()
    {
        return new MvxAndroidViewPresenter(AndroidViewAssemblies);
    }

    protected override IMvxViewDispatcher CreateViewDispatcher()
    {
        return new MvxAndroidViewDispatcher(Presenter);
    }

    protected virtual void RegisterPresenter(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        var presenter = Presenter;
        iocProvider.RegisterSingleton(presenter);
        iocProvider.RegisterSingleton<IMvxViewPresenter>(presenter);
    }

    protected override void InitializeLastChance(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        InitializeSavedStateConverter(iocProvider);
        base.InitializeLastChance(iocProvider);
    }

    [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
    protected override void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        var bindingBuilder = CreateBindingBuilder();
        bindingBuilder.DoRegistration(iocProvider);
    }

    [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
    protected virtual MvxBindingBuilder CreateBindingBuilder()
    {
        return new MvxAndroidBindingBuilder(FillValueConverters, FillValueCombiners, FillTargetFactories,
            FillBindingNames, FillViewTypes, FillAxmlViewTypeResolver, FillNamespaceListViewTypeResolver);
    }

    [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
    protected virtual void FillViewTypes(IMvxTypeCache cache)
    {
        ArgumentNullException.ThrowIfNull(cache);

        foreach (var assembly in AndroidViewAssemblies)
        {
            cache.AddAssembly(assembly);
        }
    }

    protected virtual void FillBindingNames(IMvxBindingNameRegistry registry)
    {
        // this base class does nothing
    }

    protected virtual void FillAxmlViewTypeResolver(IMvxAxmlNameViewTypeResolver viewTypeResolver)
    {
        ArgumentNullException.ThrowIfNull(viewTypeResolver);

        foreach (var kvp in ViewNamespaceAbbreviations)
        {
            viewTypeResolver.ViewNamespaceAbbreviations[kvp.Key] = kvp.Value;
        }
    }

    protected virtual void FillNamespaceListViewTypeResolver(IMvxNamespaceListViewTypeResolver viewTypeResolver)
    {
        ArgumentNullException.ThrowIfNull(viewTypeResolver);

        foreach (var viewNamespace in ViewNamespaces)
        {
            viewTypeResolver.Add(viewNamespace);
        }
    }

    [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
    protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
    {
        ArgumentNullException.ThrowIfNull(registry);

        registry.Fill(ValueConverterAssemblies);
        registry.Fill(ValueConverterHolders);
    }

    protected virtual void FillValueCombiners(IMvxValueCombinerRegistry registry)
    {
        // this base class does nothing
    }

    protected virtual IEnumerable<Type> ValueConverterHolders => new List<Type>();

    protected virtual IEnumerable<Assembly> ValueConverterAssemblies
    {
        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        get
        {
            var toReturn = new List<Assembly>();
            toReturn.AddRange(GetViewModelAssemblies());
            toReturn.AddRange(GetViewAssemblies());
            return toReturn;
        }
    }

    protected virtual IDictionary<string, string> ViewNamespaceAbbreviations => new Dictionary<string, string>
    {
        { "Mvx", "mvvmcross.platforms.android.binding.views" }
    };

    protected virtual IEnumerable<string> ViewNamespaces => new List<string>
    {
        "Android.Views",
        "Android.Widget",
        "Android.Webkit",
        "MvvmCross.Platforms.Android.Views",
        "MvvmCross.Platforms.Android.Binding.Views"
    };

    protected virtual IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>()
    {
        typeof(View).Assembly,
        typeof(MvxDatePicker).Assembly,
        GetType().Assembly,
    };

    protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
    {
        // nothing to do in this base class
    }

    protected override IMvxNameMapping CreateViewToViewModelNaming()
    {
        return new MvxPostfixAwareViewToViewModelNameMapping("View", "Activity", "Fragment");
    }
}

public abstract class MvxAndroidSetup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApplication> : MvxAndroidSetup
    where TApplication : class, IMvxApplication, new()
{
    protected override IMvxApplication CreateApp(IMvxIoCProvider iocProvider) =>
        iocProvider.IoCConstruct<TApplication>();

    [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
    public override IEnumerable<Assembly> GetViewModelAssemblies()
    {
        return [typeof(TApplication).GetTypeInfo().Assembly];
    }
}
