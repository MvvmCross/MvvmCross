---
layout: documentation
title: A Windows Store Project
category: Tutorials
---
We started with the goal of creating an app to help calculate what tip to leave in a restaurant

We had a plan to produce a UI based on this concept:

![Sketch](https://raw.github.com/slodge/MvvmCross/v3/v3Tutorial/Pictures/TipCalc_Sketch.png)

To satisfy this we built a 'Core' Portable Class Library project which contained:

* our 'business logic' - `ICalculation`
* our ViewModel - `TipViewModel`
* our `App` which contains the application wiring, including the start instructions.

We then added User Interfaces for Xamarin.Android, Xamarin.iOS, Windows UWP and Windows 8.1 Univeral apps.

While UWP is the recommended approach for Windows (and Windows Mobile) development, you can also target Windows using Windows 8.1 apps.  Feel free to skip this section if you don't need to work with Windows 8.1 apps.

To create a Windows 8.1 MvvmCross UI, you can use the Visual Studio project template wizards, but here we'll instead build up a new project 'from empty', just as we did for the Core and other UI projects.

## Create a new Windows 8.1 Project

Add a new project to your solution - a 'Blank App (Windows 8.1)' application with name `TipCalc.UI.WindowsStore`

Within this, you'll find the normal WindowsStore application constructs:

* the 'Properties' folder with just the 'AssemblyInfo' file
* the 'Assets' folder
* the 'Common' folder
* the App.Xaml 'application' object
* the MainPage.Xaml and MainPage.Xaml.cs files that define the default Page for this app
* the 'Package.appxmanifest' configuration file
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

Most of this functionality is provided for you automatically. Within your WindowsStore UI project all you have to supply is:

- your `App` - your link to the business logic and `ViewModel` content.

For `TipCalc` here's all that is needed in Setup.cs:
```C# using Windows.UI.Xaml.Controls;\nusing MvvmCross.Core.ViewModels;\nusing MvvmCross.WindowsCommon.Platform;\n\nnamespace TipCalc.UI.WindowsStore\n{\n    public class Setup : MvxWindowsSetup\n    {\n        public Setup(Frame rootFrame) : base(rootFrame)\n        {\n        }\n\n        protected override IMvxApplication CreateApp()\n        {\n            return new Core.App();\n        }\n    }\n}",
```
## Modify the App.xaml.cs to use Setup

Your `App.xaml.cs` provides the WindowsStore 'main application' object - an object which owns the User Interface and receives some callbacks from the operating system during some key events in your application's lifecycle.

To modify this `App.xaml.cs` for MvvmCross, we need to:

* modify the `OnLaunched` callback

 * replace this lines
```C# // When the navigation stack isn't restored navigate to the first page,\n// configuring the new page by passing required information as a navigation\n// parameter\nrootFrame.Navigate(typeof(MainPage), e.Arguments);",
```
  * with these lines to allow it to create `Setup`, and to then initiate the `IMvxAppStart` `Start` navigation
```C# var setup = new Setup(rootFrame);\nsetup.Initialize();\n\nvar start = Mvx.Resolve<IMvxAppStart>();\nstart.Start();",
```
After you've done this your code might look like:
```C# using System;\nusing Windows.ApplicationModel;\nusing Windows.ApplicationModel.Activation;\nusing Windows.UI.Xaml;\nusing Windows.UI.Xaml.Controls;\nusing Windows.UI.Xaml.Navigation;\nusing MvvmCross.Core.ViewModels;\nusing MvvmCross.Platform;\n\n// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227\n\nnamespace TipCalc.UI.WindowsStore\n{\n    /// <summary>\n    /// Provides application-specific behavior to supplement the default Application class.\n    /// </summary>\n    sealed partial class App : Application\n    {\n        /// <summary>\n        /// Initializes the singleton application object.  This is the first line of authored code\n        /// executed, and as such is the logical equivalent of main() or WinMain().\n        /// </summary>\n        public App()\n        {\n            this.InitializeComponent();\n            this.Suspending += OnSuspending;\n        }\n\n        /// <summary>\n        /// Invoked when the application is launched normally by the end user.  Other entry points\n        /// will be used such as when the application is launched to open a specific file.\n        /// </summary>\n        /// <param name=\"e\">Details about the launch request and process.</param>\n        protected override void OnLaunched(LaunchActivatedEventArgs e)\n        {\n\n#if DEBUG\n            if (System.Diagnostics.Debugger.IsAttached)\n            {\n                this.DebugSettings.EnableFrameRateCounter = true;\n            }\n#endif\n\n            Frame rootFrame = Window.Current.Content as Frame;\n\n            // Do not repeat app initialization when the Window already has content,\n            // just ensure that the window is active\n            if (rootFrame == null)\n            {\n                // Create a Frame to act as the navigation context and navigate to the first page\n                rootFrame = new Frame();\n                // Set the default language\n                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];\n\n                rootFrame.NavigationFailed += OnNavigationFailed;\n\n                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)\n                {\n                    //TODO: Load state from previously suspended application\n                }\n\n                // Place the frame in the current Window\n                Window.Current.Content = rootFrame;\n            }\n\n            if (rootFrame.Content == null)\n            {\n                var setup = new Setup(rootFrame);\n                setup.Initialize();\n\n                var start = Mvx.Resolve<IMvxAppStart>();\n                start.Start();\n            }\n            // Ensure the current window is active\n            Window.Current.Activate();\n        }\n\n        /// <summary>\n        /// Invoked when Navigation to a certain page fails\n        /// </summary>\n        /// <param name=\"sender\">The Frame which failed navigation</param>\n        /// <param name=\"e\">Details about the navigation failure</param>\n        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)\n        {\n            throw new Exception(\"Failed to load Page \" + e.SourcePageType.FullName);\n        }\n\n        /// <summary>\n        /// Invoked when application execution is being suspended.  Application state is saved\n        /// without knowing whether the application will be terminated or resumed with the contents\n        /// of memory still intact.\n        /// </summary>\n        /// <param name=\"sender\">The source of the suspend request.</param>\n        /// <param name=\"e\">Details about the suspend request.</param>\n        private void OnSuspending(object sender, SuspendingEventArgs e)\n        {\n            var deferral = e.SuspendingOperation.GetDeferral();\n            //TODO: Save application state and stop any background activity\n            deferral.Complete();\n        }\n    }\n}\n",
```
## Add your View

### Create an initial Page

Create a Views folder

Within this folder, add a new 'Basic Page' and call it `TipView.xaml`

You will be asked if you want to add the missing 'Common' files automatically in order to support this 'Basic Page' - answer **Yes**

The page will generate:

* TipView.xaml
* TipView.xaml.cs

### Turn TipView into the MvvmCross View for TipViewModel

Change:
```C#  public class TipView : Page",
```
to:
```C# public class TipView : MvxWindowsPage",
```
This requires the addition of:
```C# using MvvmCross.WindowsCommon.Views;",
```
### Persuade TipView to cooperate more reasonably with the `MvxStorePage` base class

Either remove the `region`:
```C# #region NavigationHelper registration\n        \nprotected override void OnNavigatedTo(NavigationEventArgs e)\n{\n    ...\n}\n\nprotected override void OnNavigatedFrom(NavigationEventArgs e)\n{\n    ...\n}\n\n#endregion ",
      "language": "text"
    }
  ]
}
```
Or change the `OnNavigatedTo` and `OnNavigatedFrom` methods so that they call their base class implementations:
```C# base.OnNavigatedTo(e);",
```
and 
```C# base.OnNavigatedFrom(e);",
```
Altogether this looks like:
```C# using MvvmCross.WindowsCommon.Views;\nusing TipCalc.UI.WindowsStore.Common;\nusing Windows.UI.Xaml.Navigation;\n\n// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237\n\nnamespace TipCalc.UI.WindowsStore.Views\n{\n    /// <summary>\n    /// A basic page that provides characteristics common to most applications.\n    /// </summary>\n    public sealed partial class TipView : MvxWindowsPage\n    {\n\n        private NavigationHelper navigationHelper;\n        private ObservableDictionary defaultViewModel = new ObservableDictionary();\n\n        /// <summary>\n        /// This can be changed to a strongly typed view model.\n        /// </summary>\n        public ObservableDictionary DefaultViewModel\n        {\n            get { return this.defaultViewModel; }\n        }\n\n        /// <summary>\n        /// NavigationHelper is used on each page to aid in navigation and \n        /// process lifetime management\n        /// </summary>\n        public NavigationHelper NavigationHelper\n        {\n            get { return this.navigationHelper; }\n        }\n\n\n        public TipView()\n        {\n            this.InitializeComponent();\n            this.navigationHelper = new NavigationHelper(this);\n            this.navigationHelper.LoadState += navigationHelper_LoadState;\n            this.navigationHelper.SaveState += navigationHelper_SaveState;\n        }\n\n        /// <summary>\n        /// Populates the page with content passed during navigation. Any saved state is also\n        /// provided when recreating a page from a prior session.\n        /// </summary>\n        /// <param name=\"sender\">\n        /// The source of the event; typically <see cref=\"Common.NavigationHelper\"/>\n        /// </param>\n        /// <param name=\"e\">Event data that provides both the navigation parameter passed to\n        /// <see cref=\"Frame.Navigate(Type, Object)\"/> when this page was initially requested and\n        /// a dictionary of state preserved by this page during an earlier\n        /// session. The state will be null the first time a page is visited.</param>\n        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)\n        {\n        }\n\n        /// <summary>\n        /// Preserves state associated with this page in case the application is suspended or the\n        /// page is discarded from the navigation cache.  Values must conform to the serialization\n        /// requirements of <see cref=\"Common.SuspensionManager.SessionState\"/>.\n        /// </summary>\n        /// <param name=\"sender\">The source of the event; typically <see cref=\"Common.NavigationHelper\"/></param>\n        /// <param name=\"e\">Event data that provides an empty dictionary to be populated with\n        /// serializable state.</param>\n        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)\n        {\n        }\n\n        #region NavigationHelper registration\n\n        /// The methods provided in this section are simply used to allow\n        /// NavigationHelper to respond to the page's navigation methods.\n        /// \n        /// Page specific logic should be placed in event handlers for the  \n        /// <see cref=\"Common.NavigationHelper.LoadState\"/>\n        /// and <see cref=\"Common.NavigationHelper.SaveState\"/>.\n        /// The navigation parameter is available in the LoadState method \n        /// in addition to page state preserved during an earlier session.\n\n        protected override void OnNavigatedTo(NavigationEventArgs e)\n        {\n            base.OnNavigatedTo(e);\n            navigationHelper.OnNavigatedTo(e);\n        }\n\n        protected override void OnNavigatedFrom(NavigationEventArgs e)\n        {\n            base.OnNavigatedTo(e);\n            navigationHelper.OnNavigatedFrom(e);\n        }\n\n        #endregion\n    }\n}\n",
```
### Edit the XAML layout

Double click on the XAML file

This will open the XAML editor within Visual Studio.

Just as with the Universal Windows Apps and Windows Phone Silverlight, I won't go into much depth at all here about how to use the XAML or do the Windows data-binding. I'm assuming most readers are already coming from at least a little XAML background.

To add the XAML user interface for our tip calculator, we will add a StackPanel to the end of the main Grid.

This `StackPanel` will include **almost** exactly the same XAML as we added to the Windows Phone Silverlight example - only the `Style` attributes are removed:

* a `StackPanel` container, into which we add:
  * some `TextBlock` static text
  * a bound `TextBox` for the `SubTotal`
  * a bound `Slider` for the `Generosity`
  * a bound `TextBlock` for the `Tip`

This will produce XAML like:
```C# <Grid x:Name=\"ContentPanel\" Grid.Row=\"1\" Margin=\"12,0,12,0\">\n    <StackPanel>\n        <TextBlock\n            Text=\"SubTotal\" />\n        <TextBox \n            Text=\"{Binding SubTotal, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}\" />\n        <TextBlock\n            Text=\"Generosity\" />\n        <Slider \n            Value=\"{Binding Generosity,Mode=TwoWay}\" \n            SmallChange=\"1\" \n            LargeChange=\"10\" \n            Minimum=\"0\" \n            Maximum=\"100\" />\n        <TextBlock\n            Text=\"Tip\" />\n        <TextBlock \n            Text=\"{Binding Tip}\" />\n    </StackPanel>\n</Grid>",
      "language": "xml"
    }
  ]
}
```
**Note** that in XAML, `OneWay` binding is generally the default. To provide TwoWay binding we explicitly add `Mode` to our binding expressions: e.g. `Value="{Binding Generosity,Mode=TwoWay}"`

**Note** the binding for the TextBox uses `UpdateSourceTrigger=PropertyChanged` so that the `SubTotal` property of `TipViewModel` is updated immediately rather than when the TextBox loses focus.

In the designer, this will look like:

![Designer](https://raw.github.com/slodge/MvvmCross/v3/v3Tutorial/Pictures/TipCalc_Store_Designer.png)

## The Store UI is complete!

At this point you should be able to run your application.

When it starts... you should see:

![Designer](https://raw.github.com/slodge/MvvmCross/v3/v3Tutorial/Pictures/TipCalc_Store_Emu.png)

## Moving on...

There's more we could do to make this User Interface nicer and to make the app richer... but for this first application, we will leave it here for now.

But there are other ways of building Windows apps...