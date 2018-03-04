---
layout: documentation
title: Value combiners
category: Fundamentals
order: 9
---

### ValueCombiners

Tibet binding (see [wiki/Databinding](https://github.com/slodge/MvvmCross/wiki/Databinding)) introduced a new interface into binding - `IMvxValueCombiner` - this interface allows multiple binding sources to be combined together within a single target expression. This interface is used in, for example, the `MvxFormatValueCombiner` in order to enable binding expressions like:

         local:MvxBind="Text Format('{0} {1} {2}', Greeting(Gender), FirstName, LastName)"
         
The rules and mechanisms for registering ValueCombiners are similar to those for registering ValueConverters. However, because combiners are not commonly declared in user code, MvvmCross doesn't current perform a Reflection sweep across your Core or UI assemblies. If you do want to add is used by default on the , then a ValueCombiner class named `FooValueCombiner` will be registered under the name `Foo`.
          
Please be aware that withing the MvvmCross Tibet binding syntax, ValueCombiners and ValueConverters share the same 'registered name' space - because both of them are expressed as 'functions' then it's impossible to have both a `Foo` ValueConverter and a `Foo` ValueCombiner - if both are registered then the ValueConverter will always be used rather than the ValueCombiner. 
          
The API for `IMvxValueCombiner` is significantly more complicated than `IMvxValueConverter` at present and it's tied to ImvxSubStep - which is part of the internal structure of the MvvmCross binding evaluation engine.

```c#
public interface IMvxValueCombiner
{
    Type SourceType(IEnumerable<IMvxSourceStep> steps);
    void SetValue(IEnumerable<IMvxSourceStep> steps, object value);
    bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value);
    IEnumerable<Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps, Type overallTargetType);
}
```

To assist with authoring ValueCombiners, a number of helper classes are available including the base `MvxValueCombiner` class which provides default implementations for all methods in the interface.

An example ValueCombiner which counts the number of non-null inputs bound to it might be:

```c#
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
```

This could be used in a binding to count (for example) how many peoples have been picked for a band:

    local:MvxBind="Text Counting(Guitarist, Drummer, Bass, Vocalist)"

Note that it's unusual for a ValueCombiner to meaningfully implement `SetValue` - this is because it's unusual (but not unheard of) for multi-bindings to support updating of the multiple source elements from changes in the View.

Developers are very welcome to write their own ValueCombiners if they wish to - please do - but please also be aware that it's likely that this internal `IMvxValueCombiner` API will change in future MvvmCross revisions - we are looking at ways to either simplify this Tibet binding interface and/or ways to make the binding structure more Type-aware so that conversions can be performed at more places within the binding engine. (Developers are also very welcome to suggest improvements for this API!)

### Available ValueCombiners

The 'standard' ValueCombiners available in MvvmCross are:

- `If` - used for if-else conditional display with syntax 

        If(boolean-test, value-if-true, value-if-false)

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

- Add - used for concatenation or addition - works with `string`, `int`, `double` Types (other input types may be accepted, but will be converted to one of these types). Also available as the `+` operator

        Add(item-one, item-two, ...)
     
        item-one + item-two


   For example:

         Add(SubTotal, Tax)

         FirstName + ' ' + LastName

- To be continued... Subtract, Multiply, Divide, Modulus, etc

- To be continued... GreaterThan, EqualTo, LessThan, GreaterThanOrEqualTo, LessThanOrEqualTo etc

- To be continued... RGB from the Color plugin

