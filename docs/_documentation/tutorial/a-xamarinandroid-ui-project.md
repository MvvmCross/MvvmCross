---
layout: documentation
title: A Xamarin.Android UI Project
category: Tutorials
---
We started with the goal of creating an app to help calculate what tip to leave in a restaurant.

We had a plan to produce a UI based on this concept:

![TipCalc interface](https://mvvmcross.github.io/mvvmcross-docs/img/tipcalc.png)


To satisfy this we built a 'Core' Portable Class Library project which contained:

- Our 'business logic' - `ICalculation`
- Our ViewModel - `TipViewModel`
- Our `App` which contains the application wiring, including the start instructions.

We're now ready to add out first User Interface.

So... let's start with Android.

To create an Android MvvmCross UI, you can use the Visual Studio project template wizards, but here we'll instead build up a new project 'from empty', just as we did for the Core project.

## Create a new Android UI Project

Add a new project to your solution - a 'Blank App (Android)' application with name `TipCalc.UI.Droid`

Within this, you'll find the normal Android application constructs:

- the Assets folder
- the Resources folder
- the MainActivity.cs


## Delete MainActivity.cs

No-one really needs a `MainActivity` :)

Also, delete `Main.axml` in the /resources/Layout folder.

## Install MvvmCross

In the Package Manager Console, enter...

    Install-Package MvvmCross.Binding

## Add a reference to TipCalc.Core.csproj

Add a reference to your `TipCalc.Core` project - the project we created in the last step which included:

- Your `Calculation` service,
- Your `TipViewModel`
- Your `App` wiring.


## Add a Setup class

Every MvvmCross UI project requires a `Setup` class.

This class sits in the root namespace (folder) of our UI project and performs the initialisation of the MvvmCross framework and your application, including:

- The Inversion of Control (IoC) system
- The MvvmCross data-binding
- Your `App` and its collection of `ViewModel`s
- Your UI project and its collection of `View`s

Most of this functionality is provided for you automatically. Within your Droid UI project all you have to supply are:

- your `App` - your link to the business logic and `ViewModel` content

For `TipCalc` here's all that is needed in Setup.cs:

```
using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using TipCalc.Core;

namespace TipCalc.UI.Droid
{
      public class Setup : MvxAndroidSetup
      {
            public Setup(Context applicationContext)
              : base(applicationContext)
            {
            }

            protected override IMvxApplication CreateApp()
            {
                  return new App();
            }
      }
}
```


## Add your View

### Add the Android Layout XML (AXML)

This tutorial doesn't attempt to give an introduction to Android XML layout.

Instead all I'll say here is the bare minimum. If you are new to Android, then you can find out more about Android XML from lots of places including the official documentation at [this site](http://developer.android.com/guide/topics/ui/declaring-layout.html). If you are coming from a XAML background - you are a *XAMLite* - then I'll include some simple XAML-AXML comparisons to help you out.

To achieve the basic layout:

- We'll add a new AXML file - `View_Tip.axml` in the `/Resources/Layout` folder

- We'll edit this using either the Xamarin Android designer or the Visual Studio XML editor - the designer gives us a visual display, while the VS editor **sometimes** gives us XML Intellisense.

```
<?xml version="1.0" encoding="utf-8"?>
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
    android:orientation="vertical"
    android:layout_width="match_parent"
    android:layout_height="match_parent" />
```

- We'll add a local app namespace - `http://schemas.android.com/apk/res-auto` - this is just like adding a namespace in XAML.


```
<?xml version="1.0" encoding="utf-8"?>
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:local="http://schemas.android.com/apk/res-auto"
    android:orientation="vertical"
    android:layout_width="match_parent"
    android:layout_height="match_parent" />
```

- Notice that the `LinearLayout` has by default a **horizontal** orientation - for XAMLites, this layout is just like a `StackPanel` except that it is **very important** to specify the **vertical** orientation

- Within this layout we'll add some `TextView`s to provide some static text labels - for XAMLites, these are like `TextBlock`s

```
<TextView
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:text="SubTotal" />
<TextView
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:text="Generosity" />
<TextView
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:text="Tip to leave" />
```

- We'll also add a short, wide `View` with a yellow background to provide a small amount of chrome:

```
<View
    android:layout_width="match_parent"
    android:layout_height="1dp"
    android:background="#ffff00" />
```

- We'll add some `View`s for data display and entry, and we'll **databind** these `View`s to properties in our `TipViewModel`

  - An `EditText` for text data entry of the `SubTotal` - for XAMLites, this is a `TextBox`

```
<EditText
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    local:MvxBind="Text SubTotal" />
```

  - A `SeekBar` for touch/slide entry of the `Generosity` - for XAMLites, this is like a `ProgressBar`:

```
<SeekBar
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:max="100"
    local:MvxBind="Progress Generosity" />
```

  - We'll add a `TextView` to display the `Tip` that results from the calculation:

```
<TextView
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    local:MvxBind="Text Tip" />
```

Put together, this looks like:

```
<?xml version="1.0" encoding="utf-8"?>
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:local="http://schemas.android.com/apk/res-auto"
    android:orientation="vertical"
    android:layout_width="match_parent"
    android:layout_height="match_parent">
    <TextView
          android:layout_width="match_parent"
          android:layout_height="wrap_content"
          android:text="SubTotal" />
    <EditText
          android:layout_width="match_parent"
          android:layout_height="wrap_content"
          local:MvxBind="Text SubTotal" />
    <TextView
          android:layout_width="match_parent"
          android:layout_height="wrap_content"
          android:text="Generosity" />
    <SeekBar
          android:layout_width="match_parent"
          android:layout_height="wrap_content"
          android:max="100"
          local:MvxBind="Progress Generosity" />
    <View
          android:layout_width="match_parent"
          android:layout_height="1dp"
          android:background="#ffff00" />
    <TextView
          android:layout_width="match_parent"
          android:layout_height="wrap_content"
          android:text="Tip to leave" />
    <TextView
          android:layout_width="match_parent"
          android:layout_height="wrap_content"
          local:MvxBind="Text Tip" />
</LinearLayout>
```


### About the data-binding syntax

Each of the data-binding blocks within our first sample looks similar:

`local:MvxBind="Text SubTotal"`

What this line means is:

- Data-bind:
  - The property `Text` on the `View`
  - To the property `SubTotal` on the `DataContext` - which in this case will be a `TipViewModel`
- So:
  - Whenever the `TipViewModel` calls `RaisePropertyChanged` on `SubTotal` then the `View` should update
  - And whenever the user enters text into the `View` then the `SubTotal` value should be `set` on the `TipViewModel`

Note that this `TwoWay` binding is **different** to XAML where generally the default `BindingMode` is only `OneWay`.

In later topics, we'll return to show you more options for data-binding, including how to use `ValueConverter`s, but for now all our binding uses this simple `ViewProperty ViewModelProperty` syntax.


### Add the View class

With our AXML layout complete, we can now add the C# `Activity` which is used to display this content. For developers coming from XAML backgrounds, these `Activity` classes are roughly equivalent to `Page` objects in WindowsPhone on WindowsStore applications - they own the 'whole screen' and have a lifecycle which means that only one of them is shown at any one time.

To create our `Activity` - which will also be our MVVM `View`:

- Create a Views folder within your TipCalc.UI.Droid project

- Within this folder create a new C# class - `TipView`

Not that the name of this class **MUST** match the name of the viewmodel. As our viewmodel is called TipViewModel our class must be named TipView).

- This class will:

  - Inherit from `MvxActivity`:


```
public class TipCalcView : MvxActivity"
```

   - Be marked with the Xamarin.Android `Activity` attribute, marking it as the `MainLauncher` for the project:

```
[Activity(Label = \"Tip\", MainLauncher=true)]",
```

  - Use `OnViewModelSet` to inflate its `ContentView` from the AXML - this will use a resource identifier generated by the Android and Xamarin tools.

```
protected override void OnViewModelSet()
{
      SetContentView(Resource.Layout.View_Tip);
}
```

As a result this completed class is very simple:


```
using Android.App;
using MvvmCross.Droid.Views;

namespace TipCalc.UI.Droid.Views
{
      [Activity(Label = \"Tip\", MainLauncher = true)]
      public class TipView : MvxActivity
      {
            protected override void OnViewModelSet()
            {
                  SetContentView(Resource.Layout.View_Tip);
            }
      }
}
```


## The Android UI is complete!

At this point you should be able to run your application.

When it starts... you should see:

![Android TipCalc](https://mvvmcross.github.io/mvvmcross-docs/img/TipCalc_Android.png)

If you then want to make it 'more beautiful', then try adding a few attributes to some of your AXML - things like:

        android:background="#00007f"
        android:textColor="#ffffff"
        android:textSize="24dp"
        android:layout_margin="30dp"
        android:padding="20dp"
        android:layout_marginTop="10dp"

Within a very short time, you should hopefully be able to create something 'styled'...

![Android TipCalc Styled](https://mvvmcross.github.io/mvvmcross-docs/img/TipCalc_Android_Styled.png)

... but actually making it look 'nice' might take some design skills!


## Moving on

There's more we could do to make this User Interface nicer and to make the app richer... but for this first application, we will leave it here for now.

Let's move on to Xamarin.iOS and to Windows!
