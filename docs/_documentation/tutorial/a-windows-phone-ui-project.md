---
layout: documentation
title: A Windows Phone UI Project
category: Tutorials
---
We started with the goal of creating an app to help calculate what tip to leave in a restaurant.

We had a plan to produce a UI based on this concept:

![Sketch](https://raw.github.com/slodge/MvvmCross/v3/v3Tutorial/Pictures/TipCalc_Sketch.png)

To satisfy this we built a 'Core' Portable Class Library project which contained:

* our 'business logic' - `ICalculation`
* our ViewModel - `TipViewModel`
* our `App` which contains the application wiring, including the start instructions.

We then added User Interfaces for Xamarin.Android, Xamarin.iOS, Windows UWP, Windows 8.1 Univeral and Windows 8.1 apps.

While UWP is the recommended approach for Windows Mobile (and Windows) development, you can also target Windows Phone using Windows Phone apps or Windows Phone Silverlight apps.  Here we'll look at Windows Phone Silverlight apps only as Windows Phone apps are very similar to Windows 8.1 apps.  Feel free to skip this section if you don't need to work with Windows Phone Silverlight  apps.

To create an Windows Phone Silverlight UI, you can use the Visual Studio project template wizards, but here we'll instead build up a new project 'from empty', just as we did for the Core, Android and iOS projects.

## Create a new Windows Phone Silverlight Project

Add a new project to your solution - a 'Blank App (Windows Phone Silverlight)' application with name `TipCalc.UI.WP`

For target operating system, choose 8.1.

Within this, you'll find the normal WP application constructs:

* the 'Properties' folder with its AppManifest.xml and WMAppManifest.xml 'configuration' files
* the 'Assets' folder which contains some icons
* the 'Resources' folder
* the App.xaml 'application' object
* the LocalizedStrings.cs class
* the MainPage.xaml and MainPage.xaml.cs files that define the default Page for this app

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

Just as we said during the construction of the other UI projects, *Every MvvmCross UI project requires a `Setup` class*

This class sits in the root namespace (folder) of our UI project and performs the initialisation of the MvvmCross framework and your application, including:

  * the Inversion of Control (IoC) system
  * the MvvmCross data-binding
  * your `App` and its collection of `ViewModel`s
  * your UI project and its collection of `View`s

Most of this functionality is provided for you automatically. Within your Windows Phone Silverlight UI project all you have to supply is:

- your `App` - your link to the business logic and `ViewModel` content

For `TipCalc` here's all that is needed in Setup.cs:
[block:code]
{
  "codes": [
    {
      "code": "using Microsoft.Phone.Controls;\nusing MvvmCross.Core.ViewModels;\nusing MvvmCross.WindowsPhone.Platform;\n\nnamespace TipCalc.UI.WP\n{\n    public class Setup : MvxPhoneSetup\n    {\n        public Setup(PhoneApplicationFrame rootFrame)\n            : base(rootFrame)\n        {\n        }\n\n        protected override IMvxApplication CreateApp()\n        {\n            return new Core.App();\n        }\n    }\n}",
      "language": "csharp"
    }
  ]
}
[/block]
## Modify the App.xaml.cs to use Setup

Your `App.xaml.cs` provides the Windows Phone Silverlight 'main application' object - an object which owns the User Interface and receives some callbacks from the operating system during some key events in your application's lifecycle.

To modify this `App.xaml.cs` for MvvmCross, we need to:

* modify the constructor so that it creates and starts 'Setup'
[block:code]
{
  "codes": [
    {
      "code": "var setup = new Setup(RootFrame);\nsetup.Initialize();",
      "language": "csharp"
    }
  ]
}
[/block]
* add a private field - just a boolean flag which we will set after we have done one navigation
[block:code]
{
  "codes": [
    {
      "code": "private bool _hasDoneFirstNavigation = false;",
      "language": "csharp"
    }
  ]
}
[/block]
* modify the Application_Launching callback so that we can intercept the first navigation, can cancel it and can delegate the initial navigation to `IMvxAppStart` instead.
[block:code]
{
  "codes": [
    {
      "code": "private void Application_Launching(object sender, LaunchingEventArgs e)\n{\n    RootFrame.Navigating += (navigatingSender, navigatingArgs) =>\n    {\n        if (_hasDoneFirstNavigation)\n            return;\n      \n        navigatingArgs.Cancel = true;\n        _hasDoneFirstNavigation = true;\n        var appStart = Mvx.Resolve<IMvxAppStart>();\n        RootFrame.Dispatcher.BeginInvoke(() => appStart.Start());\n    };\n}",
      "language": "csharp"
    }
  ]
}
[/block]
After you've done this your code might look like:
[block:code]
{
  "codes": [
    {
      "code": "using System;\nusing System.Diagnostics;\nusing System.Windows;\nusing System.Windows.Markup;\nusing System.Windows.Navigation;\nusing Microsoft.Phone.Controls;\nusing Microsoft.Phone.Shell;\nusing MvvmCross.Core.ViewModels;\nusing MvvmCross.Platform;\nusing TipCalc.UI.WP.Resources;\n\nnamespace TipCalc.UI.WP\n{\n    public partial class App : Application\n    {\n        bool _hasDoneFirstNavigation;\n\n        /// <summary>\n        /// Provides easy access to the root frame of the Phone Application.\n        /// </summary>\n        /// <returns>The root frame of the Phone Application.</returns>\n        public static PhoneApplicationFrame RootFrame { get; private set; }\n\n        /// <summary>\n        /// Constructor for the Application object.\n        /// </summary>\n        public App()\n        {\n            // Global handler for uncaught exceptions.\n            UnhandledException += Application_UnhandledException;\n\n            // Standard XAML initialization\n            InitializeComponent();\n\n            // Phone-specific initialization\n            InitializePhoneApplication();\n\n            // Language display initialization\n            InitializeLanguage();\n\n            // Show graphics profiling information while debugging.\n            if (Debugger.IsAttached)\n            {\n                // Display the current frame rate counters.\n                Application.Current.Host.Settings.EnableFrameRateCounter = true;\n\n                // Show the areas of the app that are being redrawn in each frame.\n                //Application.Current.Host.Settings.EnableRedrawRegions = true;\n\n                // Enable non-production analysis visualization mode,\n                // which shows areas of a page that are handed off to GPU with a colored overlay.\n                //Application.Current.Host.Settings.EnableCacheVisualization = true;\n\n                // Prevent the screen from turning off while under the debugger by disabling\n                // the application's idle detection.\n                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run\n                // and consume battery power when the user is not using the phone.\n                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;\n            }\n\n            var setup = new Setup(RootFrame);\n            setup.Initialize();\n        }\n\n        // Code to execute when a contract activation such as a file open or save picker returns \n        // with the picked file or other return values\n        private void Application_ContractActivated(object sender, Windows.ApplicationModel.Activation.IActivatedEventArgs e)\n        {\n        }\n\n        // Code to execute when the application is launching (eg, from Start)\n        // This code will not execute when the application is reactivated\n        private void Application_Launching(object sender, LaunchingEventArgs e)\n        {\n            RootFrame.Navigating += (navigatingSender, navigatingArgs) =>\n            {\n                if (_hasDoneFirstNavigation)\n                    return;\n\n                navigatingArgs.Cancel = true;\n                _hasDoneFirstNavigation = true;\n                var appStart = Mvx.Resolve<IMvxAppStart>();\n                RootFrame.Dispatcher.BeginInvoke(() => appStart.Start());\n            };\n        }\n\n        // Code to execute when the application is activated (brought to foreground)\n        // This code will not execute when the application is first launched\n        private void Application_Activated(object sender, ActivatedEventArgs e)\n        {\n        }\n\n        // Code to execute when the application is deactivated (sent to background)\n        // This code will not execute when the application is closing\n        private void Application_Deactivated(object sender, DeactivatedEventArgs e)\n        {\n        }\n\n        // Code to execute when the application is closing (eg, user hit Back)\n        // This code will not execute when the application is deactivated\n        private void Application_Closing(object sender, ClosingEventArgs e)\n        {\n        }\n\n        // Code to execute if a navigation fails\n        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)\n        {\n            if (Debugger.IsAttached)\n            {\n                // A navigation has failed; break into the debugger\n                Debugger.Break();\n            }\n        }\n\n        // Code to execute on Unhandled Exceptions\n        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)\n        {\n            if (Debugger.IsAttached)\n            {\n                // An unhandled exception has occurred; break into the debugger\n                Debugger.Break();\n            }\n        }\n\n        #region Phone application initialization\n\n        // Avoid double-initialization\n        private bool phoneApplicationInitialized = false;\n\n        // Do not add any additional code to this method\n        private void InitializePhoneApplication()\n        {\n            if (phoneApplicationInitialized)\n                return;\n\n            // Create the frame but don't set it as RootVisual yet; this allows the splash\n            // screen to remain active until the application is ready to render.\n            RootFrame = new PhoneApplicationFrame();\n            RootFrame.Navigated += CompleteInitializePhoneApplication;\n\n            // Handle navigation failures\n            RootFrame.NavigationFailed += RootFrame_NavigationFailed;\n\n            // Handle reset requests for clearing the backstack\n            RootFrame.Navigated += CheckForResetNavigation;\n\n            // Handle contract activation such as a file open or save picker\n            PhoneApplicationService.Current.ContractActivated += Application_ContractActivated;\n\n            // Ensure we don't initialize again\n            phoneApplicationInitialized = true;\n        }\n\n        // Do not add any additional code to this method\n        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)\n        {\n            // Set the root visual to allow the application to render\n            if (RootVisual != RootFrame)\n                RootVisual = RootFrame;\n\n            // Remove this handler since it is no longer needed\n            RootFrame.Navigated -= CompleteInitializePhoneApplication;\n        }\n\n        private void CheckForResetNavigation(object sender, NavigationEventArgs e)\n        {\n            // If the app has received a 'reset' navigation, then we need to check\n            // on the next navigation to see if the page stack should be reset\n            if (e.NavigationMode == NavigationMode.Reset)\n                RootFrame.Navigated += ClearBackStackAfterReset;\n        }\n\n        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)\n        {\n            // Unregister the event so it doesn't get called again\n            RootFrame.Navigated -= ClearBackStackAfterReset;\n\n            // Only clear the stack for 'new' (forward) and 'refresh' navigations\n            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)\n                return;\n\n            // For UI consistency, clear the entire page stack\n            while (RootFrame.RemoveBackEntry() != null)\n            {\n                ; // do nothing\n            }\n        }\n\n        #endregion\n\n        // Initialize the app's font and flow direction as defined in its localized resource strings.\n        //\n        // To ensure that the font of your application is aligned with its supported languages and that the\n        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage\n        // and ResourceFlowDirection should be initialized in each resx file to match these values with that\n        // file's culture. For example:\n        //\n        // AppResources.es-ES.resx\n        //    ResourceLanguage's value should be \"es-ES\"\n        //    ResourceFlowDirection's value should be \"LeftToRight\"\n        //\n        // AppResources.ar-SA.resx\n        //     ResourceLanguage's value should be \"ar-SA\"\n        //     ResourceFlowDirection's value should be \"RightToLeft\"\n        //\n        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.\n        //\n        private void InitializeLanguage()\n        {\n            try\n            {\n                // Set the font to match the display language defined by the\n                // ResourceLanguage resource string for each supported language.\n                //\n                // Fall back to the font of the neutral language if the Display\n                // language of the phone is not supported.\n                //\n                // If a compiler error is hit then ResourceLanguage is missing from\n                // the resource file.\n                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);\n\n                // Set the FlowDirection of all elements under the root frame based\n                // on the ResourceFlowDirection resource string for each\n                // supported language.\n                //\n                // If a compiler error is hit then ResourceFlowDirection is missing from\n                // the resource file.\n                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);\n                RootFrame.FlowDirection = flow;\n            }\n            catch\n            {\n                // If an exception is caught here it is most likely due to either\n                // ResourceLangauge not being correctly set to a supported language\n                // code or ResourceFlowDirection is set to a value other than LeftToRight\n                // or RightToLeft.\n\n                if (Debugger.IsAttached)\n                {\n                    Debugger.Break();\n                }\n\n                throw;\n            }\n        }\n    }\n}",
      "language": "csharp"
    }
  ]
}
[/block]
## Add your View

### Create an initial Page

Create a Views folder.

It is **important** in Windows Phone Silverlight apps, that this folder is called `Views` - the MvvmCross framework looks for this name by default for Windows Phone Silverlight.

Within this folder, add a new 'Windows Phone Portrait Page' and call it `TipView.xaml`

This will generate:

* TipView.xaml
* TipView.xaml.cs

### Turn TipView into the MvvmCross View for TipViewModel

Open the TipView.xaml.cs file.

To change TipView from a `PhonePage` into an MvvmCross view, change it so that it inherits from `MvxPhonePage`:
[block:code]
{
  "codes": [
    {
      "code": "public partial class TipView : MvxPhonePage",
      "language": "csharp"
    }
  ]
}
[/block]
 Altogether this looks like:
[block:code]
{
  "codes": [
    {
      "code": "using MvvmCross.WindowsPhone.Views;\n\nnamespace TipCalc.UI.WP.Views\n{\n    public partial class TipView : MvxPhonePage\n    {\n        public TipView()\n        {\n            InitializeComponent();\n        }\n    }\n}",
      "language": "csharp"
    }
  ]
}
[/block]
### Edit the XAML layout

Double click on the XAML file.

This will open the XAML editor within Visual Studio.

I won't go into much depth at all here about how to use the XAML or do the Windows data-binding. I'm assuming most readers are already coming from at least a little XAML background.

To make the XAML inheritance match the `MvxPhonePage` inheritance, change the outer root node of the Xaml file from:
[block:code]
{
  "codes": [
    {
      "code": "<phone:PhoneApplicationPage \n    ... >\n    <!-- content -->\n</phone:PhoneApplicationPage>",
      "language": "csharp"
    }
  ]
}
[/block]
to:
[block:code]
{
  "codes": [
    {
      "code": "<views:MvxPhonePage\n    xmlns:views=\"clr-namespace:MvvmCross.WindowsPhone.Views;assembly=MvvmCross.WindowsPhone\"\n    ... >\n    <!-- content -->\n</views:MvxPhonePage>\n",
      "language": "xml"
    }
  ]
}
[/block]
To then add the XAML user interface for our tip calculator, we will edit the `ContentPanel` to include:

* a `StackPanel` container, into which we add:
  * some `TextBlock` static text
  * a bound `TextBox` for the `SubTotal`
  * a bound `Slider` for the `Generosity`
  * a bound `TextBlock` for the `Tip`

This will produce XAML like:
[block:code]
{
  "codes": [
    {
      "code": "<Grid x:Name=\"ContentPanel\" Grid.Row=\"1\" Margin=\"12,0,12,0\">\n    <StackPanel>\n        <TextBlock\n            Text=\"SubTotal\"\n            Style=\"{StaticResource PhoneTextSubtleStyle}\" />\n        <TextBox \n            Text=\"{Binding SubTotal, Mode=TwoWay}\" />\n        <TextBlock\n            Text=\"Generosity\"\n            Style=\"{StaticResource PhoneTextSubtleStyle}\" />\n        <Slider \n            Value=\"{Binding Generosity, Mode=TwoWay}\" \n            SmallChange=\"1\" \n            LargeChange=\"10\" \n            Minimum=\"0\" \n            Maximum=\"100\" />\n        <TextBlock\n            Text=\"Tip\"\n            Style=\"{StaticResource PhoneTextSubtleStyle}\" />\n        <TextBlock \n            Text=\"{Binding Tip}\" />\n    </StackPanel>\n</Grid>",
      "language": "text"
    }
  ]
}
[/block]
**Note** that in XAML, `OneWay` binding is generally the default. To provide TwoWay binding we explicitly add `Mode` to our binding expressions: e.g. `Value="{Binding Generosity, Mode=TwoWay}"`

In the designer, this will look like:

![Designer](https://raw.github.com/slodge/MvvmCross/v3/v3Tutorial/Pictures/TipCalc_WP_Designer.png)

## The WP UI is complete!

At this point you should be able to run your application.

When it starts... you should see:

![v1](https://raw.github.com/slodge/MvvmCross/v3/v3Tutorial/Pictures/TipCalc_WP_Emu.png)

This seems to work perfectly, although you may notice that if you edit the value in the `SubTotal` TextBox then you rest of the display does not correctly update.

This is a View concern - it is a UI problem. So we can fix it just in the Windows Phone Silverlight UI code - in this View. For example, to fix this here, you can add the 'Coding4Fun.Toolkit.Controls' NuGet package and then use their `UpdateSourceOnChange` attached property to resolve the issue

     coding4fun:TextBinding.UpdateSourceOnChange="True"
        
## Moving on...

There's more we could do to make this User Interface nicer and to make the app richer... but for this first application, we will leave it here for now.

Let's move on to even more Windows!