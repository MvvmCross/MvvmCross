---
title: "Customize app"
excerpt: ""
---
In each deployed MvvmCross application there are two key classes which control how your app starts:

- the `App` class in the core project - which provides the initialization for your core business logic and your viewmodels.
- the `Setup` class in the native UI project - this `Setup` is a bootstrapper for the MvvmCross system and for your app.

## App.cs

Typically App.cs provides only initialization of:

- simple rule-based IoC registration - e.g.:
 
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

- the `ViewModelLocator` - how `ViewModel`s are found or created when `View`s are displayed
- the `IMvxAppStart` - which `ViewModel` or `ViewModel`s are shown when the application is first started

## Setup.cs

Internally the `Setup` bootstrapper performs many steps. 

You can see most of them in the MvxSetup.cs class source which includes a sequence like this:

            // IoC
            InitializeIoC();
         
            // Core components
            InitializeFirstChance();
            InitializeDebugServices();
            InitializePlatformServices();
            InitializeSettings();
            InitializeSingletonCache();
            
            // Second components
            PerformBootstrapActions();
            InitializeStringToTypeParser();
            InitializeViewModelFramework();
            var pluginManager = InitializePluginFramework();
            InitializeApp(pluginManager);
            InitialiseViewModelTypeFinder();
            InitializeViewsContainer();
            InitiaiseViewDispatcher();
            InitializeViewLookup();
            InitialiseCommandCollectionBuilder();
            InitializeNavigationSerializer();
            InitializeInpcInterception();
            InitializeLastChance();

Most of these steps are `virtual` - so they allow customisation. Also most of these steps are implemented using virtual `Create` steps - which again should make customisation easier:
           
            protected virtual void InitialiseFoo()
            {
               var foo = CreateFoo();
               Mvx.RegisterSingleton<Foo>();
            }
            
            protected virtual IFoo CreateFoo()
            {
               return new Foo();
            }

Added to these base class steps, each platform adds a small number of platform specific steps - eg Android adds some additional methods and properties for initialisation of the Android UI and especially of the data-binding framework:

            string ExecutableNamespace { get; }
            Assembly ExecutableAssembly { get; }
            IMvxAndroidViewPresenter CreateViewPresenter()
            InitializeSavedStateConverter()
            InitialiseBindingBuilder()
            MvxAndroidBindingBuilder CreateBindingBuilder()
            RegisterBindingBuilderCallbacks()
            FillBindingNames(IMvxBindingNameRegistry registry)
            FillAxmlViewTypeResolver(IMvxAxmlNameViewTypeResolver viewTypeResolver)
            FillNamespaceListViewTypeResolver(IMvxNamespaceListViewTypeResolver viewTypeResolver)
            FillValueConverters(IMvxValueConverterRegistry registry)
            FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
            IList<string> ViewNamespaces { get; }
            IDictionary<string, string> ViewNamespaceAbbreviations { get; }
            List<Assembly> ValueConverterAssemblies { get; }
            List<Type> ValueConverterHolders { get; }
            IList<Assembly> AndroidViewAssemblies { get; }

This long list of virtual methods does provide a lot of opportunities for overriding default MvvmCross behaviour. 

However, **most applications actually override only very few of these methods**. Indeed, many applications only override only the one required method: `IMvxApplication CreateApp()`

If you do want to override some of the Setup, then the rest of this document describes some of the methods and properties that you may find useful.

#Individual customisations

##Providing application specific initialisation

There are three key methods where application specific initialisation might be added

1. `App.Initialize` - this is the place where all cross-platform app initialisation should occur. In general it is the **first choice** for all app-specific initialization. Only use the `Setup`-based methods if you need platform-specific code injected

1. `Setup.InitializeFirstChance` - a "first blood" placeholder for any steps you want to take before any of the later steps happen

1. `Setup.InitializeLastChance` - a "last ditch" placeholder for any steps you want to take after all of earlier steps have happened.  Note that the iOS and Android base classes configure binding within this method so you must call `base.InitializeLastChance` if you implement this method.

In this section, we'll only introduce some of the possibilities for creating and registering objects during initialize and setup. For a more in-depth introduction to MvvmCross IoC, see [Service Location and Inversion of Control](https://github.com/slodge/MvvmCross/wiki/Service-Location-and-Inversion-of-Control).

###Registering cross-platform business objects in `App.Initialize`

It's very common for `App.Initialize` to include IoC registration. Indeed, the default nuget `App.cs` template includes

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

This logic says:

- within the Assembly containing the `App`
- find all *creatable* `Type`s - meaning:
 - with `public` constuctors
 - not marked abstract
- find the interfaces they implement
- lazily register these types as singletons - meaning:
 - that no instance will be created until one is requested
 - that once the first instance has been created, then that same instance will be returned for all subsequent requests

Many other registration techniques are available - including registering objects from other assemblies, using instance-per-request registration and using individual rather than reflection-based registrations. For more on these, see [Service Location and Inversion of Control](https://github.com/slodge/MvvmCross/wiki/Service-Location-and-Inversion-of-Control).

###Registering platform-specific business objects in `Setup.InitializeFirstChance` and `Setup.InitializeLastChance`

These two placeholders provide key places for you to create and register services which are specific to your platform.

For example, if you wanted to implement an `EncryptionService` which would provide native data-encryption for your application, then you could do this during `Setup.InitializeFirstChance` using:

            Mvx.RegisterType<IEncryption, MyEncryption>();
       
This would then allow all of your `App` code - including code executed during `App.Initialize()` to use calls like:

            var encryption = Mvx.Resolve<IEncryption>();
            var safe = encryption.Encode(raw);

Alternatively, if you wanted to implement a `DialogService` which would be used during normal UI flow, then you might choose to register this during `Setup.InitializeLastChance` as:

            Mvx.RegisterSingleton<IDialogService>(new MyDialogService());

For many objects the choice of when to initialize - first or last - doesn't matter. For others, the key choice is whether the service needs to be available before or after the `App` is created and initialized.

##Changing trace/debug output

Each platform provides a virtual `CreateDebugTrace` methods which offers your application a chance to customise where `Mvx.Trace` messages are displayed.

To provide a custom trace implementation:

- first implement a class which provides `IMvxTrace`
- override `Setup.CreateDebugTrace()` in order to return an instead of your new class

One common use of this is simply to display messages to `Debug` using:

            public class MyDebugTrace : IMvxTrace
	{
		public void Trace(MvxTraceLevel level, string tag, Func<string> message)
		{
			Debug.WriteLine(tag + ":" + level + ":" + message());
		}

		public void Trace(MvxTraceLevel level, string tag, string message)
		{
			Debug.WriteLine(tag + ":" + level + ":" + message);
		}

		public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
		{
			try
			{
				Debug.WriteLine(string.Format(tag + ":" + level + ":" + message, args));
			}
			catch (FormatException)
			{
				Trace(MvxTraceLevel.Error, tag, "Exception during trace of {0} {1} {2}", level, message);
			}
		}
	}

this can be returned during `Setup` using:

            protected override IMvxTrace CreateDebugTrace() 
            { 
                        return new MyDebugTrace(); 
            }
            

##Changing the IoC container that MvvmCross uses

IoC is the first thing that MvvmCross setup starts.

This is done within the method `InitializeIoC` 

To override MvvmCross' IoC, you can:

- first find your alternative IoC implementation - e.g. something like AutoFac, Funq or TinyIoC
- then create an `Adapter` which maps the implementation behind an `IMvxIoCProvider` interface and which inhertis from `MvxSingleton<IMvxIoCProvider>` in order to provide a `Singleton`  
  - the majority of the adaption should be relatively straight-forwards - see `MvxSimpleIoCContainer` for how the default IoC container is provided.
  - The only *unusual* methods in the MvvmCross IoC interface are the `CallbackWhenRegistered` hooks - these provide callbacks when new object types are registered and may require a little custom code in the `RegisterXXX` methods within your adapter.
- finally, you can override `IMvxIoCProvider CreateIocProvider()` in your `Setup` class to return your IoC provider
 

##Providing additional View and ViewModel Assemblies

The default MvvmCross ViewModel Locator looks for:

- Views in the Assembly containing Setup.cs
- ViewModels in the Assembly containing App.cs

If you wish to add additional View and ViewModel assemblies, then you can do this using overrides in Setup:

        protected override Assembly[] GetViewAssemblies()
        {
            var list = new List<Assembly>();
            list.AddRange(base.GetViewAssemblies());
            list.Add(typeof(SomeTypeFromAdditionalViewAssembly1).Assembly);
            list.Add(typeof(SomeTypeFromAdditionalViewAssembly2).Assembly);
            list.Add(typeof(SomeTypeFromAdditionalViewAssembly3).Assembly);
            // ...
            return list.ToArray();
        }

        protected override Assembly[] GetViewModelAssemblies()
        {
            var list = new List<Assembly>();
            list.AddRange(base.GetViewModelAssemblies());
            list.Add(typeof(SomeTypeFromAdditionalViewModelAssembly1).Assembly);
            list.Add(typeof(SomeTypeFromAdditionalViewModelAssembly2).Assembly);
            list.Add(typeof(SomeTypeFromAdditionalViewModelAssembly3).Assembly);
            // ...
            return list.ToArray();
        }

**Note**: for WindowsPhone only, if you load Views from any Assembly other than the main one, then you need to add a `MvxPhoneViewAttribute` to each of those Views in order to help MvvmCross work out the Xaml Url for the View. For example, if the `SpecialLoginView` is a Xaml page located in the `SubViews` folder of the `MyOrg.Satellite.ExtraViews` assembly then you will need to add the attribute: `[MvxPhoneView("/MyOrg.Satellite.ExtraView;component/SubViews/BlockedUsers.xaml")]`

##Overriding ViewModel Location/Construction

By default, MvvmCross builds a new `ViewModel` every time one is requested and uses the `CIRS` sequence - Construction-Init-ReloadState-Start - to initialize that `ViewModel`.

If you want to override this behaviour for one or more `ViewModel` types, then you can do this in your `App` object by supplying your own `IMvxViewModelLocator` implementation.

For example, you could implement

	public class MyViewModelLocator
	  : MvxDefaultViewModelLocator
	{
	    private SpecialViewModel _special = new SpecialViewModel();
	
	    public override bool TryLoad(Type viewModelType, IDictionary<string, string> parameterValueLookup,
		                             out IMvxViewModel model)
	    {
	        if (viewModelType == typeof(SpecialViewModel))
	        {
	        	model = _special;
	        	return true;
	        }
	        else if (viewModelType == typeof(FooViewModel))
	        {
	        	model = new FooViewModel(_special);
	        	return true;
	        }
	        
	        return base.TryLoad(viewModelType, parameterValueLookup, out model);
	    }
	}

and could then return this in App.cs:

	protected override IMvxViewModelLocator CreateDefaultViewModelLocator()
	{
	    return new MyViewModelLocator();
	}

##Custom IMvxAppStart

When an MvvmCross application starts by default it shows the `View` associated with a single `ViewModel` type.

This default behaviour is configured in `Initialize` in `App.cs` using:

	RegisterAppStart<FirstViewModel>();
        
If more advanced startup logic is needed, then a custom app start can be used - e.g.

        public class CustomAppStart
            : MvxNavigatingObject
            , IMvxAppStart
        {
            public void Start(object hint = null)
            {
            	var auth = Mvx.Resolve<IAuth>();
                if (auth.Check())
                {
                    ShowViewModel<HomeViewModel>();
                }
                else
                {
                    ShowViewModel<LoginViewModel>();
                }
            }
        }

This can then be registered in `App` using:

	RegisterAppStart(new CustomAppStart());

**Note:** For situations where the app is launched using a `protocol` - e.g. from a Push notification or from an email link - then the `object hint` parameter start can be used to transfer a hint from the UI to the start object. Currently, it's up to you - the app developer - to write the UI side code to do this.

##Custom Presenters

For 'my first MvvmCross application' most people start with a 'full page' app in which each `ShowViewModel` causes a new `View` to be displayed, filling an entire screen at a time.

There are many other possibilities for ViewModel->ViewModel navigation, including:

- tabbed displays
- dialogs and flyouts
- 'hamburger menus'
- splitviews, master-detail displays, screen 'regions' and other screen division/fragmentation
 
To provide these alternative UI possibilities, MvvmCross allows each app to provide a custom presenter.

For more on custom presenters, see several articles and videos linked from: http://slodge.blogspot.co.uk/2013/06/presenter-roundup.html

##Providing ValueConverters

For the platforms that use MvvmCross' databinding platform, then ValueConverters:

- are classes which implement `IMvxValueConverter`
- are registered by name with a singleton `IMvxValueConverterRegistry`
 
There are several ways that individual ValueConverters can be registered. The most common are all in `Setup.cs`:

1. registering all value converters inside an assembly. To do this: override ValueConverterAssemblies:
2. registering value converters using a wrapper `Converters` class
3. registering value converters one at a time
 
These three techniques are shown as:

        // 1. register all value converters in the Assembly containing `MyFirstValueConverter`
        protected override List<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = base.ValueConverterAssemblies;
                toReturn.Add(typeof(MyFirstValueConverter).Assembly);
                return toReturn;
            }
        }

        // 2. register all value converters in the wrapper Converters
        public class Converters
        {
        	// this converter will be registered with name "TheLength"
        	public readonly LengthValueConverter TheLength = new LengthValueConverter();
        }
        
        protected override protected List<Type> ValueConverterHolders
        {
            get
            {
                var toReturn = base.ValueConverterHolders;
                toReturn.Add(typeof(Converters);
                return toReturn;
            }
        }

	// 3. register value converters one at a time
        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            
            registry.AddOrOverwrite("Foo", new SquareValueConverter());
        }

**Notes:**

- When using the `Assembly` based method then the registered name uses the rules:
 - remove any `Mvx` prefix
 - remove any `ValueConverter` or `Converter` suffix
 - because of this a value converter in a class `MvxBeerValueConverter` will be registered under the name `Beer`
- When using any of the registration mechanisms then the value converters are always ultimately added using a call to `registry.AddOrOverwrite` - so in the case of a name clash, then the last added value converter always wins.
 
One final technique used for registering value converters is used by some of the plugins to enable them to register value converters independently of the main setup flow.

This technique involves using the `CallbackWhenRegistered` IoC method on the `IMvxValueConverterRegistry` interface. This is used, for example, in the Visibility plugin as:
        
        Mvx.CallbackWhenRegistered<IMvxValueConverterRegistry>(RegisterValueConverters);
        
        // ...
        
        private void RegisterValueConverters()
        {
            var registry = Mvx.Resolve<IMvxValueConverterRegistry>();
            registry.AddOrOverwriteFrom(GetType().Assembly);
        }            


For more on creating ValueConverters, see the ValueConverter sample in: https://github.com/slodge/MvvmCross-Tutorials/tree/master/ValueConversion

##Providing Custom Views (Android)

In Android, MvvmCross overrides the default Android xml inflation and instead provides its own mechanism.

In order to do this, you must supply the MvvmCross binding system with a list of Assembly's in which it should look for Android `View` objects (note that these are Android `View` objects and have nothing to do with Mvvm `View`s at this point).

To do this, you can override the `Setup` property `AndroidViewAssemblies`:

        protected override IList<Assembly> AndroidViewAssemblies {
            get {
                return new List<Assembly>(base.AndroidViewAssemblies) {
                    typeof (CheeseBaron.ExcellentViews.Pages).Assembly,
                    typeof (AnyAdditionalViewType).Assembly
                };
            }
        }

Or as an expression body property in C# 6:

        protected override IList<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies) {
            typeof (CheeseBaron.ExcellentViews.Pages).Assembly,
            typeof (AnyAdditionalViewType).Assembly
        };

For more on writing Custom Views, see the N=18 and N=19 steps in N+1 - todo-link

## Registering Default Binding Names

In iOS Fluent Bindings you frequently see code like:

	set.Bind(myLabel).To(vm => vm.FullName);

This code uses a developer shortcut - the **default** binding property of a UILabel. This is just a shortcut and it means the binding is actually performed as:

	set.Bind(myLabel).For(label => label.Text).To(vm => vm.FullName);

At present the defined set of default properties or events includes:

            UIButton TouchUpInside
            UIBarButtonItem Clicked
            UITextField Text
            UITextView Text
            UITextField Text
            MvxCollectionViewSource ItemsSource
            MvxTableViewSource ItemsSource
            MvxImageView ImageUrl
            UIImageView	Image
            UIDatePicker Date
            UISlider Value
            UISwitch On
            UIDatePicker Date
            IMvxImageHelper ImageUrl
            MvxImageViewLoader ImageUrl

If you would like register additional shortcuts in your application, or if you'd like to replace some of the existing shortcuts, then this can be done in `FillBindingNames`

		protected override void FillBindingNames(IMvxBindingNameRegistry registry)
		{
			registry.AddOrOverwrite(typeof (MyControl), "MyDefaultPropertyOrEvent");
		}

##Registering custom bindings

MvvmCross binding works by default against Android and iOS UI objects by trying to use Reflection on properties and events.

This generally works really well, especially for cases where properties and events are linked using the convention that the property called `Foo` is associated with the change event `FooChanged`

For situations where this default binding doesn't work, custom bindings can be written and these can be registered using an override of the `FillTargetFactories` method in the Android and iOS `Setup` classes.

For more on this, see the N+1 video on custom binding - N=28.

##Overriding View-ViewModel associations

By default, MvvmCross discovers the ViewModel that a View is associated with using the type of ViewModel discovered by name convention

This makes prototyping initial application generally very functionality straight-forward.

However, as applications grow in size and complexity, then sometimes developers like to override this lookup behaviour.

To do this they can instead:

- provide a concrete type of the ViewModel where one is specified - e.g. as:
    
        public new DetailViewModel ViewModel
        {
            get { return base.ViewModel as DetailViewModel; }
            set { base.ViewModel = value; }
        }

- or provide an explicit type of the ViewModel specified using an `MvxViewForAttribute`
 
Further, in cases where every microsecond of startup time is essential, they can also help reduce the Reflection overhead by overriding the `InitializeViewLookup` method - e.g.

        protected override void InitializeViewLookup()
        {
            var viewModelViewLookup = new Dictionary<Type, Type>()
            {
            	{ typeof (FirstViewModel), typeof(FirstView) },
            	{ typeof (SecondViewModel), typeof(SecondView) },
            	//
            	{ typeof (UmpteenthViewModel), typeof(UmpteenthView) },
            };

            var container = Mvx.Resolve<IMvxViewsContainer>();
            container.AddAll(viewModelViewLookup);
        }