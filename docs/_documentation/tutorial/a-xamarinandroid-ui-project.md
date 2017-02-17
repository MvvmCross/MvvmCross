---
layout: documentation
title: A Xamarin.Android UI Project
category: Tutorials
---
We started with the goal of creating an app to help calculate what tip to leave in a restaurant.

We had a plan to produce a UI based on this concept:
[block:image]
{
  "images": [
    {
      "image": [
        "https://files.readme.io/kN3s1ie6RxOGzpiFMJJ1_68747470733a2f2f7261772e6769746875622e636f6d2f736c6f6467652f4d76766d43726f73732f76332f76335475746f7269616c2f50696374757265732f54697043616c635f536b657463682e706e67",
        "68747470733a2f2f7261772e6769746875622e636f6d2f736c6f6467652f4d76766d43726f73732f76332f76335475746f7269616c2f50696374757265732f54697043616c635f536b657463682e706e67",
        "250",
        "449",
        "#7ea0e9",
        ""
      ]
    }
  ]
}
```
To satisfy this we built a 'Core' Portable Class Library project which contained:

* our 'business logic' - `ICalculation`
* our ViewModel - `TipViewModel`
* our `App` which contains the application wiring, including the start instructions.

We're now ready to add out first User Interface.

So... let's start with Android.

To create an Android MvvmCross UI, you can use the Visual Studio project template wizards, but here we'll instead build up a new project 'from empty', just as we did for the Core project.

## Create a new Android UI Project

Add a new project to your solution - a 'Blank App (Android)' application with name `TipCalc.UI.Droid`

Within this, you'll find the normal Android application constructs:

* the Assets folder
* the Resources folder
* the MainActivity.cs

## Delete MainActivity.cs

No-one really needs an `MainActivity` :)

Also, delete `Main.axml` in the /resources/Layout folder.

## Install MvvmCross

In the Package Manager Console, enter...

    Install-Package MvvmCross.Binding
    
## Add a reference to TipCalc.Core.csproj

Add a reference to your `TipCalc.Core` project - the project we created in the last step which included:

* your `Calculation` service, 
* your `TipViewModel` 
* your `App` wiring.

## Add a Setup class

Every MvvmCross UI project requires a `Setup` class.

This class sits in the root namespace (folder) of our UI project and performs the initialisation of the MvvmCross framework and your application, including:

  * the Inversion of Control (IoC) system
  * the MvvmCross data-binding
  * your `App` and its collection of `ViewModel`s
  * your UI project and its collection of `View`s

Most of this functionality is provided for you automatically. Within your Droid UI project all you have to supply are:

- your `App` - your link to the business logic and `ViewModel` content

For `TipCalc` here's all that is needed in Setup.cs:
```c# 
using Android.Content;\nusing MvvmCross.Core.ViewModels;\nusing MvvmCross.Droid.Platform;\nusing TipCalc.Core;\n\nnamespace TipCalc.UI.Droid\n{\n    public class Setup : MvxAndroidSetup\n    {\n        public Setup(Context applicationContext)\n            : base(applicationContext)\n        {\n        }\n\n        protected override IMvxApplication CreateApp()\n        {\n            return new App();\n        }\n    }\n}",
```
## Add your View

### Add the Android Layout XML (AXML)

This tutorial doesn't attempt to give an introduction to Android XML layout.

Instead all I'll say here is the bare minimum. If you are new to Android, then you can find out more about Android XML from lots of places including the official documentation at: http://developer.android.com/guide/topics/ui/declaring-layout.html. If you are coming from a XAML background - you are a *XAMLite* - then I'll include some simple XAML-AXML comparisons to help you out.

To achieve the basic layout:

- we'll add a new AXML file - `View_Tip.axml` in the `/Resources/Layout` folder

- we'll edit this using either the Xamarin Android designer or the Visual Studio XML editor - the designer gives us a visual display, while the VS editor *sometimes* gives us XML Intellisense.
```c# 
<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<LinearLayout xmlns:android=\"http://schemas.android.com/apk/res/android\"\n    android:orientation=\"vertical\"\n    android:layout_width=\"match_parent\"\n    android:layout_height=\"match_parent\" />",
      "language": "xml"
    }
  ]
}
```
- we'll add a local app namespace - `http://schemas.android.com/apk/res-auto` - this is just like adding a namespace in XAML.
```c# 
<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<LinearLayout xmlns:android=\"http://schemas.android.com/apk/res/android\"\n    xmlns:local=\"http://schemas.android.com/apk/res-auto\"\n    android:orientation=\"vertical\"\n    android:layout_width=\"match_parent\"\n    android:layout_height=\"match_parent\" />",
      "language": "xml"
    }
  ]
}
```
- notice that this 'layout' is already by default a **vertical** `LinearLayout` - for XAMLites, this layout is just like a `StackPanel` except that it is **very important** to specify the **vertical** orientation

- within this layout we'll add some `TextView`s to provide some static text labels - for XAMLites, these are like `TextBlock`s
```c# 
<TextView\n    android:layout_width=\"match_parent\"\n    android:layout_height=\"wrap_content\"\n    android:text=\"SubTotal\" />\n<TextView\n    android:layout_width=\"match_parent\"\n    android:layout_height=\"wrap_content\"\n    android:text=\"Generosity\" />\n<TextView\n    android:layout_width=\"match_parent\"\n    android:layout_height=\"wrap_content\"\n    android:text=\"Tip to leave\" /",
      "language": "xml"
    }
  ]
}
```
- we'll also add a short, wide `View` with a yellow background to provide a small amount of chrome:
```c# 
<View\n    android:layout_width=\"match_parent\"\n    android:layout_height=\"1dp\"\n    android:background=\"#ffff00\" />",
      "language": "xml"
    }
  ]
}
```
- we'll add some `View`s for data display and entry, and we'll **databind** these `View`s to properties in our `TipViewModel` 

  - an `EditText` for text data entry of the `SubTotal` - for XAMLites, this is a `TextBox`
```c# 
<EditText\n    android:layout_width=\"match_parent\"\n    android:layout_height=\"wrap_content\"\n    local:MvxBind=\"Text SubTotal\" />",
      "language": "xml"
    }
  ]
}
```
  - a `SeekBar` for touch/slide entry of the `Generosity` - for XAMLites, this is like a `ProgressBar`
```c# 
<SeekBar\n    android:layout_width=\"match_parent\"\n    android:layout_height=\"wrap_content\"\n    android:max=\"100\"\n    local:MvxBind=\"Progress Generosity\" />",
      "language": "xml"
    }
  ]
}
```
  - we'll add a `TextView` to display the `Tip` that results from the calculation:
```c# 
<TextView\n    android:layout_width=\"match_parent\"\n    android:layout_height=\"wrap_content\"\n    local:MvxBind=\"Text Tip\" />",
      "language": "xml"
    }
  ]
}
```
Put together, this looks like:
```c# 
<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<LinearLayout xmlns:android=\"http://schemas.android.com/apk/res/android\"\n    xmlns:local=\"http://schemas.android.com/apk/res-auto\"\n    android:orientation=\"vertical\"\n    android:layout_width=\"match_parent\"\n    android:layout_height=\"match_parent\">\n  <TextView\n      android:layout_width=\"match_parent\"\n      android:layout_height=\"wrap_content\"\n      android:text=\"SubTotal\" />\n  <EditText\n      android:layout_width=\"match_parent\"\n      android:layout_height=\"wrap_content\"\n      local:MvxBind=\"Text SubTotal\" />\n  <TextView\n      android:layout_width=\"match_parent\"\n      android:layout_height=\"wrap_content\"\n      android:text=\"Generosity\" />\n  <SeekBar\n      android:layout_width=\"match_parent\"\n      android:layout_height=\"wrap_content\"\n      android:max=\"100\"\n      local:MvxBind=\"Progress Generosity\" />\n  <View\n      android:layout_width=\"match_parent\"\n      android:layout_height=\"1dp\"\n      android:background=\"#ffff00\" />\n  <TextView\n      android:layout_width=\"match_parent\"\n      android:layout_height=\"wrap_content\"\n      android:text=\"Tip to leave\" />\n  <TextView\n      android:layout_width=\"match_parent\"\n      android:layout_height=\"wrap_content\"\n      local:MvxBind=\"Text Tip\" />\n</LinearLayout>",
      "language": "xml"
    }
  ]
}
```
### About the data-binding syntax 

Each of the data-binding blocks within our first sample looks similar:

`local:MvxBind="Text SubTotal"`

What this line means is:

* data-bind:
  * the property `Text` on the `View`
  * to the property `SubTotal` on the `DataContext` - which in this case will be a `TipViewModel`
* so:
  * whenever the `TipViewModel` calls `RaisePropertyChanged` on `SubTotal` then the `View` should update
  * and whenever the user enters text into the `View` then the `SubTotal` value should be `set` on the `TipViewModel`

Note that this `TwoWay` binding is **different** to XAML where generally the default `BindingMode` is only `OneWay`.

In later topics, we'll return to show you more options for data-binding, including how to use `ValueConverter`s, but for now all our binding uses this simple `ViewProperty ViewModelProperty` syntax.

### Add the View class

With our AXML layout complete, we can now add the C# `Activity` which is used to display this content. For developers coming from XAML backgrounds, these `Activity` classes are roughly equivalent to `Page` objects in WindowsPhone on WindowsStore applications - they own the 'whole screen' and have a lifecycle which means that only one of them is shown at any one time.

To create our `Activity` - which will also be our MVVM `View`:

* Create a Views folder within your TipCalc.UI.Droid project

* Within this folder create a new C# class - `TipView`

Not that tThe name of this class **MUST** match the name of the viewmodel. As our viewmodel is called TipViewModel our class must be named TipView).

* This class will:

 * inherit from `MvxActivity`
```c# 
public class TipCalcView : MvxActivity",
```
 * be marked with the Xamarin.Android `Activity` attribute, marking it as the `MainLauncher` for the project
```c# 
[Activity(Label = \"Tip\", MainLauncher=true)]",
```
  * use `OnViewModelSet` to inflate its `ContentView` from the AXML - this will use a resource identifier generated by the Android and Xamarin tools. 
```c# 
protected override void OnViewModelSet()\n{\n    SetContentView(Resource.Layout.View_Tip);\n}",
```
As a result this completed class is very simple:
```c# 
using Android.App;\nusing MvvmCross.Droid.Views;\n\nnamespace TipCalc.UI.Droid.Views\n{\n    [Activity(Label = \"Tip\", MainLauncher = true)]\n    public class TipView : MvxActivity\n    {\n        protected override void OnViewModelSet()\n        {\n            SetContentView(Resource.Layout.View_Tip);\n        }\n    }\n}",
```
## The Android UI is complete!

At this point you should be able to run your application.

When it starts... you should see:

![v1](https://raw.github.com/slodge/MvvmCross/v3/v3Tutorial/Pictures/TipCalc_Android.png)

If you then want to make it 'more beautiful', then try adding a few attributes to some of your AXML - things like:

        android:background="#00007f"
        android:textColor="#ffffff"
        android:textSize="24dp"
        android:layout_margin="30dp"
        android:padding="20dp"
        android:layout_marginTop="10dp"

Within a very short time, you should hopefully be able to create something 'styled'...

![v2](https://raw.github.com/slodge/MvvmCross/v3/v3Tutorial/Pictures/TipCalc_Android_Styled.png)

... but actually making it look 'nice' might take some design skills!
        
## Moving on

There's more we could do to make this User Interface nicer and to make the app richer... but for this first application, we will leave it here for now.

Let's move on to Xamarin.iOS and to Windows!