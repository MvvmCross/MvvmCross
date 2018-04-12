---
layout: documentation
title: Linking
category: Fundamentals
order: 15
---

When writing Xamarin Apps it is a well known fact that when building your code in certain configurations there is a process called Linking that will be run. 
What this does is to look at the Intermediate Language (IL) output for your App, walk through it, then it will perform mark and sweep operations to remove code that is never used at runtime.

This process is done to cut down on the size of the resulting assembly. This is very useful when consuming large third party assemblies, such as MvvmCross.

One thing the Mono Linker is not very good at is to figure out when code is used through reflection. Since MvvmCross bindings heavily rely on reflection a lot of the time the Mono Linker does not realise a piece of code is used and happily strips it out. Only for the developer to figure out at runtime that something either simply does not respond to a touch event or for the developer to experience crashes with funny stack traces.

## Typical Exceptions
Typical exceptions which you will encounter when the Linker is enabled would be:
* FileNotFoundException - this will occur if the Linker deems the entire assembly not used, it will not get included in the generated App
* NullReferenceException - this typically is thrown when a method is attempted to be found via reflection and simply isn't there
* MissingMethodException - same as above, typically when the constructor of a class is removed

## How to deal with this?
There are a couple of different ways to deal with code being removed in the Linking process.

### Preserve Attribute
If you encounter these issues in your own code the easiest way is to use the `[Preserve]` attribute. In Xamarin.iOS this comes from the [`Foundation` name space](https://developer.xamarin.com/api/type/Foundation.PreserveAttribute/). In Xamarin.Android this is located in the [`Android.Runtime` name space](https://developer.xamarin.com/api/type/Android.Runtime.PreserveAttribute/).

For portable targets such as Portable Class Libraries and .NET Standard libraries, these are not there out of the box. In such cases you can add your own `PreserveAttribute` class.

```csharp
[System.AttributeUsage(System.AttributeTargets.All)]
class PreserveAttribute : System.Attribute {
    public PreserveAttribute () {}
    public bool AllMembers { get; set; }
    public bool Conditional { get; set; }
}
```

Then you can simply apply this attribute on classes you need to be preserved like

```csharp
[Preserve]
public class MyClass {
```

To also preserve members

```csharp
[Preserve(AllMembers = true)]
public class MyClass {
    private bool someMember;
```

If you want your entire assembly to be preserved you can do

```csharp
[assembly:Preserve]
```

This only works on code you write yourself as you need to add the attribute.

### Hinting the linker with LinkerPleaseInclude.cs 

Another way to have the linker understand that you are using a piece of code that is otherwise removed, is to hint the linker about the usage.

Previously when NuGet allowed adding .cs files as content, we added a `LinkerPleaseInclude.cs` file as part of the `MvvmCross` NuGet package. We cannot do this anymore, hence you need to manage this yourself.

The gist of this file is to describe the parts used of a class to hint the linker not to remove this code.

Let us say you are using a `UIButton` in a Xamarin.iOS App and you are binding a `ICommand` to that button. The MvvmCross binding engine will through reflection weak subscribe to the `TouchUpInside` event and invoke the `ICommand` when you tap that button. Since the Linker cannot see any of this, you would typically need to add a section to your `LinkerPleaseInclude.cs` file. This would typically look as follows.

```csharp
using Foundation;
using UIKit;

namespace My.Awesome.App
{
    [Preserve(AllMembers = true)]
    public class LinkerPleaseInclude
    {
        public Include(UIButton button)
        {
            button.TouchUpInside += (s, e) =>
                button.SetTitle(button.Title(UIControlState.Normal), UIControlState.Normal);
        }
    }
}
```

This will hint that both the `TouchUpInside` event is used and that we also use `SetTitle` and `Title(UIControlState)` to set the title for that button.

You will need to add sections to this file as needed.

> You can find some good defaults in our [ContentFiles in our GitHub repository](https://github.com/MvvmCross/MvvmCross/tree/develop/ContentFiles).

LinkerPleaseInclude files need to be added per platform and need to be compiled. However, these are never invoked, so don't worry if you write code that does not make sense in them. The code is just there to hint the linker.

### Skipping assemblies

As a last resort you could instruct the Linker to skip certain assemblies. This can be done by adding semi-colon `;` delimited list of assemblies to the `AndroidLinkSkip` property in a Xamarin.Android project either by editing the `.csproj` file adding to the first `PropertyGroup` as follows.

```xml
<PropertyGroup>
    <AndroidLinkSkip>MvvmCross;MvvmCross.Plugin.Messenger</AndroidLinkSkip>
```

Or on a Android project you can go to **Options > Android Build > Linker** and add to the *Ignore assemblies* field.

To do something similar for an Xamarin.iOS app, you can go to **Options > iOS Build** and add some additional *mtouch* arguments for each of the assemblies you want to skip.

```
--linkskip=MvvmCross --linkskip=MvvmCross.Plugin.Messenger
```

> Please note, the *Ignore assemblies* field and the *mtouch arguments* are configuration specific, so make sure you do this for each configuration you want to skip assemblies.