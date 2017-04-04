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
```c# 
using Microsoft.Phone.Controls;
using MvvmCross.Core.ViewModels;
using MvvmCross.WindowsPhone.Platform;

namespace TipCalc.UI.WP
{
    public class Setup : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame)
            : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
    }
}
```
## Modify the App.xaml.cs to use Setup

Your `App.xaml.cs` provides the Windows Phone Silverlight 'main application' object - an object which owns the User Interface and receives some callbacks from the operating system during some key events in your application's lifecycle.

To modify this `App.xaml.cs` for MvvmCross, we need to:

* modify the constructor so that it creates and starts 'Setup'
```c# 
var setup = new Setup(RootFrame);
setup.Initialize();
```
* add a private field - just a boolean flag which we will set after we have done one navigation
```c# 
private bool _hasDoneFirstNavigation = false;
```
* modify the Application_Launching callback so that we can intercept the first navigation, can cancel it and can delegate the initial navigation to `IMvxAppStart` instead.
```c# 
private void Application_Launching(object sender, LaunchingEventArgs e)
{
    RootFrame.Navigating += (navigatingSender, navigatingArgs) =>
    {
        if (_hasDoneFirstNavigation)
            return;
      
        navigatingArgs.Cancel = true;
        _hasDoneFirstNavigation = true;
        var appStart = Mvx.Resolve<IMvxAppStart>();
        RootFrame.Dispatcher.BeginInvoke(() => appStart.Start());
    };
}
```
After you've done this your code might look like:
```c# 
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using TipCalc.UI.WP.Resources;

namespace TipCalc.UI.WP
{
    public partial class App : Application
    {
        bool _hasDoneFirstNavigation;

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            var setup = new Setup(RootFrame);
            setup.Initialize();
        }

        // Code to execute when a contract activation such as a file open or save picker returns 
        // with the picked file or other return values
        private void Application_ContractActivated(object sender, Windows.ApplicationModel.Activation.IActivatedEventArgs e)
        {
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            RootFrame.Navigating += (navigatingSender, navigatingArgs) =>
            {
                if (_hasDoneFirstNavigation)
                    return;

                navigatingArgs.Cancel = true;
                _hasDoneFirstNavigation = true;
                var appStart = Mvx.Resolve<IMvxAppStart>();
                RootFrame.Dispatcher.BeginInvoke(() => appStart.Start());
            };
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Handle contract activation such as a file open or save picker
            PhoneApplicationService.Current.ContractActivated += Application_ContractActivated;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }
    }
}
```
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
```c# 
public partial class TipView : MvxPhonePage
```
 Altogether this looks like:
```c# 
using MvvmCross.WindowsPhone.Views;

namespace TipCalc.UI.WP.Views
{
    public partial class TipView : MvxPhonePage
    {
        public TipView()
        {
            InitializeComponent();
        }
    }
}
```
### Edit the XAML layout

Double click on the XAML file.

This will open the XAML editor within Visual Studio.

I won't go into much depth at all here about how to use the XAML or do the Windows data-binding. I'm assuming most readers are already coming from at least a little XAML background.

To make the XAML inheritance match the `MvxPhonePage` inheritance, change the outer root node of the Xaml file from:
```c# 
<phone:PhoneApplicationPage 
    ... >
    <!-- content -->
</phone:PhoneApplicationPage>
```
to:
```xml
<views:MvxPhonePage
    xmlns:views="clr-namespace:MvvmCross.WindowsPhone.Views;assembly=MvvmCross.WindowsPhone"
    ... >
    <!-- content -->
</views:MvxPhonePage>
```
To then add the XAML user interface for our tip calculator, we will edit the `ContentPanel` to include:

* a `StackPanel` container, into which we add:
  * some `TextBlock` static text
  * a bound `TextBox` for the `SubTotal`
  * a bound `Slider` for the `Generosity`
  * a bound `TextBlock` for the `Tip`

This will produce XAML like:

```xml
<Grid x:Name="ContentPanel" Grid.Row="1" Margin="12,0,12,0">
    <StackPanel>
        <TextBlock
            Text="SubTotal"
            Style="{StaticResource PhoneTextSubtleStyle}" />
        <TextBox 
            Text="{Binding SubTotal, Mode=TwoWay}" />
        <TextBlock
            Text="Generosity"
            Style="{StaticResource PhoneTextSubtleStyle}" />
        <Slider 
            Value="{Binding Generosity, Mode=TwoWay}" 
            SmallChange="1" 
            LargeChange="10" 
            Minimum="0" 
            Maximum="100" />
        <TextBlock
            Text="Tip"
            Style="{StaticResource PhoneTextSubtleStyle}" />
        <TextBlock 
            Text="{Binding Tip}" />
    </StackPanel>
</Grid>
```
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
