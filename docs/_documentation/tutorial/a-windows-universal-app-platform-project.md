---
layout: documentation
title: A Windows Universal App Platform Project
category: Tutorials
---
We started with the goal of creating an app to help calculate what tip to leave in a restaurant

We had a plan to produce a UI based on this concept:

![Sketch](https://raw.github.com/slodge/MvvmCross/v3/v3Tutorial/Pictures/TipCalc_Sketch.png)

To satisfy this we built a 'Core' Portable Class Library project which contained:

* our 'business logic' - `ICalculation`
* our ViewModel - `TipViewModel`
* our `App` which contains the application wiring, including the start instructions.

We then added User Interfaces for Xamarin.Android and Xamarin.iOS

![Android](https://raw.github.com/slodge/MvvmCross/v3/v3Tutorial/Pictures/TipCalc_Android_Styled.png) ![v1](https://raw.github.com/slodge/MvvmCross/v3/v3Tutorial/Pictures/TipCalc_Touch_Sim.png)

For our next project, let's look at Windows, specifically Universal Windows Platform (UWP) Apps which run on Windows 10 and Windows 10 Mobile.

To create a Windows UWP MvvmCross UI, you can use the Visual Studio project template wizards, but here we'll instead build up a new project 'from empty', just as we did for the Core and other UI projects.

## Create a new Windows UWP Project

Add a new project to your solution - a 'Blank App (Universal Windows)' application with name `TipCalc.UI.UWP`

Within this, you'll find the normal WindowsStore application constructs:

* the 'Properties' folder with just the 'AssemblyInfo' file
* the 'Assets' folder
* the 'Common' folder
* the App.Xaml 'application' object
* the MainPage.Xaml and MainPage.Xaml.cs files that define the default Page for this app
* the 'Package.appxmanifest' configuration file
* the 'project.json'
* the debug private key for your development

## Delete MainPage.xaml

No-one really needs a `MainPage` :)

## Install MvvmCross

In the Package Manager Console, enter...

    Install-Package MvvmCross.Core

## Add a reference to TipCalc.Core.csproj

Add a reference to your `TipCalc.Core` project - the project we created in the last step which included:

* your `Calculation` service, 
* your `TipViewModel` 
* your `App` wiring.

## Add a Setup class

Just as we said during the Android, iOS and WO construction *Every MvvmCross UI project requires a `Setup` class*

This class sits in the root namespace (folder) of our UI project and performs the initialisation of the MvvmCross framework and your application, including:

  * the Inversion of Control (IoC) system
  * the MvvmCross data-binding
  * your `App` and its collection of `ViewModel`s
  * your UI project and its collection of `View`s

Most of this functionality is provided for you automatically. Within your Windows UWP UI project all you have to supply is:

- your `App` - your link to the business logic and `ViewModel` content.

For `TipCalc` here's all that is needed in Setup.cs:
```c# 
using Windows.UI.Xaml.Controls;\nusing MvvmCross.Core.ViewModels;\nusing MvvmCross.WindowsUWP.Platform;\n\nnamespace TipCalc.UI.UWP\n{\n    public class Setup : MvxWindowsSetup\n    {\n        public Setup(Frame rootFrame) : base(rootFrame)\n        {\n        }\n\n        protected override IMvxApplication CreateApp()\n        {\n            return new Core.App();\n        }\n    }\n}",
```
## Modify the App.xaml.cs to use Setup

Your `App.xaml.cs` provides the Windows UWP 'main application' object - an object which owns the User Interface and receives some callbacks from the operating system during some key events in your application's lifecycle.

To modify this `App.xaml.cs` for MvvmCross, we need to:

* modify the `OnLaunched` callback

 * replace this lines
```c# 
// When the navigation stack isn't restored navigate to the first page,\n// configuring the new page by passing required information as a navigation\n// parameter\nrootFrame.Navigate(typeof(MainPage), e.Arguments);",
```
  * with these lines to allow it to create `Setup`, and to then initiate the `IMvxAppStart` `Start` navigation
```c# 
var setup = new Setup(rootFrame);\nsetup.Initialize();\n\nvar start = Mvx.Resolve<IMvxAppStart>();\nstart.Start();",
```
After you've done this your code might look like:
```c# 
using System;\nusing System.Collections.Generic;\nusing System.IO;\nusing System.Linq;\nusing System.Runtime.InteropServices.WindowsRuntime;\nusing Windows.ApplicationModel;\nusing Windows.ApplicationModel.Activation;\nusing Windows.Foundation;\nusing Windows.Foundation.Collections;\nusing Windows.UI.Xaml;\nusing Windows.UI.Xaml.Controls;\nusing Windows.UI.Xaml.Controls.Primitives;\nusing Windows.UI.Xaml.Data;\nusing Windows.UI.Xaml.Input;\nusing Windows.UI.Xaml.Media;\nusing Windows.UI.Xaml.Navigation;\nusing MvvmCross.Core.ViewModels;\nusing MvvmCross.Platform;\n\nnamespace TipCalc.UI.UWP\n{\n    /// <summary>\n    /// Provides application-specific behavior to supplement the default Application class.\n    /// </summary>\n    sealed partial class App : Application\n    {\n        /// <summary>\n        /// Initializes the singleton application object.  This is the first line of authored code\n        /// executed, and as such is the logical equivalent of main() or WinMain().\n        /// </summary>\n        public App()\n        {\n            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(\n                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |\n                Microsoft.ApplicationInsights.WindowsCollectors.Session);\n            this.InitializeComponent();\n            this.Suspending += OnSuspending;\n        }\n\n        /// <summary>\n        /// Invoked when the application is launched normally by the end user.  Other entry points\n        /// will be used such as when the application is launched to open a specific file.\n        /// </summary>\n        /// <param name=\"e\">Details about the launch request and process.</param>\n        protected override void OnLaunched(LaunchActivatedEventArgs e)\n        {\n\n#if DEBUG\n            if (System.Diagnostics.Debugger.IsAttached)\n            {\n                this.DebugSettings.EnableFrameRateCounter = true;\n            }\n#endif\n\n            Frame rootFrame = Window.Current.Content as Frame;\n\n            // Do not repeat app initialization when the Window already has content,\n            // just ensure that the window is active\n            if (rootFrame == null)\n            {\n                // Create a Frame to act as the navigation context and navigate to the first page\n                rootFrame = new Frame();\n\n                rootFrame.NavigationFailed += OnNavigationFailed;\n\n                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)\n                {\n                    //TODO: Load state from previously suspended application\n                }\n\n                // Place the frame in the current Window\n                Window.Current.Content = rootFrame;\n            }\n\n            if (rootFrame.Content == null)\n            {\n                //// When the navigation stack isn't restored navigate to the first page,\n                //// configuring the new page by passing required information as a navigation\n                //// parameter\n                //rootFrame.Navigate(typeof(MainPage), e.Arguments);\n                var setup = new Setup(rootFrame);\n                setup.Initialize();\n\n                var start = Mvx.Resolve<IMvxAppStart>();\n                start.Start();\n            }\n            // Ensure the current window is active\n            Window.Current.Activate();\n        }\n\n        /// <summary>\n        /// Invoked when Navigation to a certain page fails\n        /// </summary>\n        /// <param name=\"sender\">The Frame which failed navigation</param>\n        /// <param name=\"e\">Details about the navigation failure</param>\n        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)\n        {\n            throw new Exception(\"Failed to load Page \" + e.SourcePageType.FullName);\n        }\n\n        /// <summary>\n        /// Invoked when application execution is being suspended.  Application state is saved\n        /// without knowing whether the application will be terminated or resumed with the contents\n        /// of memory still intact.\n        /// </summary>\n        /// <param name=\"sender\">The source of the suspend request.</param>\n        /// <param name=\"e\">Details about the suspend request.</param>\n        private void OnSuspending(object sender, SuspendingEventArgs e)\n        {\n            var deferral = e.SuspendingOperation.GetDeferral();\n            //TODO: Save application state and stop any background activity\n            deferral.Complete();\n        }\n    }\n}",
```
## Add your View

### Create an initial Page

Create a Views folder

Within this folder, add a new 'Blank Page' and call it `TipView.xaml`

You will be asked if you want to add the missing 'Common' files automatically in order to support this 'Basic Page' - answer **Yes**

The page will generate:

* TipView.xaml
* TipView.xaml.cs

### Turn TipView into the MvvmCross View for TipViewModel

Change:

```public class TipView : Page```

To:

```public class TipView : MvxWindowsPage```

This requires the addition of:

```using MvvmCross.WindowsUWP.Views;```

Altogether this looks like:
```c# 
using MvvmCross.WindowsUWP.Views;\n\n// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238\n\nnamespace TipCalc.UI.UWP.Views\n{\n    /// <summary>\n    /// An empty page that can be used on its own or navigated to within a Frame.\n    /// </summary>\n    public sealed partial class TipView : MvxWindowsPage\n    {\n        public TipView()\n        {\n            this.InitializeComponent();\n        }\n    }\n}",
```
### Edit the XAML layout

Double click on the XAML file

This will open the XAML editor within Visual Studio.

I won't go into much depth at all here about how to use the XAML or do the Windows data-binding. I'm assuming most readers are already coming from at least a little XAML background.

To add the XAML user interface for our tip calculator, we will add a StackPanel within the existing Grid.

* a `StackPanel` container, into which we add:
  * some `TextBlock` static text
  * a bound `TextBox` for the `SubTotal`
  * a bound `Slider` for the `Generosity`
  * a bound `TextBlock` for the `Tip`

The full page will look like:
```c# 
<views:MvxWindowsPage\n    x:Class=\"TipCalc.UI.UWP.Views.TipView\"\n    xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\n    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n    xmlns:local=\"using:TipCalc.UI.UWP.Views\"\n    xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n    xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n    xmlns:views=\"using:MvvmCross.WindowsUWP.Views\"\n    mc:Ignorable=\"d\">\n\n    <Grid Background=\"{ThemeResource ApplicationPageBackgroundThemeBrush}\">\n        <StackPanel Margin=\"12,0,12,0\">\n            <TextBlock Text=\"SubTotal\" />\n            <TextBox Text=\"{Binding SubTotal, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}\" />\n            <TextBlock Text=\"Generosity\" />\n            <Slider Value=\"{Binding Generosity,Mode=TwoWay}\" \n                SmallChange=\"1\" \n                LargeChange=\"10\" \n                Minimum=\"0\" \n                Maximum=\"100\" />\n            <TextBlock Text=\"Tip\" />\n            <TextBlock Text=\"{Binding Tip}\" />\n        </StackPanel>\n    </Grid>\n</views:MvxWindowsPage>",
      "language": "xml"
    }
  ]
}
```
**Note** that in XAML, `OneWay` binding is generally the default. To provide TwoWay binding we explicitly add `Mode` to our binding expressions: e.g. `Value="{Binding Generosity,Mode=TwoWay}"`

**Note** the binding for the TextBox uses `UpdateSourceTrigger=PropertyChanged` so that the `SubTotal` property of `TipViewModel` is updated immediately rather than when the TextBox loses focus.

In the designer, this will look like:
[block:image]
{
  "images": [
    {
      "image": [
        "https://files.readme.io/zedtG5waSFCP0PF5uot7_Capture.PNG",
        "Capture.PNG",
        "1155",
        "989",
        "#89a7d3",
        ""
      ]
    }
  ]
}
```
## The Store UI is complete!

At this point you should be able to run your application either on the Local Machine or in a Mobile emulator.

When it starts... you should see this for the local machine:
[block:image]
{
  "images": [
    {
      "image": [
        "https://files.readme.io/bwvmKGSTRci3XX9H05Dr_Capture.PNG",
        "Capture.PNG",
        "1338",
        "883",
        "#cd913f",
        ""
      ]
    }
  ]
}
```
and in the mobile emulator:
[block:image]
{
  "images": [
    {
      "image": [
        "https://files.readme.io/9nlBnmbWR4WSn3PrUNU5_Capture.PNG",
        "Capture.PNG",
        "566",
        "1059",
        "#c88854",
        ""
      ]
    }
  ]
}
```
## Moving on...

There's more we could do to make this User Interface nicer and to make the app richer... but for this first application, we will leave it here for now.

Let's move on to the next piece of Windows!