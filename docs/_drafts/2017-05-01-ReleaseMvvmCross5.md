---
layout: post
title: MvvmCross 5.0 release!
date:   2017-05-01 11:37:35 +0100
categories: mvvmcross
---

# Announcing MvvmCross 5.0!
We're happy to announce the immediate availability of MvvmCross 5.0!
For the last 6 months we have been working on this release and we're really excited about it.

Let's have a look at a couple of things:

## Highlights

### Merge of repos
For a long time we have been looking at merging the different repositories of the project.
Besides a better overview one of the main advantages this brings is the ability to setup a proper CI process.
This is hugely beneficial in the release process itself and makes (patch) releasing MvvmCross a breeze. 

### New website with improved documentation
Over time we received a lot of feedback from developers that use MvvmCross on a day-2-day basis. Besides the usual
hugs and kisses the most common remark we often got was: "sir, you need to improve the documention". And as you all know: writing documention is hard. And not always the most funny thing you can think of. So our first focus was enabling *you* to help out improving the documentation. With 5.0 the documentation has landed in the GIT repo, making it possible to submit documentation (changes) just as you do with code: create a pull request. We're already seeing the benefits of this: the amount of community driven documentation changes has increased tenfold. 

Over the coming months we'll introduce a 'documentation policy' to make sure the documentation keeps on improving over time and keeps it uniformity.

### New iOS presenter

Starting with MvvmCross 5.0, there is a new default Presenter for Views, namely `MvxIosViewPresenter`.

#### View Presenter Overview

The default presenter that comes with MvvmCross offers out of the box support for the following navigation patterns / strategies:

- Stack navigation
- Tabs
- SplitView
- Modal
- Modal navigation

Also if your app needs another kind of presentation mode, you can easily extend it!

#### Presentation Attributes

The presenter uses a set of `PresentationAttributes` to define how a view will be displayed. The existing attributes are:

* MvxRootPresentationAttribute
* MvxChildPresentationAttribute
* MvxModalPresentationAttribute
* MvxTabPresentationAttribute
* MvxMasterSplitViewPresentationAttribute
* MvxDetailSplitViewPresentationAttribute

#### Views without attributes: Default values

- If the initial view class of your app has no attribute over it, the presenter will assume stack navigation and will wrap your initial view in a `MvxNavigationController`.
- If a view class has no attribute over it, the presenter will assume _animated_ child presentation.

#### Sample please!
You can browse the code of the [Playground](https://github.com/MvvmCross/MvvmCross/tree/master/TestProjects/Playground) iOS project to see this presenter in action.

### Improved navigation

MvvmCross 5 introduces a new NavigationService! The new navigation enables you to inject it into your ViewModels, which makes it more testable, and gives you the ability to implement your own navigation!

The main features of the new navigation are:

* Return a result to the ViewModel where you navigated from
* Check if you are able to navigate to a certain ViewModel
* Type safe
* Fully async await
* URL navigation with deeplinking to ViewModels
* Events on navigate

The following Api is available to use:

```c#
public interface IMvxNavigationService
{
    Task Navigate<TViewModel>() where TViewModel : IMvxViewModel;
    Task Navigate<TViewModel, TParameter>(TParameter param) where TViewModel : IMvxViewModel<TParameter> where TParameter : class;
    Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param) where TViewModel : IMvxViewModel<TParameter, TResult> where TParameter : class where TResult : class;
    Task<TResult> Navigate<TViewModel, TResult>() where TViewModel : IMvxViewModelResult<TResult> where TResult : class;
    Task Navigate(string path);
    Task Navigate<TParameter>(string path, TParameter param) where TParameter : class;
    Task<TResult> Navigate<TResult>(string path) where TResult : class;
    Task<TResult> Navigate<TParameter, TResult>(string path, TParameter param) where TParameter : class where TResult : class;
    Task<bool> CanNavigate(string path);
    Task<bool> CanNavigate<TViewModel>() where TViewModel : IMvxViewModel;
    Task<bool> Close(IMvxViewModel viewModel);
}
```

For more details see [#1634](https://github.com/MvvmCross/MvvmCross/issues/1634)

The Uri navigation will build the navigation stack if required. This will also enable deeplinking and building up the navigationstack for it. Every ViewModel added to the stack can split up into multiple paths of it's own backstack. This will enable all kinds of layout structures as Hamburger, Tab or Top navigation.

If you want to intercept ViewModel navigation changes you can hook into the events of the NavigationService.

```c#
Mvx.Resolve<IMvxNavigationService>().AfterClose += (object sender, NavigateEventArgs e) => {
    //Do something with e.ViewModelType or e.Url
};
```

The events available are:
* BeforeNavigate
* AfterNavigate
* BeforeClose
* AfterClose

A full explanation can be found on the [documentation](https://www.mvvmcross.com/documentation/fundamentals/navigation)

### Lifecycle / Event hooks

Starting from MvvmCross 5.0 ViewModels will be coupled to the lifecycle of the view. This means that the ViewModel has the following methods available:

```c#
    void Appearing();
    void Appeared();
    void Disappearing();
    void Disappeared();
```

The MvxViewController, MvxFragment(s), MvxActivity and the UWP views will call those methods open the platform specific events that are fired. This will give us a more refined control of the ViewModel and the state of its lifecycle. There may be binding that you want to update or resources to clean up, these lifecycle events can help with that.

It should be noted however that it is not 100% reliable but it should work for most of the apps. We don't know what you do in the lifecycle of your app and what could interfere with the called order of the viewmodel lifecycle events.

For more information on the implementation of this functionality please see [Github](https://github.com/MvvmCross/MvvmCross/pull/1601)

Documentation for this is available on the [website](https://www.mvvmcross.com/documentation/fundamentals/viewmodel-lifecycle)

### Recyclerview features

Something about the changes here.

### Generic and typed bindings

This change will add a generic "WithConversion" method. This will allow developers to strongly type the use of value converters, making refactoring a lot easier and more save. For example:

```c#
set.Bind(textField).To(vm => vm.Counter).WithConversion<SomeValueConverter>();
```

Add something about the Generic implementation of IMvxTargetBinding [#1610](https://github.com/MvvmCross/MvvmCross/pull/1610)

Include an additional option than literal strings for MvvmCross defined custom bindings. The functionality to define a binding via its string name to remain as is, so no functionally is lost and fully backwards supported. Just expose the MvvmCross custom binding properties in a strongly typed manner. Will affect both MvvmCross platform custom bindings and plugins custom bindings.

Extension methods can be used to return the custom binding name and additionally be used to restrict bindings against only allowed base types.

Expose public extension methods. Note additional work will need to be done inside MvxPropertyExpressionParser to properly handle the extension methods (May need some help figuring out how best to do this).

##### Developer Usage

```c#
var labelButton = new UILabel();

var bindingSet = this.CreateBindingSet<HomeViewController, HomeViewModel>();
bindingSet.Bind(labelButton).For(c => c.BindingUILabelText()).To(vm => vm.NextLabel);
bindingSet.Bind(labelButton).For(c => c.BindingUIViewTap()).To(vm => vm.NextCommand);
bindingSet.Apply();
```

##### Advantages

Can include additional comments for developer to see via intellisense if needs be
Checks whether the binding is possible against the specified control base type, i.e. TouchUpInside binding works against UIControl inheritance and not a UIView.

More information can be found in the [documentation](https://www.mvvmcross.com/documentation/fundamentals/data-binding)

### Removal of WindowsPhone 8.x and Windows 8.x
As is usual with a major release it's time to say goodbye to old friends. Windows(Phone) 8 is depreceated for a long time; removing formal support for this platform is the right thing to do.

### Removal of deprecated plugins
MvvmCross' powerful plugin framework has brought us many good things. However, over time certain plugins have become obsolete or considered not useful anymore. With 5.0 we've decided to remove the following plugins:

* AutoView 
* CrossUI
* Dialog
* SQLite plugin
* Bookmarks
* SoundEffects
* ThreadUtils
* JASidePanels

### Other improvements

* tvOS support
* Test projects in main repo
* Migrate Test.Core to PCL 
* Sidebar fixes

### Bugfixes

## Changelog

Below is a link to the complete changelog. More than 120 PR's made it in this release from over 30 developers. So a big hug to all these contributors!

https://github.com/MvvmCross/MvvmCross/pulls?q=is%3Apr+is%3Aclosed+milestone%3A5.0.0

## Open Collective

As you all know MvvmCross is an Open Source project, so that means we're not making any money out of it. But sometimes we're facing actual costs which is always difficult to arrange. To improve on this situation we've created the MvvmCross Open Collective - a place where you can donate your money to the project but also have full insight to what we're actually doing with it. We really hope you're going to join this Open Collective!

https://opencollective.com/mvvmcross
