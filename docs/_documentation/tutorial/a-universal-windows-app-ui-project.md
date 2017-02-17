---
layout: documentation
title: A Universal Windows App UI Project
category: Tutorials
---
We started with the goal of creating an app to help calculate what tip to leave in a restaurant

We had a plan to produce a UI based on this concept:

![Sketch](https://raw.github.com/slodge/MvvmCross/v3/v3Tutorial/Pictures/TipCalc_Sketch.png)

To satisfy this we built a 'Core' Portable Class Library project which contained:

* our 'business logic' - `ICalculation`
* our ViewModel - `TipViewModel`
* our `App` which contains the application wiring, including the start instructions.

We then added User Interfaces for Xamarin.Android, Xamarin.iOS and Windows UWP.

While UWP is the recommended approach for Windows and Windows Mobile development, you can also target Windows and Windows Phone using Windows 8.1 Universal Windows Apps.  Feel free to skip this section if you don't need to work with Windows 8.1 Universal Windows Apps.

A Universal Windows App is actually a set of three projects.  A Windows Phone 8.1 WinRT project, A Windows 8.1 project and a Shared project.  Unlike normal projects, the Shared project does not build to an assembly.  It's actually a set of files which are accessible within the Windows Phone and Windows projects as if they existed within those projects.  If you are familiar with file linking, it's pretty much the same thing with an improved UI.

You could follow all the steps below creating two separate Windows 8.1 and Windows Phone 8.1 RT projects but the code which gets placed in the Universal Shared project gets placed in both the Windows 8.1 and the Windows Phone 8.1 RT projects instead.

Obviously, to work with Windows and Windows Phone, we will need to switch back to working on the PC with Visual Studio.

## Create the new Universal Windows Apps Projects

Add a new project to your solution - a 'Blank App (Universal Apps)' application with name `TipCalc.UI.WindowsCommon`

Note that three projects have been added to your solution.  TipCalc.UI.WindowsCommon.Windows, TipCalc.UI.WindowsCommon.WindowsPhone and TipCalc.UI.WindowsCommon.Shared.

Within the WindowsCommon.Windows and WindowsCommon.WindowsPhone projects, you'll find the normal Universal Windows App UI application constructs:

* the 'Assets' folder, which contains the default images
* the MainPage.Xaml and MainPage.Xaml.cs files that define the default Page for this app
* the 'Package.appxmanifest' configuration file
* the debug private key for your development (WindowsCommon.Windows project only)

Within the Shared project, you'll find the normal Universal Windows App shared application constructs:

* the App.Xaml 'application' object

## Delete MainPage.xaml

No-one really needs a `MainPage` :)

This needs to be removed from both the WindowsCommon.Windows and WindowsCommon.WindowsPhone projects.

## Install MvvmCross

In the Package Manager Console, enter...

    Install-Package MvvmCross.Core
    
...for both the WindowsCommon.Windows and WindowsCommon.WindowsPhone projects.

## Add a reference to TipCalc.Core.csproj

Add a reference to your `TipCalc.Core` project - the project we created in the last step which included:

* your `Calculation` service, 
* your `TipViewModel` 
* your `App` wiring.

This needs to be added to both the WindowsCommon.Windows and WindowsCommon.WindowsPhone projects

## Add a Setup class

Just as we said during the construction of the other UI projects, *Every MvvmCross UI project requires a `Setup` class*

This class sits in the root namespace (folder) of our UI project and performs the initialisation of the MvvmCross framework and your application, including:

  * the Inversion of Control (IoC) system
  * the MvvmCross data-binding
  * your `App` and its collection of `ViewModel`s
  * your UI project and its collection of `View`s

Most of this functionality is provided for you automatically. Within your WindowsCommon.Shared UI project all you have to supply are:

- your `App` - your link to the business logic and `ViewModel` content

For `TipCalc` here's all that is needed in Setup.cs:
```C# using Windows.UI.Xaml.Controls;\nusing MvvmCross.Core.ViewModels;\nusing MvvmCross.WindowsCommon.Platform;\n\nnamespace TipCalc.UI.WindowsCommon\n{\n    public class Setup : MvxWindowsSetup\n    {\n        public Setup(Frame rootFrame) : base(rootFrame)\n        {\n        }\n\n        protected override IMvxApplication CreateApp()\n        {\n            return new Core.App();\n        }\n    }\n}",
```
## Modify the App.xaml.cs to use Setup

Your `App.xaml.cs` provides the Universal Windows App 'main application' object - an object which owns the User Interface and receives some callbacks from the operating system during some key events in your application's lifecycle.

To modify this `App.xaml.cs` for MvvmCross, we need to:

* modify the `OnLaunched` callback

 * remove these lines 
```C# // When the navigation stack isn't restored navigate to the first page,\n// configuring the new page by passing required information as a navigation\n// parameter\nif (!rootFrame.Navigate(typeof(MainPage), e.Arguments))\n{\n    throw new Exception(\"Failed to create initial page\");\n}",
```
 * add these lines to allow it to create `Setup`, and to then initiate the `IMvxAppStart` `Start` navigation
```C# var setup = new Setup(rootFrame);\nsetup.Initialize();\n\nvar start = Mvx.Resolve<IMvxAppStart>();\nstart.Start();",
```
To do this, you will need to add these `using` lines:
```C# using MvvmCross.Core.ViewModels;\nusing MvvmCross.Platform;",
```
## Add your View

### Create an initial Page for the WindowsCommon.Windows project

Create a Views folder in the WindowsCommon.Windows project

Within this folder, add a new 'Basic Page' and call it `TipView.xaml`

You will be asked if you want to add the missing 'Common' files automatically in order to support this 'Basic Page' - answer **Yes**

The page will generate:

* TipView.xaml
* TipView.xaml.cs

A Common folder will be added containing:

* NavigationHelper.cs
* ObservableDictionary.cs
* RelayCommand.cs
* SuspensionManager.cs

### Convert TipView into an MvvmCross base view

Change `TipView` so that it inherits from `MvxWindowsPage`

Change:
```C# public class TipView : Page",
```
to:
```C# public class TipView : MvxWindowsPage",
```
This requires the addition of:
```C# using MvvmCross.WindowsCommon.Views;",
```
### Persuade TipView to cooperate more reasonably with the `MvxWindowsPage` base class

Change the `OnNavigatedTo` and `OnNavigatedFrom` methods so that they call their base class implementations:
```C# base.OnNavigatedTo(e);",
```
and
```C# base.OnNavigatedFrom(e);",
```
Altogether this looks like:
```C# using TipCalc.UI.WindowsCommon.Common;\nusing Windows.UI.Xaml.Navigation;\nusing MvvmCross.WindowsCommon.Views;\n\n// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237\n\nnamespace TipCalc.UI.WindowsCommon.Views\n{\n    /// <summary>\n    /// A basic page that provides characteristics common to most applications.\n    /// </summary>\n    public sealed partial class TipView : MvxWindowsPage\n    {\n\n        private NavigationHelper navigationHelper;\n        private ObservableDictionary defaultViewModel = new ObservableDictionary();\n\n        /// <summary>\n        /// This can be changed to a strongly typed view model.\n        /// </summary>\n        public ObservableDictionary DefaultViewModel\n        {\n            get { return this.defaultViewModel; }\n        }\n\n        /// <summary>\n        /// NavigationHelper is used on each page to aid in navigation and \n        /// process lifetime management\n        /// </summary>\n        public NavigationHelper NavigationHelper\n        {\n            get { return this.navigationHelper; }\n        }\n\n\n        public TipView()\n        {\n            this.InitializeComponent();\n            this.navigationHelper = new NavigationHelper(this);\n            this.navigationHelper.LoadState += navigationHelper_LoadState;\n            this.navigationHelper.SaveState += navigationHelper_SaveState;\n        }\n\n        /// <summary>\n        /// Populates the page with content passed during navigation. Any saved state is also\n        /// provided when recreating a page from a prior session.\n        /// </summary>\n        /// <param name=\"sender\">\n        /// The source of the event; typically <see cref=\"Common.NavigationHelper\"/>\n        /// </param>\n        /// <param name=\"e\">Event data that provides both the navigation parameter passed to\n        /// <see cref=\"Frame.Navigate(Type, Object)\"/> when this page was initially requested and\n        /// a dictionary of state preserved by this page during an earlier\n        /// session. The state will be null the first time a page is visited.</param>\n        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)\n        {\n        }\n\n        /// <summary>\n        /// Preserves state associated with this page in case the application is suspended or the\n        /// page is discarded from the navigation cache.  Values must conform to the serialization\n        /// requirements of <see cref=\"Common.SuspensionManager.SessionState\"/>.\n        /// </summary>\n        /// <param name=\"sender\">The source of the event; typically <see cref=\"Common.NavigationHelper\"/></param>\n        /// <param name=\"e\">Event data that provides an empty dictionary to be populated with\n        /// serializable state.</param>\n        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)\n        {\n        }\n\n        #region NavigationHelper registration\n\n        /// The methods provided in this section are simply used to allow\n        /// NavigationHelper to respond to the page's navigation methods.\n        /// \n        /// Page specific logic should be placed in event handlers for the  \n        /// <see cref=\"Common.NavigationHelper.LoadState\"/>\n        /// and <see cref=\"Common.NavigationHelper.SaveState\"/>.\n        /// The navigation parameter is available in the LoadState method \n        /// in addition to page state preserved during an earlier session.\n\n        protected override void OnNavigatedTo(NavigationEventArgs e)\n        {\n            base.OnNavigatedTo(e);\n            navigationHelper.OnNavigatedTo(e);\n        }\n\n        protected override void OnNavigatedFrom(NavigationEventArgs e)\n        {\n            base.OnNavigatedFrom(e);\n            navigationHelper.OnNavigatedFrom(e);\n        }\n\n        #endregion\n    }\n}",
```
### Edit the XAML layout

Double click on the XAML file

This will open the XAML editor within Visual Studio.

I won't go into much depth at all here about how to use the XAML or do the Windows data-binding. I'm assuming most readers are already coming from at least a little XAML background.

To make the XAML inheritance match the `MvxWindowsPage` inheritance, change the outer root node of the Xaml file from:
```C# <Page \n    ... >\n    <!-- content -->\n</Page>",
      "language": "xml"
    }
  ]
}
```
to:
```C# <views:MvxWindowsPage\n    xmlns:views=\"using:MvvmCross.WindowsCommon.Views\"\n    ... >\n    <!-- content -->\n</views:MvxWindowsPage>",
      "language": "xml"
    }
  ]
}
```
To add the XAML user interface for our tip calculator, we will add a `ContentPanel` Grid just above the final `</Grid>` in the existing XAML.  This will contain:

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
**Note** that in XAML, `OneWay` binding is generally the default. To provide TwoWay binding we explicitly add `Mode` to our binding expressions: e.g. `Value="{Binding Generosity, Mode=TwoWay}"`

**Note** the binding for the TextBox uses `UpdateSourceTrigger=PropertyChanged` so that the `SubTotal` property of `TipViewModel` is updated immediately rather than when the TextBox loses focus.

In the designer, this will look like:

![Designer](https://raw.githubusercontent.com/sequence/MvvmCross/universalapping/v3Tutorial/Pictures/TipCalc_WindowsCommonWindows_Designer.png)

### Create an initial Page for the WindowsCommon.WindowsPhone project

Create a Views folder in the WindowsCommon.WindowsPhone project

Within this folder, add a new 'Basic Page' and call it `TipView.xaml`

You will be asked if you want to add the missing 'Common' files automatically in order to support this 'Basic Page' - answer **Yes**

The page will generate:

* TipView.xaml
* TipView.xaml.cs

A Common folder will be added containing:

* NavigationHelper.cs
* ObservableDictionary.cs
* RelayCommand.cs
* SuspensionManager.cs

### Convert TipView into an MvvmCross base view

Change `TipView` so that it inherits from `MvxWindowsPage`

Change:
```C# public class TipView : Page",
```
to:
```C# public class TipView : MvxWindowsPage",
```
This requires the addition of:
```C# using MvvmCross.WindowsCommon.Views;",
```
### Persuade TipCalc to cooperate more reasonably with the `MvxWindowsPage` base class

Change the `OnNavigatedTo` and `OnNavigatedFrom` methods so that they call their base class implementations:
```C# base.OnNavigatedTo(e);",
```
and 
```C# base.OnNavigatedFrom(e);",
```
Altogether this looks like:
```C# using TipCalc.UI.WindowsCommon.Common;\nusing Windows.UI.Xaml.Navigation;\nusing MvvmCross.WindowsCommon.Views;\n\n// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556\n\nnamespace TipCalc.UI.WindowsCommon.Views\n{\n    /// <summary>\n    /// An empty page that can be used on its own or navigated to within a Frame.\n    /// </summary>\n    public sealed partial class TipView : MvxWindowsPage\n    {\n        private NavigationHelper navigationHelper;\n        private ObservableDictionary defaultViewModel = new ObservableDictionary();\n\n        public TipView()\n        {\n            this.InitializeComponent();\n\n            this.navigationHelper = new NavigationHelper(this);\n            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;\n            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;\n        }\n\n        /// <summary>\n        /// Gets the <see cref=\"NavigationHelper\"/> associated with this <see cref=\"Page\"/>.\n        /// </summary>\n        public NavigationHelper NavigationHelper\n        {\n            get { return this.navigationHelper; }\n        }\n\n        /// <summary>\n        /// Gets the view model for this <see cref=\"Page\"/>.\n        /// This can be changed to a strongly typed view model.\n        /// </summary>\n        public ObservableDictionary DefaultViewModel\n        {\n            get { return this.defaultViewModel; }\n        }\n\n        /// <summary>\n        /// Populates the page with content passed during navigation.  Any saved state is also\n        /// provided when recreating a page from a prior session.\n        /// </summary>\n        /// <param name=\"sender\">\n        /// The source of the event; typically <see cref=\"NavigationHelper\"/>\n        /// </param>\n        /// <param name=\"e\">Event data that provides both the navigation parameter passed to\n        /// <see cref=\"Frame.Navigate(Type, Object)\"/> when this page was initially requested and\n        /// a dictionary of state preserved by this page during an earlier\n        /// session.  The state will be null the first time a page is visited.</param>\n        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)\n        {\n        }\n\n        /// <summary>\n        /// Preserves state associated with this page in case the application is suspended or the\n        /// page is discarded from the navigation cache.  Values must conform to the serialization\n        /// requirements of <see cref=\"SuspensionManager.SessionState\"/>.\n        /// </summary>\n        /// <param name=\"sender\">The source of the event; typically <see cref=\"NavigationHelper\"/></param>\n        /// <param name=\"e\">Event data that provides an empty dictionary to be populated with\n        /// serializable state.</param>\n        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)\n        {\n        }\n\n        #region NavigationHelper registration\n\n        /// <summary>\n        /// The methods provided in this section are simply used to allow\n        /// NavigationHelper to respond to the page's navigation methods.\n        /// <para>\n        /// Page specific logic should be placed in event handlers for the  \n        /// <see cref=\"NavigationHelper.LoadState\"/>\n        /// and <see cref=\"NavigationHelper.SaveState\"/>.\n        /// The navigation parameter is available in the LoadState method \n        /// in addition to page state preserved during an earlier session.\n        /// </para>\n        /// </summary>\n        /// <param name=\"e\">Provides data for navigation methods and event\n        /// handlers that cannot cancel the navigation request.</param>\n        protected override void OnNavigatedTo(NavigationEventArgs e)\n        {\n            base.OnNavigatedTo(e);\n            this.navigationHelper.OnNavigatedTo(e);\n        }\n\n        protected override void OnNavigatedFrom(NavigationEventArgs e)\n        {\n            base.OnNavigatedFrom(e);\n            this.navigationHelper.OnNavigatedFrom(e);\n        }\n\n        #endregion\n    }\n}",
```
### Edit the XAML layout

Double click on the XAML file

This will open the XAML editor within Visual Studio.

Again, I won't go into much depth at all here about how to use the XAML or do the Windows data-binding. I'm assuming most readers are already coming from at least a little XAML background.

To make the XAML inheritance match the `MvxWindowsPage` inheritance, change the outer root node of the Xaml file from:
```C# <Page \n    ... >\n    <!-- content -->\n</Page>",
```
to:
```C# <views:MvxWindowsPage\n    xmlns:views=\"using:MvvmCross.WindowsCommon.Views\"\n    ... >\n    <!-- content -->\n</views:MvxWindowsPage>",
```
To add the XAML user interface for our tip calculator, we will add a `ContentPanel` Grid in place of the `ContentRoot` Grid in the existing XAML.

This `Content Panel` will include exactly the same XAML as we added to the WindowsCommon.WindowsPhone project except for the margins:

* a `StackPanel` container, into which we add:
  * some `TextBlock` static text
  * a bound `TextBox` for the `SubTotal`
  * a bound `Slider` for the `Generosity`
  * a bound `TextBlock` for the `Tip`

This will produce XAML like:
```C# <Grid Grid.Row=\"1\" x:Name=\"ContentRoot\" Margin=\"19,9.5,19,0\">\n    <StackPanel>\n        <TextBlock\n            Text=\"SubTotal\" />\n        <TextBox \n            Text=\"{Binding SubTotal, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}\" />\n        <TextBlock\n            Text=\"Generosity\" />\n        <Slider \n            Value=\"{Binding Generosity,Mode=TwoWay}\" \n            SmallChange=\"1\" \n            LargeChange=\"10\" \n            Minimum=\"0\" \n            Maximum=\"100\" />\n        <TextBlock\n            Text=\"Tip\" />\n        <TextBlock \n            Text=\"{Binding Tip}\" />\n    </StackPanel>\n</Grid>",
```
**Note** that in XAML, `OneWay` binding is generally the default. To provide TwoWay binding we explicitly add `Mode` to our binding expressions: e.g. `Value="{Binding Generosity, Mode=TwoWay}"`


**Note** the binding for the TextBox uses `UpdateSourceTrigger=PropertyChanged` so that the `SubTotal` property of `TipViewModel` is updated immediately rather than when the TextBox loses focus.

In the designer, this will look like:

![Designer](https://raw.githubusercontent.com/sequence/MvvmCross/universalapping/v3Tutorial/Pictures/TipCalc_WindowsCommonWindowsPhone_Designer.png)

### The navigation cache

Universal Windows Phone apps seem to differ from Silverlight Windows Phone apps in that the default page navigation cache mechanism has changed. While Silverlight Windows Phone apps have built-in support for caching pages in forward/backward navigation, universal Windows Phone apps do not and need the NavigationHelper class and setting the NavigationCacheMode property of a Page to "Required" to provide this. This tutorial already showed you how to make use of the NavigationHelper and it is recommended that you do this even if you plan not to cache a view.

If you do enable caching by setting the NavigationCacheMode property of a Page to "Required" and navigate backwards or forwards, the view is retrieved from the cache. This includes the ViewModel property of the view. While this doesn't create a problem when navigating backwards, it does when you navigate forward! If you already have cached a view with a particular state (loading from the init parameters into the viewmodel), that state is also retrieved from the cache and you'll end up with a view with an 'old' viewmodel state.

To counter this, you must set the viewmodel to null when navigating to a page:
```C# protected override void OnNavigatedTo(NavigationEventArgs e)\n{\n    if (e.NavigationMode == NavigationMode.New)\n        ViewModel = null;\n\n    base.OnNavigatedTo(e);\n    this.navigationHelper.OnNavigatedTo(e);\n}",
```
## The Universal Windows App is complete!

At this point you should be able to run your application.

When you run the WindowsCommon.Windows project... you should see:

![v1](https://raw.githubusercontent.com/sequence/MvvmCross/universalapping/v3Tutorial/Pictures/TipCalc_WindowsCommonWindows_Emu.png)

When you run the WindowsCommon.WindowsPhone project... you should see:

![v1](https://raw.githubusercontent.com/sequence/MvvmCross/universalapping/v3Tutorial/Pictures/TipCalc_WindowsCommonWindowsPhone_Emu.png)
      
## Moving on...

There's more we could do to make this User Interface nicer and to make the app richer... but for this first application, we will leave it here for now.

But there are other ways of building Windows apps...