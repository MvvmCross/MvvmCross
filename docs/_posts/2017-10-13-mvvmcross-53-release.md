---
layout: post
title: MvvmCross 5.3
date:   2017-10-13 11:37:35 +0100
categories: mvvmcross
---

# Announcing MvvmCross 5.3!

Most of our focus went out to creating new presenters for Xamarin.Forms and improving the code around that. With that said we are proud to announce the new presenters! 
They now work on the same view attributes as the other presenters and are very flexible in showing views. This also enables you to mix and match Native Xamarin views with Xamarin.Forms pages using ViewModel navigation!

## Lets look at a sample:

```c#
[MvxModalPresentation(WrapInNavigationPage = true ,Title = "Nested")]
public partial class NestedModalPage : MvxContentPage<NestedModalViewModel>
{
	public NestedModalPage()
	{
		InitializeComponent();
	}
}
```

## Supported attributes

- MvxCarouselPagePresentation
- MvxContentPagePresentation
- MvxMasterDetailPagePresentation
- MvxModalPresentation
- MvxNavigationPagePresentation
- MvxTabbedPagePresentation

There are also new MvvmCross base classes for all Xamarin.Forms Page types supporting DataBinding, ViewModels and more from code and layout files!

# Extended MvvmCross bindings support

## Already possible

```c#
<Label mvx:La.ng="Text ThisIsLocalized" />
```

## New bindings

```c#
<Label Text="{mvx:MvxLang ThisIsLocalizedToo}" />
<Label Text="{mvx:MvxBind TextSource, Mode=OneTime, Converter=Language, ConverterParameter=ThisIsLocalizedThroughMvxBind}" />
```

# Other improvements

To make the mixing for Xamarin.Forms and Native Xamarin views possible we had to align all the presenters. We've now done that and they use the same structure with View Attributes.
At the same time we improved those presenters and fixed issue with Dialogs and the NavigationService.

## Open-Generic registration on IoC

There are situations where you have an interface with a generic type parameter `IFoo<T>` and you have to register it in the IoC with different T types. One way to do this is to register it as many times as T types you have:

    Mvx.RegisterType<IFoo<Bar1>, Foo<Bar1>>();
    Mvx.RegisterType<IFoo<Bar2>, Foo<Bar2>>();
    Mvx.RegisterType<IFoo<Bar3>, Foo<Bar3>>();
    Mvx.RegisterType<IFoo<Bar4>, Foo<Bar4>>();

But this creates boilerplate code and in case you need another instance with a different T type, you have to register it as well. To solve this you can register this interface as *open-generic*, i.e. you don't specify the generic type parameter in neither the interface nor the implementation:
    
    Mvx.RegisterType<IFoo<>, Foo<>>();
    
Then at the moment of resolving the interface the implementation takes the same generic type parameter that the interface, e.g. if you resolve `var foo = Mvx.Resolve<IFoo<Bar1>>();` then `foo` will be of type `Foo<Bar1>`.
As you can see this give us more flexibility and scalability because we can effortlessly change the generic type parameters at the moment of resolving the interface and we don't need to add anything to register the interface with a new generic type parameter.

# Change Log

Coming soon
