---
layout: post
title: MvvmCross 5.2
date:   2017-09-12 11:37:35 +0100
categories: mvvmcross
---

# Announcing MvvmCross 5.2!

With the release of 5.2 we focused on creating new presenters who are able to handle almost all navigation patterns for the platform. Since we already had new ones for iOS and MacOS we are now introducing new presenters for Android and WPF!
The next release(5.3) we will continue with this for UWP and Xamarin.Forms. Also a lot of work was done during the .NET Summer Hackfest, where you can read all about in [this page](https://www.xablu.com/2017/09/05/mvvmcross-xamarin-hackday-results/).

Read carefully about the changes because some of them are breaking, but easy to fix.

## A new default Android presenter [#1934](https://github.com/MvvmCross/MvvmCross/issues/1934)

The current Android presenter is very limited. Out of the box there is no support for Fragments, Dialogs, or other navigation patterns. To fix this we developed a new presenter which will replace the current one as default. The new presenter supports:

- Fragments (Nested)
- Activity's
- Dialogs
- Tabs / ViewPager

Navigation patterns that should be easy to implement with this are:

- NavigationDrawer
- BottomNavigationBar
- BottomSheetDialog
- Master/Detail Flows
- Nested navigation

Additionally it adds support for:

- CustomAnimations per Attribute
- No dependency on MvxCachingFragmentActivity. All Activities can show Fragments now
- SharedElement transitions
- Support for Fragments in default non AppCompat presenter
- Closing of Dialog, Fragments, and Fragments when Activity closes
- Override behaviour on runtime with IMvxOverridePresentationAttribute

The new presenter is very easy to customize and extend. If you have an existing custom presenter we would advice to check compatibility and possible replace it with the new default. 

Read more in the [documentation](https://www.mvvmcross.com/documentation/presenters/android-view-presenter)

## A new default WPF presenter [#2124](https://github.com/MvvmCross/MvvmCross/pull/2124)

The new WPF presenter enables to show modal/modal less window. It also changes the signature and some methods.

Read more in the [documentation](https://www.mvvmcross.com/documentation/presenters/wpf-view-presenter)

## NavigationService improvements [#2072](https://github.com/MvvmCross/MvvmCross/pull/2072)

There are breaking changes in the signature to prevent problems with async code. Any navigation done with the NavigationService and a parameter or result will now be triggered in the `Prepare` method.

```c#
public class MyViewModel : MvxViewModel
{
    private readonly IMvxNavigationService _navigationService;
    public MyViewModel(IMvxNavigationService navigationService)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
    }
    
    public override async Task Initialize()
    {
        //Do heavy work and data loading here
    }

    public async Task SomeMethod()
    {
        var result = await _navigationService.Navigate<NextViewModel, MyObject, MyReturnObject>(new MyObject());
        //Do something with the result MyReturnObject that you get back
    }
}

public class NextViewModel : MvxViewModel<MyObject, MyReturnObject>
{
    private readonly IMvxNavigationService _navigationService;
    public MyViewModel(IMvxNavigationService navigationService)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
    }
    
    public override void Prepare(MyObject parameter)
    {
        //Do anything before navigating to the view
        //Save the parameter to a property if you want to use it later
    }
    
    public override async Task Initialize()
    {
        //Do heavy work and data loading here
    }
    
    public async Task SomeMethodToClose()
    {
        await _navigationService.Close(this, new MyReturnObject());
    }
}
```

This prevents blocking the navigation before showing the view.

Read more in the [documentation](https://www.mvvmcross.com/documentation/fundamentals/navigation)

## MvvmCross.Forms StarterPack [#2166](https://github.com/MvvmCross/MvvmCross/pull/2166)

You can now get started on a new Xamarin.Forms app with a set of files to get up to speed.
Just install the MvvmCross.Forms.StarterPack nuget package and you are good to go!

## Combiners with Fluent Bindings [#2143](https://github.com/MvvmCross/MvvmCross/pull/2143)

You can now use Expressions, pass each property individually and even pass an instance of your combiner instead of relying on a string that will retrieve a registered combiner.

Read more in the [documentation](https://www.mvvmcross.com/documentation/fundamentals/value-combiners)

## Ongoing work to clean the samples

During the Hackfest we had a lot of contributions to the samples. This is an ongoing process where anyone including you can help in!

# Change Log

Coming soon!
