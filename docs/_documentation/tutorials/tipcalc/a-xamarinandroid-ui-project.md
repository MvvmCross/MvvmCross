---
layout: documentation
title: TipCalc Android Project
category: Tutorials
order: 3
---

We started with the goal of creating an app to help calculate what tip to leave in a restaurant.

We had a plan to produce a UI based on this concept:

![Sketch]({{ site.url }}/assets/img/tutorials/tipcalc/TipCalc_Sketch.png)

To satisfy this we built a 'Core' .NET Standard project which contained:

- Our 'business logic' - `ICalculationService`
- Our ViewModel - `TipViewModel`
- Our `App` - which contains some bootstrapping code.

We're now ready to add out first User Interface!

So... let's start with Android.

Same as we did with the _Core_ project, we will use a standard template to create the Android project.

## Create a new Android project

Add a new project to your solution - a 'Blank App (Android)' application with name `TipCalc.Droid`

Within this, you'll find the normal Android application constructs:

- The Assets folder
- The Resources folder
- The MainActivity.cs

## Delete MainActivity.cs

No-one really needs that `MainActivity` :)

Also, make sure you delete `Main.axml` in the /resources/Layout folder.

## Install MvvmCross

Open the Nuget Package Manager and search for the package `MvvmCross`.

If you don't really enjoy the NuGet UI experience, then you can alternatively open the Package Manager Console, and type:

    Install-Package MvvmCross

## Add a reference to TipCalc.Core project

Add a reference to your `TipCalc.Core` project - the project we created in the first step.

## Add an Android Application class

The Android Application class will allow us to specify the MvvmCross framework some key classes to be used for initialization:

```c#
using System;
using Android.App;
using Android.Runtime;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;
using TipCalc.Core;

namespace TipCalc.Droid
{
    [Application]
    public class MainApplication : MvxAndroidApplication<MvxAndroidSetup<App>, App>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
    }
}
```

We won't go deeper into what role does MainApplication have on the Android platform, but let's talk a bit about the MvvmCross bits:

- `MvxAndroidApplication` provides some behavior for initializing the framework in runtime - although it isn't really the only way to configure the Android project.
- `MvxAndroidSetup` is the default setup MvvmCross contains. But you can use your own customized setup - sometimes it's necessary and it's a good place to initialize some 3rd party libraries on it.
- `App` here is a reference to our `TipCalc.Core.App` class.

### Some more details about the Setup class

Every MvvmCross UI project requires a `Setup` class, but if your app is fairly simple, like the TipCalc is, then you can safely use the default one, provided by the framework.

The `Setup` class is responsible for performing the initialization of the MvvmCross framework, including:

- The IoC Container and DI engine
- The Data-Binding engine
- The ViewModel / View lookups
- The whole navigation system
- Plugins

Finally, the `Setup` class is also responsible for initializing your `App` class.

Luckily for us, all this functionality is provided for you automatically, unless you want / need to use a custom `Setup` class (since it is an excellent place to register your own services / plugins, it is often the case).

## Add your View

### Add the Android Layout XML (AXML)

This tutorial doesn't attempt to give an introduction to Android XML layout, but any knowledge is actually really necessary at this point. If you are very new to Android, you can read more about Android XML on the [official documentation](http://developer.android.com/guide/topics/ui/declaring-layout.html). 

To achieve the basic layout that we need:

- We will add a new .axml file - called `TipView.axml` into the `/Resources/Layout` folder.

- We will edit this file using the XML editor - the designer gives us a visual display, while the VS editor **sometimes** gives us XML Intellisense. Open the file, go to the "Source" tab and replace the file content with the following code:

```xml
<?xml version="1.0" encoding="utf-8"?>
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
    android:orientation="vertical"
    android:layout_width="match_parent"
    android:layout_height="match_parent" />
```

- We will now add a local app namespace - `http://schemas.android.com/apk/res-auto` - which is just like adding a namespace in XAML:

```xml
<?xml version="1.0" encoding="utf-8"?>
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:local="http://schemas.android.com/apk/res-auto"
    android:orientation="vertical"
    android:layout_width="match_parent"
    android:layout_height="match_parent" />
```

- Notice that the `LinearLayout` has by default a **horizontal** orientation - for XAMLites, this layout is just like a `StackPanel` except that it is **very important** to specify the **vertical** orientation

- Within this layout we will add some `TextView`s to provide some static text labels:

```xml
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

- We will also add a short, wide `View` with a yellow background to provide a small amount of chrome after the last `TextView`:

```xml
<View
    android:layout_width="match_parent"
    android:layout_height="1dp"
    android:background="#ffff00" />
```

- Now it's time to add some `View`s for data display and data entry, which we will also **databind** to properties in our `TipViewModel`:

  - Add an `EditText` for text data entry of the `SubTotal`:

```xml
<EditText
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    local:MvxBind="Text SubTotal" />
```

  - Add a `SeekBar` for touch/slide entry of the `Generosity`:

```xml
<SeekBar
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:max="100"
    local:MvxBind="Progress Generosity" />
```

  - Now for a last step, add a `TextView` to display the final `Tip` result:

```xml
<TextView
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    local:MvxBind="Text Tip" />
```

Putting everything together, your .axml file should look like this:

```xml
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

You may have noticed that each of the data-binding blocks within our first sample look very similar, for example:

```xml
local:MvxBind="Text SubTotal"
```

What this line means is:

Data-Binding:
- The property `Text` of the `TextView`
- To the property `SubTotal` on the `DataContext` - which in this case will be a `TipViewModel`.

This means:
- Whenever the `TipViewModel` calls `RaisePropertyChanged` on `SubTotal` then the `View` should update its content
- And whenever the user enters text into the `View`, the `SubTotal` value should be `set` on the `TipViewModel`

Note that this `TwoWay` binding is **different** to XAML where generally the default `BindingMode` is only `OneWay`.

### Add the View class

With our .axml layout complete, we can now move back to C# and add an `Activity`, which is used to display the content. These `Activity` classes are very special objects on Android, which provide a `context` to your app and a placeholder to display widgets on the UI.

This `Activity` will act as our MVVM `View`. Please follow these steps:

- Create a `Views` folder within your TipCalc.Droid project.

- Within this folder create a new C# class called `TipView`.

- This class should inherit from `MvxActivity<TipViewModel>`:

```c#
public class TipView : MvxActivity<TipViewModel>"
```

- Add an `Activity` attribute over the class and set the `MainLauncher` property to `true`. This attribute lets Xamarin.Android add it automatically to your AndroidManifest file:

```c#
[Activity(Label = "Tip Calculator", MainLauncher = true)]
```

- Override the method `OnCreate` and call `SetContentView()` right after the call to base:

```c#
protected override void OnCreate(Bundle bundle)
{
    base.OnCreate(bundle);
    SetContentView(Resource.Layout.TipView);
}
```

As a result this completed class is very simple:

```c#
using Android.App;
using Android.OS;
using MvvmCross.Platforms.Android.Views;
using TipCalc.Core.ViewModels;

namespace TipCalc.Droid.Views
{
    [Activity(Label = "Tip Calculator", MainLauncher = true)]
    public class TipView : MvxActivity<TipViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.TipView);
        }
    }
}
```

## The Android project is complete!

At this point you should be able to run your application.

When it starts... you should see something like this:

![Android TipCalc]({{ site.url }}/assets/img/tutorials/tipcalc/TipCalc_Android.png)

If you then want to make it 'more beautiful', then try adding a few attributes to some of your .axml - things like:

```xml
android:background="#00007f"
android:textColor="#ffffff"
android:textSize="24dp"
android:layout_margin="30dp"
android:padding="20dp"
android:layout_marginTop="10dp"
```

## Moving on

There's more we could do to make this User Interface nicer and to make the app richer... but for this first application, we will leave it here for now.

Let's move on to Xamarin.iOS!

[Next!](https://www.mvvmcross.com/documentation/tutorials/tipcalc/a-xamarinios-ui-project)

