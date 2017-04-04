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
```c#
using Windows.UI.Xaml.Controls;
using MvvmCross.Core.ViewModels;
using MvvmCross.WindowsCommon.Platform;

namespace TipCalc.UI.WindowsStore
{
public class Setup : MvxWindowsSetup
{
    public Setup(Frame rootFrame) : base(rootFrame)
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

Your `App.xaml.cs` provides the WindowsStore 'main application' object - an object which owns the User Interface and receives some callbacks from the operating system during some key events in your application's lifecycle.

To modify this `App.xaml.cs` for MvvmCross, we need to:

* modify the `OnLaunched` callback

 * replace this lines
```c#
// When the navigation stack isn't restored navigate to the first page,
// configuring the new page by passing required information as a navigation
// parameter
rootFrame.Navigate(typeof(MainPage), e.Arguments);
```
  * with these lines to allow it to create `Setup`, and to then initiate the `IMvxAppStart` `Start` navigation
```c#
var setup = new Setup(rootFrame);
setup.Initialize();

var start = Mvx.Resolve<IMvxAppStart>();
start.Start();
```
After you've done this your code might look like:
```c#
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace TipCalc.UI.WindowsStore
{
/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
sealed partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
        this.Suspending += OnSuspending;
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="e">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {

#if DEBUG
        if (System.Diagnostics.Debugger.IsAttached)
        {
            this.DebugSettings.EnableFrameRateCounter = true;
        }
#endif

        Frame rootFrame = Window.Current.Content as Frame;

        // Do not repeat app initialization when the Window already has content,
        // just ensure that the window is active
        if (rootFrame == null)
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new Frame();
            // Set the default language
            rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

            rootFrame.NavigationFailed += OnNavigationFailed;

            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
        }

        if (rootFrame.Content == null)
        {
            var setup = new Setup(rootFrame);
            setup.Initialize();

            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();
        }
        // Ensure the current window is active
        Window.Current.Activate();
    }

    /// <summary>
    /// Invoked when Navigation to a certain page fails
    /// </summary>
    /// <param name="sender">The Frame which failed navigation</param>
    /// <param name="e">Details about the navigation failure</param>
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    /// <summary>
    /// Invoked when application execution is being suspended.  Application state is saved
    /// without knowing whether the application will be terminated or resumed with the contents
    /// of memory still intact.
    /// </summary>
    /// <param name="sender">The source of the suspend request.</param>
    /// <param name="e">Details about the suspend request.</param>
    private void OnSuspending(object sender, SuspendingEventArgs e)
    {
        var deferral = e.SuspendingOperation.GetDeferral();
        //TODO: Save application state and stop any background activity
        deferral.Complete();
    }
}
}

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
```c#
public class TipView : Page
```
to:
```c#
public class TipView : MvxWindowsPage
```
This requires the addition of:
```c#
using MvvmCross.WindowsCommon.Views;
```
### Persuade TipView to cooperate more reasonably with the `MvxStorePage` base class

Either remove the `region`:
```c#
#region NavigationHelper registration

protected override void OnNavigatedTo(NavigationEventArgs e)
{
    ...
}

protected override void OnNavigatedFrom(NavigationEventArgs e)
{
    ...
}

#endregion
```
Or change the `OnNavigatedTo` and `OnNavigatedFrom` methods so that they call their base class implementations:
```c#
base.OnNavigatedTo(e);
```
and 
```c#
base.OnNavigatedFrom(e);
```
Altogether this looks like:
```c#
using MvvmCross.WindowsCommon.Views;
using TipCalc.UI.WindowsStore.Common;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace TipCalc.UI.WindowsStore.Views
{
/// <summary>
/// A basic page that provides characteristics common to most applications.
/// </summary>
public sealed partial class TipView : MvxWindowsPage
{

    private NavigationHelper navigationHelper;
    private ObservableDictionary defaultViewModel = new ObservableDictionary();

    /// <summary>
    /// This can be changed to a strongly typed view model.
    /// </summary>
    public ObservableDictionary DefaultViewModel
    {
        get {
            return this.defaultViewModel;
        }
    }

    /// <summary>
    /// NavigationHelper is used on each page to aid in navigation and
    /// process lifetime management
    /// </summary>
    public NavigationHelper NavigationHelper
    {
        get {
            return this.navigationHelper;
        }
    }


    public TipView()
    {
        this.InitializeComponent();
        this.navigationHelper = new NavigationHelper(this);
        this.navigationHelper.LoadState += navigationHelper_LoadState;
        this.navigationHelper.SaveState += navigationHelper_SaveState;
    }

    /// <summary>
    /// Populates the page with content passed during navigation. Any saved state is also
    /// provided when recreating a page from a prior session.
    /// </summary>
    /// <param name="sender">
    /// The source of the event; typically <see cref="Common.NavigationHelper"/>
    /// </param>
    /// <param name="e">Event data that provides both the navigation parameter passed to
    /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
    /// a dictionary of state preserved by this page during an earlier
    /// session. The state will be null the first time a page is visited.</param>
    private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
    {
    }

    /// <summary>
    /// Preserves state associated with this page in case the application is suspended or the
    /// page is discarded from the navigation cache.  Values must conform to the serialization
    /// requirements of <see cref="Common.SuspensionManager.SessionState"/>.
    /// </summary>
    /// <param name="sender">The source of the event; typically <see cref="Common.NavigationHelper"/></param>
    /// <param name="e">Event data that provides an empty dictionary to be populated with
    /// serializable state.</param>
    private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
    {
    }

    #region NavigationHelper registration

    /// The methods provided in this section are simply used to allow
    /// NavigationHelper to respond to the page's navigation methods.
    ///
    /// Page specific logic should be placed in event handlers for the
    /// <see cref="Common.NavigationHelper.LoadState"/>
    /// and <see cref="Common.NavigationHelper.SaveState"/>.
    /// The navigation parameter is available in the LoadState method
    /// in addition to page state preserved during an earlier session.

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        navigationHelper.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        navigationHelper.OnNavigatedFrom(e);
    }

    #endregion
}
}

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
```xml
<Grid x:Name="ContentPanel" Grid.Row="1" Margin="12,0,12,0">
    <StackPanel>
        <TextBlock
            Text="SubTotal" />
        <TextBox 
            Text="{Binding SubTotal, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />
        <TextBlock
            Text="Generosity" />
        <Slider 
            Value="{Binding Generosity,Mode=TwoWay}" 
            SmallChange="1" 
            LargeChange="10" 
            Minimum="0" 
            Maximum="100" />
        <TextBlock
            Text="Tip" />
        <TextBlock 
            Text="{Binding Tip}" />
    </StackPanel>
</Grid>
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
