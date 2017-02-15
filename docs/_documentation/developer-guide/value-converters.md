---
title: "Value converters"
excerpt: ""
---
Value Converters in MvvmCross are used to provide mappings to/from logical values in the view models and presented values in the user interface.

A Value Converter is any class which implements the `IMvxValueConverter` interface

The `IMvxValueConverter` interface provides:

- a `Convert` method - for changing ViewModel values into View values
- a `ConvertBack` method - for changing View values into ViewModel values


        public interface IMvxValueConverter
        {
            object Convert(object value, Type targetType, object parameter, CultureInfo culture);
            object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
        }

For value converters which are used with non-editable UI fields (e.g. labels, images, etc), it is very common for Value Converters to implement **only** the `Convert` method - with the `ConvertBack` left as `throw new NotImplementedException();`

Within MvvmCross, we try to encourage the use of cross-platform value converters wherever possible - but it is also possible and straight-forward to implement platform specific value converters.
     
Note: this article assumes the reader has an understanding already of MvvmCross data-binding syntax - see [wiki/Databinding](https://github.com/slodge/MvvmCross/wiki/Databinding)

### ValueConverter Samples

For several good ValueConverter samples, including Strings, Dates, Colors, Visibility and Two-Way conversion, please see:

- Value Conversion sample - https://github.com/slodge/MvvmCross-Tutorials/tree/master/ValueConversion
- N+1 Video - http://slodge.blogspot.ca/2013/04/n4-valueconverters-n1-days-of-mvvmcross.html

### A first ValueConverter

To implement a ValueConverter from the 'raw interface' you could implement something like:

     public class PlusOneValueConverter : IMvxValueConverter
     {
         public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
         {
             return ((int)value) + 1;
         }
         
         public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
         {
             return ((int)value) - 1;
         }
     }
     
Notice how this 'raw' value converter provides you with `object`-level input parameters and output values.     

### Using the MvxValueConverter<TFrom,TTo> helper

While allowing you the maximum flexibility, the `object` level `IMvxValueConverter` is a little tedious to work with - it always requires you to cast your input `value` to the expected type and it requires you to declare both `Convert` and `ConvertBack` functions.

In many cases, the `MvxValueConverter<TFrom, TTo>` helper class can do this casting and can also provide default placeholder `NotImplemented` methods for you.

For example, a value converter for converting `DateTime`s in the ViewModel to 'time ago' `string`s in the View might look like:

    public class MyTimeAgoValueConverter : MvxValueConverter<DateTime, string>
    {
        protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            var timeAgo = DateTime.UtcNow - value;
            if (timeAgo.TotalSeconds < 30)
            {
            	return "just now";
            }
            
            if (timeAgo.TotalMinutes < 10)
            {
            	return "a few minutes ago";
            }
            
            if (timeAgo.TotalMinutes < 60)
            {
            	return "in the last hour";
            }
            
            if (timeAgo.TotalMinutes < 24*60)
            {
            	return "in the last day";
            }
            
            return "previously";            
        }
    }

Note:

- this class doesn't need to provide a `ConvertBack` override - but can if you need it to.
- there is also a less frequently used `MvxValueConverter<TFrom>` helper class which is equivalent to `MvxValueConverter<TFrom, object>` 


### Using the TargetType, CultureInfo, and Parameter arguments

The `Type targetType` parameter provides you with an indication of the type expected as the output of each call to the `Convert` or `ConvertBack` method. This isn't generally that useful when writing application specific value converters, but can be useful if you are writing general purpose value converters which you expect to be reused in a wide range of scenarios.

The `CultureInfo cultureInfo` parameter provides you with the culture used in the UI. Within MvvmCross binding this is always the current value of `System.Globalization.CultureInfo.CurrentUICulture`

The `object parameter` parameter is a general purpose field which you can use in your binding declarations. For example:

- you might choose to use a binding in Droid like:

      local:MvxBind="Value Power(CurrentValue, 2)"
      
  or:
 
      local:MvxBind="Value CurrentValue, Converter=Power, ConverterParameter=2"
  
  Both of these would result in the Power ValueConverter being passed a parameter value of `long` 2.
  
- you might choose to use a binding in Touch like:

        set.Bind(label).For(l => l.Text).To(vm => vm.FullName).WithConversion("AbbreviateIfLongerThan", 12L);
       
  or:
  
       set.Bind(label).Described("Text AbbreviateIfLongerThan(FullName, 12)");
  
  Both of these would result in the AbbreviateIfLongerThan ValueConverter being called with parameter of `long` 12

Note that the only types that are used when parameters are parsed from text binding descriptions are: `long`, `double`, `bool` or `string` (for more on the binding parsing engine, see [wiki/Databinding](https://github.com/slodge/MvvmCross/wiki/Databinding))
    

### Referencing Value Converters in Touch and Droid

Data-Binding syntax including how to specify ValueConverters using Swiss, Fluent and Tibet binding is discussed in [wiki/Databinding](https://github.com/slodge/MvvmCross/wiki/Databinding). This covers all syntax including:

- Swiss
   
        local:MvxBind="Value CurrentValue, Converter=Power, ConverterParameter=2"

- Tibet
   
        local:MvxBind="Value Power(CurrentValue, 2)"

- Fluent

        set.Bind(field)
           .For(l => l.Value)
           .To(vm => vm.CurrentValue)
           .WithConversion("Power", 2);

Within this syntax, ValueConverters are generally referred to by a name - e.g. `Power`

To find the specified ValueConverter, MvvmCross maintains a registry of ValueConverter instances indexed by name.

Typically, this registry is created using a one-pass Reflection sweep on your application's Core and UI (platform-specific) projects. This sweep:

- locates all instanciable classes which implement `IMvxValueConverter` within your assemblies
- creates an instance of each one
- registers the instance with the name stripped of any `Mvx` prefix and any `ValueConverter` or `Converter` postfix.

So, for example, the following class names will all be registered with the same ValueConverter name of "Foo":

- Foo
- FooValueConverter
- FooConverter
- MvxFooValueConverter
- MvxFooConverter

Where multiple classes with the same ValueConverter name are encountered, then the last one found will be the one which remains registered.

To prevent value converter classes being registered during this reflection sweep, you can use the `[MvxUnconventional]` attribute on the ValueConverter class.

To include additional assemblies in the ValueConverter reflection sweep, you can do this in your `Setup` class using an override of the `protected virtual List<Assembly> ValueConverterAssemblies { get; }` property - e.g.:

     protected override List<Assembly> ValueConverterAssemblies 
     {
      	get
      	{
      		var toReturn = base.ValueConverterAssemblies;
      		toReturn.Add(typeof(SomeValueConverter).Assembly);
      		return toReturn;
      	}
    }

To manually register additional value converters, you can do this in your `Setup` class using an override of the `FillValueConverters` method - e.g.

     protected override FillValueConverters (IMvxValueConverterRegistry registry)
     {
         base.FillValueConverters(registry);
         registry.AddOrOverwrite("CustomName", new MyVerySpecialValueConverter(42));
         registry.AddOrOverwrite("CustomName2", new AnotherVerySpecialValueConverter("Summer"));
     }

Finally, ValueConverters can also be registered using a technique called "ValueConverter holders". This technique uses Reflection against indidivual Types which then hold ValueConverters in public instance or static fields. This technique was common in earlier MvvmCross versions, but is not recommended within v3 - it's kept only for backwards compatability.


### Preventing the ValueConverter Reflection Sweeps in Touch and Droid

The ValueConverter sweeps do use a small amount of Reflection and so can add a very small amount of lag to application start time. If you'd prefer to minimise this small startup lag in your application, then you can, of course, disable the sweeps and can use direct registration instead.

To do this, override the `FillValueConverters` method in your `Setup` class, do not call the base class method and instead use just register your own value converters - e.g.

     protected override FillValueConverters (IMvxValueConverterRegistry registry)
     {
         // avoid the reflection overhead - do not call base class
         // base.FillValueConverters(registry);
         registry.AddOrOverwrite("Foo", new FooValueConverter());
         registry.AddOrOverwrite("Bar", new BarValueConverter());
     }

Note: unless your application is very large, this is most likely only a micro-optimisation and will most likely not significantly change your app's startup time.
 
### Using Value Converters in Windows (conventional Xaml binding)

The `IMvxValueConverter` interface is closely based on the `IValueConverter` interface used in Windows WPF and Silverlight Xaml binding. This interface is also similar (but slightly different) to the `IValueConverter` interface used in Windows WinRT Xaml binding. 

Because these Xaml `IValueConverter` interfaces are not 100% identical to each other, nor to the `IMvxValueConverter` version, shared Mvx ValueConverters cannot be used directly in Windows Xaml binding - they must instead be wrapped for use in Xaml.

The steps to do this are similar on each Windows platform:

- for each `IMvxValueConverter` class, e.g. for 
 
``` 
       public class TheTruthValueConverter 
           : MvxValueConverter<bool, string> 
       { 
       		public string Convert(bool value, Type targetType, CultureInfo cultureInfo, object parameter)
			{
				return value ? "Yay" : "Nay";
			} 
       }
```

- in your UI project, create a 'native' wrapper using the `MvxNativeValueConverter` class:

```
       public class TheNativeTruthValueConverter
       		: MvxNativeValueConverter<TheTruthValueConverter>
       {
       }
```

- in your Xaml, include an instance of your ValueConverter as a static resource - this can be done in the `Resources` at App, Page or Control Xaml level, e.g.:

```
       <converters:TheNativeTruthValueConverter x:Key="TheTruth" />
```
       
- now your converter can be used - e.g.:

```
       <TextBlock Text="{Binding HasAccepted, Converter={StaticResource TheTruth}}" />
```

### Using Value Converters in Windows (Tibet binding)

In addition to 'traditional' Xaml bindings, MvvmCross also allows 'Tibet' binding within Windows - for more on this see [wiki/Databinding](https://github.com/slodge/MvvmCross/wiki/Databinding).

When Tibet binding is used, then Value Converters can be accessed by name - exactly as in Droid and Touch binding - without the above native Xaml wrapping.

Further, if using 'Tibet' binding then an entire assembly's worth of value converters can be registered using the Reflection sweep technique and this can be specified at the Xaml level - meaning it can be used in both design and run-time.

To include all value converters within an Assembly at the Xaml level, then use an `mvx:Import` block with an inner `From` attribute which contains an instance of a class from that Assembly.

This may sound complicated… but actually it is quite simple. 

- Suppose you have an Assembly `MyTools` containing `FooValueConverter`, `BarValueConverter`, etc
- Within this Assembly add a simple, instanciable public Class which we will use only for the import - e.g. `public class MarkerClass {}`
- Then within the xaml, you can include a static resource import block like:

	     <mvx:Import x:Key="MvxAssemblyImport0">
	       <mvx:Import.From>
	         <myTools:MarkerClass />
	       <mvx:Import.From>
	     </mvx:Import>
     
- After this is done, then the ValueConverters `Foo` and `Bar` will be available for use within 'Tibet' bindings - e.g. as:

     	 <TextBlock mvx:Bi.nd="Text Foo(Name)" />

 
### Using platform-specific Value Converters

Most of the discussions so far have assumed that ValueConverters are placed in shared PCL code and are shared between platforms.

Beyond this, ValueConverters can also of course be used within UI projects for platform specific functionality. This is seen particularly often for things like loading images and resources or for performing platform specific layout adjustments.

### The Mvx Visibility ValueConverters

The Visibility plugin contains a couple of simple value converters which assist with `Visibility` on each platform.

These are `MvxVisibilityValueConverter` and `MvxInvertedVisibilityValueConverter` registered under the names "Visibility" and "InvertedVisibility".

On each platform, these allow ViewModel properties to be mapped to ui visibility properties. The basic logic of the Visibility test catches many common cases:

- `string` - any non-null non-empty string is `visible`
- `int` or `double` - any >0 value is `visible`
- `bool` - a `true` value is `visible`
- any other `Type` - a non-`null` value is visible

The logic of the "InvertedVisibility" is the inverse of this.

If you require other visibility logic - e.g. if you need a mapping for a `nullable<float>` to Visibility on each platform, then you can easily implement your own Visibility ValueConverter using the `MvxBaseVisibilityValueConverter` base class.

To use these converters on each platform, use:

- Droid (not that in Droid there is no support for the `Invisible` state - cross platform, we only support `Visible` and `Gone`):
  
        local:MvxBind="Visibility Visibility(VMProperty)"
        
- Touch:

        set.Bind(field)
           .For("Visibility")
           .To(vm => vm.VMProperty)
           .WithConversion("Visibility");
           
- Windows - use Native wrappers or Tibet Binding as described above:

        Visibility="{Binding VMProperty, Converter={StaticResource Visibility}}"

  or:
     
        mvx:Bi.nd="Visibility Visibility(VMProperty)"
  
Note: to use the Visibility converters at design-time on the Windows platforms, you can include the design-time helper objects - `MvxVisibilityDesignTimeHelper` - these can be used as:

        <visibility:MvxVisibilityDesignTimeHelper x:Key="DesignTimeVisibility" />

If you need to create your own Visibility ValueConverter's then the `MvxBaseVisibilityValueConverter<T>` and `MvxBaseVisibilityValueConverter` base classes can assist with this - e.g.:

	public class SayPleaseVisibilityValueConverter : MvxBaseVisibilityValueConverter<string>
	{
		protected override MvxVisibility Convert(string value, object parameter, CultureInfo culture)
                {
                    return (value == "Please) ? MvxVisibility.Visible : MvxVisibility.Collapsed;
                }
	}

**Note:** In addition to the `Visibility` properties that are available on all `UIElement`s within Xaml, MvvmCross also provides a `Visible` custom binding - allowing ViewModel properties of type `Boolean` to be bound directly to the UI visibility without using the `VisibilityConverter` - e.g.:

        mvx:Bi.nd="Visible VMProperty"


### The Mvx Color ValueConverters

The Color plugin contains a couple of simple value converters which assist with `MvxColor` on each platform.

These are:

- `MvxNativeColorValueConverter` registered as "NativeColor" - this converts `MvxColor` properties on the ViewModel to a native color format.
-  `MvxRGBAValueConverter` registered as "RGBA" - this converts Hex ViewModel string properties like "RGB", "RRGGBB" and "RRGGBBAA" with optional leading "#" characters into native color formats.
-  `MvxRGBIntColorConverter` registered under the name "RGBIntColor" - this converters `int` ViewModel properties into native color formats.

The Color plugin also provides a base class - `MvxColorValueConverter` which you can inherit from in order to define your own cross-platform Color converters.

On each platform, the native color format output from these converters is:

- Windows - a `SolidColorBrush` for the appropriate Windows flavor
- Droid - an `Android.Graphics.Color`
- Touch - an `MonoTouch.UIKit.UIColor`

On Droid, the Color plugin also includes a couple of custom bindings to assist with binding. These are:

- `TextView` `TextColor` binding
- `View` `BackgroundColor` binding

To use Color on each platform - for example, with a ViewModel property `public MvxColor CurrentColor { get; set; }` you can use:

- Droid:
  
        local:MvxBind="BackgroundColor NativeColor(CurrentColor)"
        
- Touch:

        set.Bind(field)
           .For(field => field.BackgroundColor)
           .To(vm => vm.CurrentColor)
           .WithConversion("NativeColor");
           
- Windows - use Native wrappers or Tibet Binding as described above:

        Fill="{Binding CurrentColor, Converter={StaticResource NativeColor}}"

  or:
     
        mvx:Bi.nd="Fill NativeColor(CurrentColor)"
          
Note: to use the Color converters at design-time on the Windows platforms, you can include the design-time helper objects - `MvxColorDesignTimeHelper` - these can be used as:

        <color:MvxColorDesignTimeHelper x:Key="DesignTimeColor" />


### The Mvx Language ValueConverter

The MvvmCross internationalisation (i18n) techniques are based on the JsonLocalisation plugin with `TextSource` properties in each ViewModel and with `mvxLang` binding attributes.

Under the covers, these `mvxLang` bindings are actually just normal bindings which make use of the `MvxLanguageValueConverter`. This is automatically registered using the name `Language`.
This consumes the `TextSource` as it's `value` and the `Key` as its `parameter`.

So a language binding:

     local:mvxLang='Text HelloWorld,TextSource=SharedTextSource'
     
Is actually equivalent to a normal binding:

     local:mvxBind='Text SharedTextSource, Converter=Language, ConverterParameter="HelloWorld", Mode=OneTime'
 
### Using internationalised text in your ValueConverters

Earlier we considered a `MyTimeAgoValueConverer` which returned strings like "just now" from it's `Convert` implementation,

If using the MvvmCross JsonLocalisation system, then that same value converter could be rewritten to make use of a `IMvxTextProvider` reference.

For example, it could be rewritten:

    public class MyTimeAgoValueConverter : MvxValueConverter<DateTime, string>
    {
        private IMvxTextProvider _textProvider;
        private IMvxTextProvider TextProvider
        {
            get
            {
                _textProvider = _textProvider ?? Mvx.Resolve<IMvxTextProvider>();
                return _textProvider;
            }
        }
            
        protected override string Convert(DateTime value, Type targetType, CultureInfo culruteInfo, object parameter)
        {
            var timeAgo = DateTime.UtcNow - value;
            var key = "unknown";
            
            if (timeAgo.TotalSeconds < 30)
            {
            	key = "just.now";
            }            
            else if (timeAgo.TotalMinutes < 10)
            {
            	key = "a.few.minutes.ago";
            }   
            else if (timeAgo.TotalMinutes < 60)
            {
            	key = "in.the.last.hour";
            }    
            else if (timeAgo.TotalMinutes < 24*60)
            {
            	key = "in.the.last.day";
            }
            else
            {
            	key = "previously";
            }
            
            return TextProvider.GetText(Constants.GeneralNamespace, Constants.TimeAgoStrings, key)        
        }
    }

Where `Constants.TimeAgoStrings` refers to JSON content loaded (for EN-US) as:

    {
    	'unknown':'oops',
    	'just.now':'just now',
    	'a.few.minutes.ago':'a few minutes ago',
    	'in.the.last.hour':'in the last hour',
    	'in.the.last.day':'in the last day',
    	'previously':'previously'
    }

### The Mvx CommandParameter ValueConverter

MvvmCross v3 added a special `CommandParameter` parse step to allow the binding to specify fixed values (`string`s, `long`s, `bool`s) to `ICommand` parameter bindings.

This can be used as:

        local:MvxBind="Click MyCommand,CommandParameter=Thursday"

which uses the `MvxCommandParameterValueConverter` ValueConverter to turn a Click into a call on:

       MyCommand.Execute("Thursday")

So this can be received in the ViewModel as:

       new MvxCommand(day => DoAction(day));
      
This helps where you want to reuse a single `ICommand` implementation across multiple buttons.

Please note that this cannot be used currently with other `ValueConverter`s - as it itself uses a `ValueConverter` to achieve it's effect.

### ValueConverters are evil?

Some Mvvm commentators have been known to say "ValueConverters are evil"

I'm not 100% sure why they've said this, but I believe the main reasons are based around:

- concerns with people hiding too much application logic within ValueConverters
- concerns with performance - especially in list situations where ValueConverters can be executed rapidly for every list item during scrolling.

I personally suspect these concerns are valid - that there can be situations where putting the conversion functionality directly into the ViewModels rather than into the ValueConverters can make apps easier to understand and can help avoid performance issues. However, I also suspect that these concerns are over-stated. I can't see any reason why a developer who makes use of value converters should be any more prone to architectural or performance problems than another deverloper who doesn't. Indeed I'd suspect the reverse.

In summary… **ValueConverters are good** - use them.


### ValueConverters and FallbackValues

When specifying a binding, you can also provide a `FallbackValue` - see [wiki/Databinding](https://github.com/slodge/MvvmCross/wiki/Databinding). This `FallbackValue` is used within the View:

- whenever the binding source path is missing - e.g. if you specify a Path of `Child.Property` and `Child` is currently `null`
- whenever the value converter throws an exception during the `Convert` conversion
  
Note that in 'normal binding' `FallbackValue`s are not themselves passed through the ValueConverter. So, this conversion is **not** correct:

     local:MvxBind="Text Visibility(IsEnabled), FalllbackValue=false"

If you do want to pass a `FallbackValue` through the Value Converter then you can do this using recursive binding syntax within the `Tibet` binding engine - e.g.

     local:MvxBind="Text Visibility((IsEnabled, FalllbackValue=false))"

### Tibet: ValueCombiners

Tibet binding (see [wiki/Databinding](https://github.com/slodge/MvvmCross/wiki/Databinding)) introduced a new interface into binding - `IMvxValueCombiner` - this interface allows multiple binding sources to be combined together within a single target expression. This interface is used in, for example, the `MvxFormatValueCombiner` in order to enable binding expressions like:

         local:MvxBind="Text Format('{0} {1} {2}', Greeting(Gender), FirstName, LastName)"
         
The rules and mechanisms for registering ValueCombiners are similar to those for registering ValueConverters. However, because combiners are not commonly declared in user code, MvvmCross doesn't current perform a Reflection sweep across your Core or UI assemblies. If you do want to add is used by default on the , then a ValueCombiner class named `FooValueCombiner` will be registered under the name `Foo`.
          
Please be aware that withing the MvvmCross Tibet binding syntax, ValueCombiners and ValueConverters share the same 'registered name' space - because both of them are expressed as 'functions' then it's impossible to have both a `Foo` ValueConverter and a `Foo` ValueCombiner - if both are registered then the ValueConverter will always be used rather than the ValueCombiner. 
          
The API for `IMvxValueCombiner` is significantly more complicated than `IMvxValueConverter` at present and it's tied to ImvxSubStep - which is part of the internal structure of the MvvmCross binding evaluation engine.

    public interface IMvxValueCombiner
    {
        Type SourceType(IEnumerable<IMvxSourceStep> steps);
        void SetValue(IEnumerable<IMvxSourceStep> steps, object value);
        bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value);
        IEnumerable<Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps, Type overallTargetType);
    }

To assist with authoring ValueCombiners, a number of helper classes are available including the base `MvxValueCombiner` class which provides default implementations for all methods in the interface.

An example ValueCombiner which counts the number of non-null inputs bound to it might be:

	public class CountingValueCombiner
		: MvxValueCombiner
	{
		public override Type SourceType (IEnumerable<IMvxSourceStep> steps)
		{
			return typeof(int);
		}
	
		public override System.Boolean TryGetValue (IEnumerable<IMvxSourceStep> steps, out Object value)
		{
			var count = 0;
			foreach (var input in steps) 
			{
				object innerResult;
				if (!input.TryGetValue (out innerResult)) 
				{
					// one of our input bindings is missing so we can't work out our answer
					value = null;
					return false;
				}

				if (innerResult != null)
					count++;
			}

			value = count;
			return true;
		}
	}

This could be used in a binding to count (for example) how many peoples have been picked for a band:

    local:MvxBind="Text Counting(Guitarist, Drummer, Bass, Vocalist)"

Note that it's unusual for a ValueCombiner to meaningfully implement `SetValue` - this is because it's unusual (but not unheard of) for multi-bindings to support updating of the multiple source elements from changes in the View.

Developers are very welcome to write their own ValueCombiners if they wish to - please do - but please also be aware that it's likely that this internal `IMvxValueCombiner` API will change in future MvvmCross revisions - we are looking at ways to either simplify this Tibet binding interface and/or ways to make the binding structure more Type-aware so that conversions can be performed at more places within the binding engine. (Developers are also very welcome to suggest improvements for this API!)


### Available ValueCombiners

**TODO** - this section is draft - a work in progress.

The 'standard' ValueCombiners available in MvvmCross are:

- `If` - used for if-else conditional display with syntax 

        If(booleab-test, value-if-true, value-if-false)

   For example:

        If(HasProAccount, ExtendedName, PromotionalMessage)

- `Format` - used for displaying strings using the standard C# CLR `string.Format` syntax.

        Format(format-string, input-arguments...)

   For example:

        Format("{0:ddMMMyyyy} - {1} - {2:0.000}", Entry.Date, Entry.Location, Entry.Reading)

- `And` and `Or` - used for logical combinations. Also available as operators - `&&` and `||`

        And(test-one, test-two, ...)
        test-one && test-two

        Or(test-one, test-two, ...)
        test-one || test-two


   For example:

        And(HasProAccount, HasCreditCardDetails)
        HasProAccount && HasCreditCardDetails

- Add - used for contatenation or addition - works with `string`, `int`, `double` Types (other input types may be accepted, but will be converted to one of these types). Also available as the `+` operator

        Add(item-one, item-two, ...)
     
        item-one + item-two


   For example:

         Add(SubTotal, Tax)

         FirstName + ' ' + LastName

- To be coninued... Subtract, Multiply, Divide, Modulus, etc

- To be continued... GreaterThan, EqualTo, LessThan, GreaterThanOrEqualTo, LessThanOrEqualTo etc

- To be continued... RGB from the Color plugin