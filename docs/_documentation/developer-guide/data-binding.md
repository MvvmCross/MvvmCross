---
title: "Data binding"
excerpt: ""
---
DataBinding is the key technology that Mvvm relies on to link Views with their ViewModels.

DataBinding provides and maintains the automated two-way connection between View and ViewModel. A good understanding DataBinding is essential for every Mvvm developer.

Within MvvmCross, databinding was initially built to mirror the structure provided by Microsoft in their Xaml based frameworks, but in more recent developments MvvmCross has extended databinding in new directions.

This article focuses first on the core 'Windows' databinding approach, but then later extends to some of the newer ideas.

###Core Windows Databinding
In this structure, for each binding:

- C# properties are used in both View and ViewModel
- A single View property is 'bound' - connected - to a ViewModel property
- is specified with a Mode which gives a direction for data flow
- can optionally be specified with a ValueConverter - and this can optionally also be parameterised
- can also optionally be specified with a FallbackValue for when binding fails.

###C# properties and data-binding

C# properties are used for data-binding on both the View and the ViewModel.

On the ViewModel, these properties often look like:

    private string _myProperty;
    public string MyProperty
    {
       get { return _myProperty; }
       set 
       { 
          _myProperty = value;
          RaisePropertyChanged(() => MyProperty);
          // take any additional actions here which are required when MyProperty is updated
       }
    }

This pattern uses a local private backing variable to store the current value, and relies on `RaisePropertyChanged` to signal changes in the value to any listening Views.

In the View:

- in Windows platforms, `DependencyProperty` objects are used to store variable values. These `DependencyProperty` objects provide well-known mechanisms:
  - to allow both `get` and `set` of the value within the View.
  - to listen for changes on the value within the View - e.g. when the user enters new text into a `TextBox` 

- in new C# platforms like Xamarin.Android and Xamarin.iOS:
  - most commonly
     - normal C# properties are used to get and set variable values - typically these C# properties are wrapper properties that Xamarin has created around native Java or Objective-C methods
     - normal C# events are used to determine when these variable values have changed - e.g. when a user has entered a value.
  - sometimes - when there isn't a neat property and event based mapping available
     - custom C# methods have to be used to get and set the variable values
     - custom Java listeners or Objective-C delegates have to be used to detect when the UI View state changes (e.g. when the user enters text or taps on a button).

For more info on the details on implementing custom bindings, see http://slodge.blogspot.co.uk/2013/06/n28-custom-bindings-n1-days-of-mvvmcross.html

### DataBound properties

Using the View and ViewModel properties described above, it's common for a ViewModel C# property to be used to  model the value of a View property.

For example:

- if your View contained a `CheckBox` which has an `IsChecked` property
- then your ViewModel might contain a property:

        private bool _rememberMe;
        public bool RememberMe
        {
           get { return _rememberMe; }
           set 
           { 
              _rememberMe = value;
              RaisePropertyChanged(() => RememberMe);
           }
        }

- then a binding might connect together `IsChecked` on the View with `RememberMe` in the ViewModel.

### DataBound events and actions

Databinding also enables a ViewModel to react to 'events' which occur in a View - e.g. for a ViewModel to respond to events such as a button being pressed.

The technique generally used for this is for the ViewModel to expose special `Command` properties which can be bound to corresponding `Command` properties on the View.

For example, a `CheckBox` might have a `CheckedCommand` and this might be bindable to a `RememberMeChangedCommand` on the ViewModel.

Within Windows, For sometimes, when a View has not exposed

### Binding Modes 

There are 4 modes in which properties in the View can be bound to properties in the ViewModel:

- One-Way
- One-Way-To-Source
- Two-Way
- One-Time

**One-Way** 

- This binding mode transfers values from the ViewModel to the View
- whenever the property changes within the ViewModel, then the corresponding View property is automatically adjusted. 
- This binding mode is useful when when showing, for example, data which is arriving from a dynamic source - like from a sensor or from a network data feed. 
- In Windows/Xaml, this is very often the default binding mode - so it is the mode used when no other is selected.

**One-Way-To-Source**

- This binding mode transfers values from the View to the ViewModel 
- When the View property changes then the corresponding ViewModel will be updated. 
- This binding mode is useful when collecting new data from a user - e.g. when a user fills in a blank form.
- In practice, this binding mode is rarely used - most developers choose to use Two-Way instead.

**Two-Way**

- This binding mode transfers values in both directions
- Changes in both View and ViewModel properties are monitored - if either changes, then the other will be updated.
- This binding mode is useful when editing entries an existing form, and is very commonly used by developers.
- Where MvvmCross had created new bindings, then this is very often the default binding mode MvvmCross tries to use.  

**One-Time**

- This binding mode transfers values from ViewModel to View
- This transfer doesn't actively monitor change messages/events from the ViewModel
- instead this binding mode tries to transfer data from ViewModel to View only when the binding source is set. After this, the binding doesn't monitor changes and doesn't perform any updates, unless the binding source itself is reset. 
- This mode is not very commonly used, but can be useful for fields which are configurable but which don't tend to change after they have initially been set. 
- In MvvmCross, we use One-Time binding when setting static text from language files - this is because it's common for the user to select a language, but once chosen, it's uncommon for the user to then change that language.

###Value Conversion

A `ValueConverter` is a class which implements the `IValueConverter` interface.

This interface provides two `object` level conversion methods:

- `Convert` - providing a simple mechanism for changing values from `ViewModel` to `View`
- `ConvertBack` - providing a simple mechanism for changing values back from `View` to `ViewModel`

It is very common for a `ValueConverter` to only be used for converting values for display in the `View`. In this case, only the `Convert` method is implemented.

Because the `IValueConverter` interface is not exactly the same across all platforms, MvvmCross instead provides `IMvxValueConverter` interface which can be mapped to `IValueConverter` on each platform.

Further, MvvmCross provides some supporting base classes to help with writing value converters:

- `MvxValueConverter`
- `MvxValueConverter<TFrom>`
- `MvxValueConverter<TFrom, TTo>`

As an example, a `LengthValueConverter` which is only used to help display the lenght of a string - with no `ConvertBack` use - might be implemented:

    public class LengthValueConverter 
    	: MvxValueConverter<string, int>
    {
        protected override int Converter(string value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (value == null)
            	return 0;
            return value.Length;
        }
    }

ValueConverters can also be provided with a `parameter` - this can sometimes be useful to reuse a single value converter in different situations. For example, a `TrimValueConverter` might be able to take the characters to trim in its `parameter`.

###Fallback Values

Sometimes the ViewModel source property for the ViewModel isn't available.

For example:

- suppose you bind a label's `Text` to `Customer.FirstName` on the ViewModel
- and that you specify a `FallbackValue='no customer'`
- if `Customer` is `null` at any point, then `Customer.FirstName` will be treated as `UnsetValue` - and so the fallback 'no customer' will be displayed.

Notes:

- in this example, if `Customer` is an object which has a `null` `FirstName` then this will **not** cause the Fallback to be used. The Fallback is there only for cases where the value is `Unset` - where the binding engine cannot find a value. A `null` value is still a value - so it doesn't trigger the Fallback.
- one situation which can trigger an `UnsetValue` is if an `Exception` is thrown - e.g. during the evaluation of a ValueConverter. In this case, the binding engine will treat this `Exception` as an `UnsetValue` and so the `FallbackValue` will be used.



###A note about `DataContext`

While we have used the terms `View` and `ViewModel` throughout this article, you will also see `DataContext` used in this article and in code.

`DataContext` is simply the C# name for the property on a `View` to which the `ViewModel` is assigned.

`ViewModel` and `DataContext` can be considered as the same thing.


##MvvmCross Databinding

Databinding has always been at the heart of MvvmCross, and the functionality has grown over the different versions of MvvmCross.
  
The evolution of MvvmCross Data-Binding can be trace over a path of:

- JSON
- Swiss
- Fluent
- Tibet, Rio and the future!

**JSON**

In the first versions of MvvmCross the databinding framework used strings based on JSON to achieve databinding.

This JSON version is now retired - it is no longer easily available in MvvmCross v3. If you come across any samples or documents which use JSON binding, then be aware that these samples are now **out of date**.

**Swiss**

The replacement for JSON databinding was called 'Swiss'. It provides the same functionality as the JSON binding, but used a cleaner, less verbose syntax that is easier to use.

For example:

- a binding in JSON which was written:

      {
        'Text'
          {
            'Path':'TweetText',
            'Converter':'RemainingLength',
            'ConverterParameter':140
          }
      }

- could be rewritten in Swiss using:

      Text TweetText, 
         Converter=RemainingLength,
         ConverterParameter=140
      
**Fluent** 

In addition to providing text-format binding statement which can be easily included in xml layout files, MvvmCross also provides a C# based syntax to enable bindings to be easily constructed using code.

This binding syntax is referred to as 'Fluent' bindings.

As an example:

- the text binding:

      Text TweetText, 
         Converter=RemainingLength,
         ConverterParameter=140
         
- might be rewritten using fluent bindings as:

       this.CreatingBinding(label)
           .For(l => l.Text)
           .To(vm => vm.TweetText)
           .WithConversion("RemainingLength", 140); 

Fluent binding is especially useful in the iOS and OSX platforms where the Xml layout formats are not easily human-editable.

**Tibet, Rio and the future**

As the MvvmCross binding format evolved from its JSON origins through to Swiss and Fluent, the improvements were largely cosmetic. These cosmetic changes offered improved developer productivity through easier-to-use binding syntax, and did so without offering any changes in functionality in the underlying data-binding itself.

The future of MvvmCross continues to aim to include cosmetic improvements, but also aims to deliver functional enhancements as well.

The current ideads for improvements and enhancements include:

- Tibet binding
  - multi-binding
  - ValueCombiners
  - literal-binding
  - binding macros
  - functional syntax for ValueConverters and ValueCombiners
  - nested value conversion
- Rio binding
- Auto-Command binding

Using these ideas, then a binding like:

      Text TweetText, 
         Converter=RemainingLength,
         ConverterParameter=140

might be rewritten:

      Text RemainingLength(TweetText,140)

or perhaps even:

      Text 140 - Length(TweetText)

These ideas and their current development status are discussed further later in this article.

Beyond these, of course, the opportunity is there for plenty more ideas and improvements from the community - the evolution of MvvmCross and its databinding is driven by real users and the invention and ideas their real apps require.


###JSON

As discussed above, JSON binding is not supported within MvvmCross v3 or later.

###Swiss

Swiss binding syntax allows a basic binding from a View $Target$ to a ViewModel property $SourcePath$ to be written using a syntax:

     $Target$ $SourcePath$
     
where `$Target$` must always be the direct path to the View property - e.g.:

- Text
- IsChecked
- Value
- …

`$SourcePath$` can be any C# style path to the property on the ViewModel or on a sub-object exposed using a property - e.g.:

- UserId
- RememberMe
- Password
- Customer.FirstName
- Customer.Address.City
- Customer.Orders[0].Date
- Customer.Orders[0].Total
- Customer.Cards["Primary"].Expiry
- Customer.Cards["Primary"].Number
- …

     
Beyond this basic `$TargetPath$` to `$SourcePath$` syntax:

- If `$SourcePath$` is omitted or a single period "." is used, then the Source used is the whole of the ViewModel.
     
- If a converter is needed, then this can be added using:

       , Converter=$ConverterName$

  where `$ConverterName$` is the name of the Value Converter to use - which typically is the class name without it's `ValueConverter` postfix - e.g. the name "Length" would be used for the class `LengthValueConverter`
  
- If a ConverterParameter is needed then this can be added using:

       , ConverterParameter=$ParameterValue$

  where `$ParameterValue$` is one of:

  - a quotation or double-quotation delimited `string`
  - the word 'null' meaning C# `null`.
  - the word 'true' or 'false' providing a `bool` value
  - an integer number - which will be parsed as a `long`
  - a floating point number - which will be parsed as a `double`


- If a FallbackValue is needed then this can be added using:

       , FallbackValue=$FallbackValue$

  where `$FallbackValue$` has the same permitted contents as `$ParameterValue$` above, but can also additionally be:
  
  - the `ToString()` representation of an Enum value.
  
  This is especially useful in, for example, the binding of `Visibililty` properties.

- If a specific Binding Mode is needed, then this can be added:

       , Mode=$WhichMode$

  where `$WhichMode$` is one of:
  
  - `OneWay`
  - `OneWayToSource`
  - `TwoWay`
  - `OneTime`
  - `Default`
  
- Where multiple bindings are needed, these can be separated using a semi-colon

- One very specific extension to 'Swiss' binding is the `CommandParameter` binding, this binding uses an implicit ValueConverter to specify the parameter for an `ICommand` invocation. This is specified using:

       , CommandParameter=$CPValue$
       
  where `$CPValue$` is a literal value similar to `$ParameterValue$` above



Some examples of Swiss binding statements are:

---

    Text Customer.FirstName
  
Bind the `Text` property to `Customer.FirstName` on the ViewModel.

---

    Text Title, Converter=Length
  
Bind the `Text` property to `Title` on the ViewModel, but apply the `Length` value converter - which will normally be a default instance of the class `LengthValueConverter`.

---

    Text Order.Amount, Converter=Trim, ConverterParameter='£'
  
Bind the `Text` property to `Order.Amount` on the ViewModel, but apply the `Trim` value converter, passing it the string "£".

---

    Text Order.Amount, Converter=Trim, ConverterParameter='£', FallbackValue='N/A'
  
Bind the `Text` property to `Order.Amount` on the ViewModel, but apply the `Trim` value converter, passing it the string "£". If no `Order` is available, or if the Order object doesn't have an `Amount` value, then display "N/A"

---

    Value Count, Mode=TwoWay
  
Bind the `Value` property to `Count` on the ViewModel, and ensure this binding is both from View to ViewModel and from ViewModel to View.
 
---

    Click DayCommand, CommandParameter='Thursday'

Bind the `Click` event to the `DayCommand` property on the ViewModel (which should implement `ICommand`). When invoked, ensure that Execute is passeda parameter value of "Thursday"

###Fluent

The fluent syntax provides a C# way to create bindings.

This syntax is generally done using the `CreateBindingSet<TView, TViewModel>` helper.

The syntax includes:

    Bind($ViewObject$) 

where `$ViewObject$` is the view target for binding.

    For(v => v.$ViewProperty$) 

where `$ViewProperty$` is the property on the view for binding.

If `For` is not provided, then the default view property is used - e.g. for a `UILabel` the default is `Text`

    To(vm => vm.$ViewModelPath$)

where `$ViewModelPath$` is the path to the view model 'source'  property for binding.

    OneWay()
    TwoWay()
    OneWayToSource()
    OneTime()
    
all of which provide the mode for the binding

    WithConversion($name$, $parameter$)
    
where `$name$` is the name of the value converter to use, and `$parameter$` is the parameter to pass in. 

Using this syntax, an example binding set is:

     var set = this.CreateBindingSet<MyView, MyViewModel>();
     set.Bind(nameLabel)
        .For(v => v.Text)
        .To(vm => vm.Customer.FirstName);
     set.Bind(creditLabel)
        .For(v => v.Text)
        .To(vm => vm.Customer.Total)
        .WithConversion("CurrencyFormat", "$");
     set.Bind(cardLabel)
        .For(v => v.Text)
        .To(vm => vm.Customer.Cards["Primary"].Number)
        .WithConversion("LastFour")
        .OneWay()
        .FallbackValue("N/A");
     set.Bind(warningView)
        .For(v => v.Hidden)
        .To(vm => vm.Customer.Alert)
        .WithConversion("Not")
        .FallbackValue(true);
     set.Apply();  

In addition to the `Expression` based Fluent bindings, string based Fluent bindings are also available. This is particularly useful for situations where bindings are needed to View events or to binding targets which are not fully exposed as C# properties. For example, even though a `UIButton` does not have a `Title` property in C#, a 'Title' property can still be set using:

    set.Bind(okButton)
       .For("Title")
       .To(vm => vm.Caption);
           

**Note:** when using a fluent binding, always remember to use `.Apply()` - if this is missed then the binding won't ever be created.


###Tibet

Tibet binding includes several ideas which **extend** Swiss binding.

Tibet has been carefully designed so that it is backwards compatible - any existing Swiss bindings should work within Tibet.

The core parts of Tibet are:

  - multi-binding
  - value combiners
  - literal-binding
  - binding macros
  - functional syntax for ValueConverters and ValueCombiners
  - nested value conversion
  
**Multi-binding**

In Swiss binding, each binding can only reference a single ViewModel property path.

This meant that if a ViewModel had 2 properties like `FirstName` and `LastName`, then the main way to create a display of the ful name was to create a new ViewModel property - e.g.:

    private string _firstName;
    public string FirstName
    {
       get { return _firstName; }
       set 
       { 
          _firstName = value;
          RaisePropertyChanged(() => FirstName);
          RaisePropertyChanged(() => FullName);
       }
    }
    
    private string _lastName;
    public string LastName
    {
       get { return _lastName; }
       set 
       { 
          _lastName = value;
          RaisePropertyChanged(() => LastName);
          RaisePropertyChanged(() => FullName);
       }
    }

    public string FullName
    {
       get { return _firstName + " " + _lastName; }
    }

With Multi-Binding, the addition of the `FullName` property is no longer necessary - instead the binding can be written inside the binding expression directly as:

    Text FirstName + ' ' + LastName

This **multi-binding** will cause `Text` to automatically be updated whenever either of `FirstName` or `LastName` change.

**ValueCombiners**

A ValueCombiner is a new technique provided by Tibet binding in order to merge multiple sources into a single value.

For example, the multi-binding example above used two `Add` value combiners in order to link together three inputs:

- `FirstName`
- ' '
- `LastName`

There are a small number of ValueCombiners provided within Tibet, including:

- `If(test, if_true, if_false)` - takes 3 inputs:
  - a boolean test value
  - an if_true value which is used if the test value is true
  - an if_false value which is used otherwise
- `Format(format, args…)` - takes 1 or more inputs
  - a string value which evaluates to a format string template
  - 0 or more args which will be evaluated in the string template
- `Add(one,two)` - takes 2 arguments which it 'combines' together. For two strings, this means concatenation. For two doubles or two longs it means numeric addition. For mixtures of those items, the result is currently not fully specified.
- `GreaterThan(one, two)` - takes 2 arguments and attempts to apply greater than logic to them. As with `Add` this logic is straight-forward for two strings, two doubles or two longs, but is not well-defined for other object combinations.

Notes:

- some ValueCombiners are also available in `operator` form - e.g. `Add` can be used as `+` and `GreaterThan` can be used as `>`
- combination is an interpretation, not a compilation step - especially because dynamic code generation is not supported on all MvvmCross platforms.
- for direct comparison/combination of simple `string`, `long` and `double` values, this 'interpretation' should work well. Using combiners for more complicated combinations is less well defined.
- currenly ValueCombiners typically try to use `long` rather than `int` and `double` rather than `float`.

**Important note:** The current interface of value combiners is currently a proposal and working prototype only. As we learn more about the real use, benefits and challenges of value combination we may revise the API, including making breaking changes to any value combiners produced by the community. In particular, it's possible we may make changes to the type safety of the APIs, and we may try to reduce the complexity of the APIs - as Value Combiners are currently quite 'open' in the source/target types they accept and this makes developing them quite complicated.

**Literal binding**

As we've seen in the previous Multi-Binding and Value-Combining steps, literals can now be included in Tibet binding expressions.

For example, a binding:

    Value 100 * Ratio
    
uses the literal 100 to provide a way of translating a `Ratio` into a `Percentage`.

Or a binding:

    Value 'True'
    
uses a literal string which is automatically converted to a boolean if Value is a boolean property.

Or a binding:

    Value Format('Hello {1} - today is {0:ddd MMM yyyy}', TheDate, Name)
    
uses a literal string to assist formating `TheDate` and `Name`

**Binding macros**

Binding macros are not yet implemented.

Ideas being considered in this area include:

- access to `parent`, `global` and `named(name)` binding contexts
- access to shared variables - e.g, shared numbers, strings and Colors which could provide more theming/styling options
- access to i18n resources to make text localisation more straight-forward

It is likely that that prefix characters, such as `$`, `#` or `@`, might be used as simple markers to enable the identification of these 'macros'

**Functional syntax for ValueConverters and ValueCombiners**

This change allows shorter simpler binding expressions to be used.

This means that a Swiss binding of:

      Text TweetText, 
         Converter=RemainingLength,
         ConverterParameter=140

can be rewritten in Tibet as:

      Text RemainingLength(TweetText,140)

Note that the function name space is shared between the Value Combiners and the Value Converters. When looking for a name, MvvmCross first looks for a matching ValueCombiner, and then second for a matching ValueConverter.
     
**Nested value conversion**

In addition to supporting multiple bound source values, Tibet binding further introduces nested evaluation of value converters and combiners.

For example, Length and Trim value converters could be applied with the Add value combiner as:

       Text Length(Trim(FirstName + ' ' + LastName))

**A Mild Warning** 

Tibet binding provides developers with many options for more advanced bindings.

This advancement is, of course, not free - it does come with a small memory and processing cost during both the construction and the exceution of the bindings.

In general, this additional overhead is very small and so should not be of concern to developers. However, it's always important to be aware of your application's performance - so always consider how a binding will be constructed and evaluated, especially when applying large numbers of bindings, when applying bindings within loops (collections) or when applying bindings to data which changes very frequently. Always consider applying source (ViewModel-based) data manipulation, writing a single optimised combiner/converter or consider simple `OneTime` binding as potential ways to avoid performance issues.

###Rio

Within ViewModels, Mvvm in C# has always been centred around the `INotifyPropertyChanged` interface.

This interface is typically implemented around C# properties which look like:
 
        private string _lastName;
        public string LastName
        {
           get { return _lastName; }
           set 
           { 
              _lastName = value;
              RaisePropertyChanged(() => LastName);
           }
        }

and this is further enhanced using `ICommand` properties for action callbacks - e.g.

    private ICommand _submitCommand;
    public ICommand SubmitCommand
    {
        get
        {
             _submitCommand = _submitCommand ?? new MvxCommand(DoSubmit);
             return _submitCommand;
        }
    }

This syntax is well understood by experienced Mvvm developers, but can also appear quite verbose when dealing with very small view models.

Some developers have worked around this verbosity by using techniques such as post-compilation injectio of code - e.g. using the AOP Property plugin from Fody.

Rio binding offers developers a different approach - using the new FieldBinding and MethodBinding plugins.

**FieldBinding**

With the field binding plugin, MvvmCross databinding can use ViewModel public fields as data-sources for binding - e.g.

     public string LastName;
     
Further, to provide events to drive UI updates, a lightweight `INotifyChanged` interface has been added, along with abbreviated helper interfaces and classes - `INC<T>` and `NC<T>`. These can be used as:

     public readonly INC<string> LastName = new NC<string>();

A `LastName` declared in this way can be databound exactly as te earlier `INotifyPropertyChanged`-based property:

    Text LastName

Further, the underlying `string` field can be accessed in code using:

    LastName.Value = "Hello";
    Mvx.Trace("Current value is {0}", LastName.Value);

To use FieldBinding, import the Field binding plugin into both your `core` and your UI projects.

**Notes:** 

- In addition to the syntatic changes of Rio, there are also some slight performance improvements - achieved by avoiding some Reflection-based get/set and by avoiding string-based event notifications.
- `INotifyChanged` binding has no way to support the `INotifyPropertyChanged` feature 'all changed' which is achieved by signalling a property change with a null or empty property name.
- `INotifyChanged` itself is a very simple interface - so you can easily implement your own classes to implement this if you require extensions.

 
**MethodBinding**

With the method binding plugin, MvvmCross databinding can use ViewModel public methods as sources for `ICommand` without declaring an `ICommand` property.

For example, a method

    public void GoHome()
    {
    	ShowViewModel<HomeViewModel>();
    }
    
can be used in binding as:

    Click GoHome 

Where a single argument is available within the source method, Method Binding uses the command parameter for this call. This is useful in, for example, list item selection events - e.g.:

    public void ShowDetail(ListItem item)
    {
    	ShowViewModel<DetailViewModel>(new { id = item.Id} );
    }
    
bound with:

    ItemClick ShowDetail 

To use MethodBinding, import the Method binding plugin into just your UI projects - there is no 'core' component required for these plugins.

**Note:** One important feature sometimes used in Windows Xaml binding but poorly supported by MvvmCross is the `CanExecute`/`CanExecuteChanged` functionality on `ICommand`. In Xaml binding this property and event pair can be used to enable/disable UI controls such as buttons.

However, in MvvmCross, this auto-enable/disable binding isn't currently widely supported - with support instead being given to secondary binding properties - e.g. to pairs of bindings like:

    Click GoHome; IsEnabled CanGoHome

**The Rio Effect**

A view model built using Rio will **not** be to every developer's liking.

However, the effect on the code-size and readability can be striking.
     
A Rio `INotifyChanged` ViewModel like:

     public class MathsViewModel
     {
     	public readonly INC<double> SubTotal = new NC<double>();
     	public readonly INC<double> Percent = new NC<double>();
        
        public void Calculate()
        {
            Total.Value = SubTotal.Value * Percent.Value;
        }    
         
     	public readonly INC<double> Total = new NC<double>();         
     }
     
is equivalent to a `INotifyPropertyChanged` ViewModel of:
     
     public class MathsViewModel
     {
     	private double _subTotal;
        public double SubTotal
        {
           get { return _subTotal; }
           set
           {
           	  _subTotal = value;
           	  RaisePropertyChanged(() => SubTotal);
           }
        } 
        
     	private double _precent;
        public double Percent
        {
           get { return _percent; }
           set
           {
           	  _percent = value;
           	  RaisePropertyChanged(() => Percent);
           }
        }
        
        private ICommand _calculateCommand;
        public ICommand CalculateCommand;
        {
           get 
           {
              _calculateCommand = _calculateCommand ?? new MvxCommand(Calculate);
              return _calculateCommand;
           }
        }
        
        private void Calculate()
        {
            Total = SubTotal * Percent;
        }    
         
     	private double _total;
        public double Total
        {
           get { return _total; }
           set
           {
           	  _total = value;
           	  RaisePropertyChanged(() => Total);
           }
        }         
     }
     
**BindingEx - Tibet and Rio in Xaml**

Xaml is a platform and product from Microsoft which offers excellent tooling, lots of extensibility for adding new controls, but only limited extensibility for adding customisation.

Unfortunately, this means MvvmCross can't intercept the 'normal' Xaml binding syntax which might look like:

     Text="{Binding FirstName}"
     
However, MvvmCross Swiss, Tibet and Rio binding can be enabled through `AttachedProperties` 

In particular two `AttachedProperties` is supplied in the BindingEx package:

- `mvx:Bi.nd` - for bindings
- `mvx:La.ng` - for internationalisation extensions

To add these properties to your Windows Phone, Store or WPF MvvmCross app:

- include the BindingEx package
- include an additional step in Setup which initialises the WindowsBinding framework

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            
            var builder = new MvxWindowsBindingBuilder();
            builder.DoRegistration();        
        }

- in your Xaml files include an xml attribute for `mvx` - this will be different according to the platform:

 - phone
 
       xmlns:mvx="clr-namespace:mvx;assembly=Cirrious.MvvmCross.BindingEx.WindowsPhone"
 
 - store
 
       xmlns:mvx="using:mvx"
        
 - WPF

       xmlns:mvx="clr-namespace:mvx;assembly=Cirrious.MvvmCross.BindingEx.Wpf"


- in your Xaml files you can now include bindings within tags such as:

       `<TextBlock mvx:Bi.nd="Text Customer.FirstName; Visible=ShowFirstName" />`

- for design-time support, you may also need to pull in additional value converters into the Xaml namespace. For more on this, see http://slodge.blogspot.co.uk/2013/07/n35-multibinding-with-tibet-n1-videos.html

Once installed, the syntax within these `AttachedProperties` bindings is exactly the same as within all other Swiss and Tibet binding - and this binding functionality can be extended with custom bindings, with FieldBinding, etc - just as in MvvmCross on non-Xaml platforms.



###Beyond Rio
The framework that enables the Rio and Tibet binding extensions is interface-based and is built upon the small `CrossCore` platform which underpins `MvvmCross`.

We're excited by the possibilities that this framework can provide - by the inventions that the community can now develop.

Anyone wishing to experiment with creating their own source binding plugins is encouraged to get started by looking at the source code for the MethodBinding and FieldBinding plugins.