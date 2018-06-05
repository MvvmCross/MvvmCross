---
layout: documentation
title: TipCalc WPF Project
category: Tutorials
order: 6
---

We started with the goal of creating an app to help calculate what tip to leave in a restaurant.

We had a plan to produce a UI based on this concept:

![TipCalc]({{ site.url }}/assets/img/tutorials/tipcalc/TipCalc_Sketch.png)

To satisfy this we built a 'Core' .NET Standard project which contained:

- Our 'business logic' - `ICalculationService`
- Our ViewModel - `TipViewModel`
- Our `App` - which contains some bootstrapping code.

We even added User Interfaces for Xamarin.Android, Xamarin.iOS and UWP so far.

Now let's build a UI for WPF (Windows Presentation Foundation)!

Once again, we will build up a new project 'from empty', just as we did before.

## Create a new WPF Project

Add a new project to your solution - a 'WPF App (.NET Framework)' named `TipCalc.WPF`.

Within this, you will find the normal WPF application constructs:

- The `Properties` folder with the `AssemblyInfo` file, some resources and a settings file
- The `App.config` configuration file
- The `App.xaml` file, which we will edit shortly
- A `MainWindow.xaml` and `MainWindow.xaml.cs` files that define a default Window for this app

## Keep(!) MainWindow.xaml

We do actually want a `MainWindow` for this app :)

## Install MvvmCross

Open the Nuget Package Manager and search for the package `MvvmCross`.

If you don't really enjoy the NuGet UI experience, then you can alternatively open the Package Manager Console, and type:

    Install-Package MvvmCross

## Install MvvmCross.Platforms.Wpf

Same as you did with the `MvvmCross` package, install the specific one for `Wpf`.

## Add a reference to TipCalc.Core project

Add a reference to your `TipCalc.Core` project - the project we created in the first step.

## Edit App.xaml.cs

WPF will be an easy addition if you have followed the article for UWP. On this platform, the `App` class also plays a very important role, as it provides a set of callback that the OS uses to inform you about events in your application's lifecycle. We won't dig further into it's responsibilities, but you may want to read about it in the official documentation.

Open the `App.xaml.cs` and delete all the class content. Leave only the default class and make it extend `MvxApplication`:

```c#
public partial class App : MvxApplication
```

Now override the method `RegisterSetup` and use the object extension method `RegisterSetupType`:

```c#
this.RegisterSetupType<MvxWpfSetup<Core.App>>();
```

This line of code tells MvvmCross we want to use the default provided Setup class, and also that our _Core_ application is `Core.App`.

Altogether this is what your App.xaml.cs should loook like:

```c#
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Views;

namespace TipCalc.WPF
{
    public partial class App : MvxApplication
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<MvxWpfSetup<Core.App>>();
        }
    }
}
```

## Edit App.xaml

Now it's time to edit the xaml part of our WPF `App` class. Open the file and replace all the content for this code:

```xml
<views:MvxApplication
    xmlns:views="clr-namespace:MvvmCross.Platforms.Wpf.Views;assembly=MvvmCross.Platforms.Wpf"
    x:Class="TipCalc.WPF.App"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    StartupUri="MainWindow.xaml">
</views:MvxApplication>
```

That's it! We now only need to build the UI.

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

## Make MainWindow be a MvvmCross window

Open up `MainWindow.xaml.cs` and change the base class to `MvxWindow`:

```c#
using MvvmCross.Platforms.Wpf.Views;

namespace TipCalc.WPF
{
    public partial class MainWindow : MvxWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
```

Now open the XAML document and apply the same change:

```xml
<views:MvxWindow  x:Class="TipCalc.WPF.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:TipCalc.WPF"
        xmlns:views="clr-namespace:MvvmCross.Platforms.Wpf.Views;assembly=MvvmCross.Platforms.Wpf"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <Grid>        
    </Grid>
</views:MvxWindow>
```

## Add your View

### Create the UserControl

Create a folder named `Views`.

Within this folder, add a new 'User Control (WPF)' and call it `TipView.xaml`

This will generate two files:

- `TipView.xaml`
- `TipView.xaml.cs`

### Turn TipView into the MvvmCross View for TipViewModel

Open the `TipView.xaml.cs` file.

Change the class to inherit from `MvxWpfView`

```c#
public partial class TipView : MvxWpfView
```

Altogether this looks like:

```c#
using MvvmCross.Platforms.Wpf.Views;

namespace TipCalc.WPF.Views
{
    public partial class TipView : MvxWpfView
    {
        public TipView()
        {
            InitializeComponent();
        }
    }
}
```

### Edit the XAML layout

Double click on the XAML file. At this point we will just assume you understand the basics of XAML editing. If you need further explanations on that, please take a read at the WPF official documentation. We will concentrate on the MvvmCross specific bits only.

Change the root node from:

```xml
<UserControl 
         ...>
</UserControl>
```

To:

```xml
<views:MvxWpfView
	xmlns:views="clr-namespace:MvvmCross.Platforms.Wpf.Views;assembly=MvvmCross.Platforms.Wpf"               
        ...>
</views:MvxWpfView>
```

We now need to fill the content of the view with some widgets:

- Add a `StackPanel` container. Then inside of it, add:
    - A `TextBlock` which text should be `SubTotal`
    - A bound `TextBox` for the `SubTotal` property of `TipViewModel`
    - A `TextBlock` which text should be `Generosity`
    - A bound `Slider` for the `Generosity` property of `TipViewModel`
    - A `TextBlock` which text should be `Tip to leave`
    - A bound `TextBlock` for the `Tip` property of `TipViewModel`


This will produce finished XAML like:

```xml
<views:MvxWpfView x:Class="TipCalc.WPF.Views.TipView"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
      xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
      xmlns:views="clr-namespace:MvvmCross.Platforms.Wpf.Views;assembly=MvvmCross.Platforms.Wpf"
      mc:Ignorable="d" 
      d:DesignHeight="450" d:DesignWidth="800">
    <Grid>
        <StackPanel Margin="12,0,12,0">
            <TextBlock Text="SubTotal" />
            <TextBox Text="{Binding SubTotal, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />
            <TextBlock Text="Generosity" />
            <Slider Value="{Binding Generosity,Mode=TwoWay}" 
                    SmallChange="1" 
                    LargeChange="10" 
                    Minimum="0" 
                    Maximum="100" />
            <TextBlock Text="Tip to leave" />
            <TextBlock Text="{Binding Tip}" />
        </StackPanel>
    </Grid>
</views:MvxWpfView>

```

**Note** that in XAML, `OneWay` binding is generally the default. To provide TwoWay binding we explicitly add `Mode` to our binding expressions: e.g. `Value="{Binding Generosity,Mode=TwoWay}"`

**Second note** the binding for the TextBox uses `UpdateSourceTrigger=PropertyChanged` so that the `SubTotal` property of `TipViewModel` is updated immediately rather than when the TextBox loses focus.

Although this sample only shows simple bindings, the infrastructure built within MvvmCross is really powerful! Our data-binding engine supports ValueConverters, ValueCombiners, FallbackValues, different modes of bindings and a super straight forward mechanism to add your own custom bindings.

## The Wpf UI is complete!

At this point you should be able to run your application.

When it starts... you should see something like this:

![TipCalc WPF Run]({{ site.url }}/assets/img/tutorials/tipcalc/TipCalc_WPF.png)

## Moving on...

There's more we could do to make this User Interface nicer and to make the app richer... but for this first application, we will leave it here for now.

Stay tuned, because for the next step, we will completely change the approach and build a Xamarin.Forms version.

[Next!](https://www.mvvmcross.com/documentation/tutorials/tipcalc/a-xamarin-forms-version)