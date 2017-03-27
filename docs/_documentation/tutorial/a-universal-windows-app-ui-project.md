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
```c# 
using Windows.UI.Xaml.Controls;
using MvvmCross.Core.ViewModels;
using MvvmCross.WindowsCommon.Platform;

namespace TipCalc.UI.WindowsCommon
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

Your `App.xaml.cs` provides the Universal Windows App 'main application' object - an object which owns the User Interface and receives some callbacks from the operating system during some key events in your application's lifecycle.

To modify this `App.xaml.cs` for MvvmCross, we need to:

* modify the `OnLaunched` callback

 * remove these lines 
```c# 
// When the navigation stack isn't restored navigate to the first page,
// configuring the new page by passing required information as a navigation
// parameter
if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
{
    throw new Exception("Failed to create initial page");
}
```
 * add these lines to allow it to create `Setup`, and to then initiate the `IMvxAppStart` `Start` navigation
```c# 
var setup = new Setup(rootFrame);
setup.Initialize();

var start = Mvx.Resolve<IMvxAppStart>();
start.Start();
```
To do this, you will need to add these `using` lines:
```c# 
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
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
### Persuade TipView to cooperate more reasonably with the `MvxWindowsPage` base class

Change the `OnNavigatedTo` and `OnNavigatedFrom` methods so that they call their base class implementations:
```c# 
base.OnNavigatedTo(e);
```
and
```c# 
base.OnNavigatedFrom(e);
```
Altogether this looks like:
```c# 
using TipCalc.UI.WindowsCommon.Common;
using Windows.UI.Xaml.Navigation;
using MvvmCross.WindowsCommon.Views;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace TipCalc.UI.WindowsCommon.Views
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
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
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
            base.OnNavigatedFrom(e);
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
```
### Edit the XAML layout

Double click on the XAML file

This will open the XAML editor within Visual Studio.

I won't go into much depth at all here about how to use the XAML or do the Windows data-binding. I'm assuming most readers are already coming from at least a little XAML background.

To make the XAML inheritance match the `MvxWindowsPage` inheritance, change the outer root node of the Xaml file from:
```c# 
<Page 
    ... >
    <!-- content -->
</Page>
```
to:
```c# 
<views:MvxWindowsPage
    xmlns:views="using:MvvmCross.WindowsCommon.Views"
    ... >
    <!-- content -->
</views:MvxWindowsPage>
```
To add the XAML user interface for our tip calculator, we will add a `ContentPanel` Grid just above the final `</Grid>` in the existing XAML.  This will contain:

* a `StackPanel` container, into which we add:
  * some `TextBlock` static text
  * a bound `TextBox` for the `SubTotal`
  * a bound `Slider` for the `Generosity`
  * a bound `TextBlock` for the `Tip`

This will produce XAML like:
```c# 
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
### Persuade TipCalc to cooperate more reasonably with the `MvxWindowsPage` base class

Change the `OnNavigatedTo` and `OnNavigatedFrom` methods so that they call their base class implementations:
```c# 
base.OnNavigatedTo(e);
```
and 
```c# 
base.OnNavigatedFrom(e);
```
Altogether this looks like:
```c# 
using TipCalc.UI.WindowsCommon.Common;
using Windows.UI.Xaml.Navigation;
using MvvmCross.WindowsCommon.Views;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TipCalc.UI.WindowsCommon.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TipView : MvxWindowsPage
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public TipView()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
```
### Edit the XAML layout

Double click on the XAML file

This will open the XAML editor within Visual Studio.

Again, I won't go into much depth at all here about how to use the XAML or do the Windows data-binding. I'm assuming most readers are already coming from at least a little XAML background.

To make the XAML inheritance match the `MvxWindowsPage` inheritance, change the outer root node of the Xaml file from:
```c# 
<Page 
    ... >
    <!-- content -->
</Page>
```
to:
```c# 
<views:MvxWindowsPage
    xmlns:views="using:MvvmCross.WindowsCommon.Views"
    ... >
    <!-- content -->
</views:MvxWindowsPage>
```
To add the XAML user interface for our tip calculator, we will add a `ContentPanel` Grid in place of the `ContentRoot` Grid in the existing XAML.

This `Content Panel` will include exactly the same XAML as we added to the WindowsCommon.WindowsPhone project except for the margins:

* a `StackPanel` container, into which we add:
  * some `TextBlock` static text
  * a bound `TextBox` for the `SubTotal`
  * a bound `Slider` for the `Generosity`
  * a bound `TextBlock` for the `Tip`

This will produce XAML like:
```c# 
<Grid Grid.Row="1" x:Name="ContentRoot" Margin="19,9.5,19,0">
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
**Note** that in XAML, `OneWay` binding is generally the default. To provide TwoWay binding we explicitly add `Mode` to our binding expressions: e.g. `Value="{Binding Generosity, Mode=TwoWay}"`


**Note** the binding for the TextBox uses `UpdateSourceTrigger=PropertyChanged` so that the `SubTotal` property of `TipViewModel` is updated immediately rather than when the TextBox loses focus.

In the designer, this will look like:

![Designer](https://raw.githubusercontent.com/sequence/MvvmCross/universalapping/v3Tutorial/Pictures/TipCalc_WindowsCommonWindowsPhone_Designer.png)

### The navigation cache

Universal Windows Phone apps seem to differ from Silverlight Windows Phone apps in that the default page navigation cache mechanism has changed. While Silverlight Windows Phone apps have built-in support for caching pages in forward/backward navigation, universal Windows Phone apps do not and need the NavigationHelper class and setting the NavigationCacheMode property of a Page to "Required" to provide this. This tutorial already showed you how to make use of the NavigationHelper and it is recommended that you do this even if you plan not to cache a view.

If you do enable caching by setting the NavigationCacheMode property of a Page to "Required" and navigate backwards or forwards, the view is retrieved from the cache. This includes the ViewModel property of the view. While this doesn't create a problem when navigating backwards, it does when you navigate forward! If you already have cached a view with a particular state (loading from the init parameters into the viewmodel), that state is also retrieved from the cache and you'll end up with a view with an 'old' viewmodel state.

To counter this, you must set the viewmodel to null when navigating to a page:
```c# 
protected override void OnNavigatedTo(NavigationEventArgs e)
{
    if (e.NavigationMode == NavigationMode.New)
        ViewModel = null;

    base.OnNavigatedTo(e);
    this.navigationHelper.OnNavigatedTo(e);
}
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