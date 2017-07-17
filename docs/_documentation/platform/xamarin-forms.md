---
layout: documentation
title: Xamarin.Forms with MvvmCross
category: Platform specifics
---

With the introduction of MvvmCross 5.0 we have full support for Xamarin Forms!

# Tutorial

Let's take  a look at setting up a project

MvvmCross has some very helpful Nuget packages to get you started. In this case we will use the StarterPack Nuget to install the basic files and the MvvmCross.Forms Nuget to get us connected to Forms. Another great way is to use a Visual Studio extension like XabluCross for MvvmCross. In this guide I’m going to use Visual Studio for Mac to develop the sample App. You should be able to do the same using Visual Studio 2017, but things might just look slightly different.

Note: this tutorial is using MvvmCross 5.0.7. Using other versions may not work!

- Open up Visual Studio and start creating a New Solution in the File menu in the Menu.
- In the Multi-platform section select Blank Forms App.
- Enter the name for your new project, for this sample we will call it: MvxForms
- As Target platforms we leave the default selected ones, Android and iOS.
- For shared code we pick Portable Class Library. More information on that is available in the MvvmCross documentation.
- In this we will use XAML for our layouts so leave that on default.
- In the next page we leave the Project name and solution name to MvxForms. I would advice to create a .gitignore file if you are using Git. Press Create to finish.
- In MvvmCross it is common to name the shared code project ".Core". Change the name from MvxForms to MvxForms.Core.
- Open the Core, Android and iOS projects, double click Packages and add the MvvmCross.StarterPack nuget and the MvvmCross.Forms nuget.
- Remove the ToDo-MvvmCross and Views folders in the Android and iOS projects.
- Change the App.cs name from:

```c#
public class App : MvvmCross.Core.ViewModels.MvxApplication
```

To:

```c#
public class CoreApp : MvvmCross.Core.ViewModels.MvxApplication
```

We do this because otherwise the Forms.App class conflicts with the MvvmCross.App class.

- Edit the App.xaml file from:

```c#
<FormsApplication xmlns="http://xamarin.com/schemas/2014/forms"
     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
     x:Class="MvxForms.Core.App" 
</FormsApplication>
```

To:

```c#
<d:MvxFormsApplication xmlns="http://xamarin.com/schemas/2014/forms"
     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
     x:Class="MvxForms.Core.App" 
     xmlns:d="clr-namespace:MvvmCross.Forms.Core;assembly=MvvmCross.Forms">
</d:MvxFormsApplication>
```

Do the same for App.xaml.cs

```c#
public partial class App : FormsApplication
```

To:

```c#
public partial class App : MvxFormsApplication
```

- Remove the line 'MainPage = MvxFormsPage();' from App.xaml.cs.
- In the Setup.cs class of Android and iOS, change:

```c#
protected override IMvxApplication CreateApp()
{
    return new Core.App();
}
```

To:

```c#
protected override IMvxApplication CreateApp()
{
    return new Core.CoreApp();
}
protected override MvvmCross.Forms.Core.MvxFormsApplication CreateFormsApplication()
{
    return new App();
}
```

- Also change the base class of the Android setup from MvxAndroidSetup to MvxFormsAndroidSetup. Do the same on iOS from MvxIosSetup to MvxFormsIosSetup.
- Note, if you get the error: No resource found that matches the given name (at ‘icon’ with value ‘@mipmap/icon’). Go to SplashScreen.cs and change Icon = “@mipmap/icon” to Icon = “@drawable/icon”.
- On Android in your Activity replace theOnCreate method with:

```c#
protected override void OnCreate(Bundle bundle)
{
    TabLayoutResource = Resource.Layout.Tabbar;
    ToolbarResource = Resource.Layout.Toolbar;
 
    base.OnCreate(bundle);
 
    global::Xamarin.Forms.Forms.Init(this, bundle);
 
    var formsPresenter = (MvxFormsPagePresenter)Mvx.Resolve<IMvxAndroidViewPresenter>();
    LoadApplication(formsPresenter.FormsApplication);
}
```

On iOS replace your AppDelegate code with:

```c#
public partial class AppDelegate : MvxFormsApplicationDelegate
{
   public override UIWindow Window { get; set; }
 
   public override bool FinishedLaunching(UIApplication app, NSDictionary options)
   {
        Window = new UIWindow(UIScreen.MainScreen.Bounds);
 
        var setup = new Setup(this, Window);
        setup.Initialize();
 
        var startup = Mvx.Resolve<IMvxAppStart>();
        startup.Start();
 
        LoadApplication(setup.FormsApplication);
 
        Window.MakeKeyAndVisible();
 
        return true;
    }
}
```

- The last step is to make your Xamarin.Forms page extend the MvxContentPage. You can either use a generic or name based convention to make MvvmCross recognize it. In MvxFormsPage.xaml.cs change:

```c#
public partial class MvxFormsPage : ContentPage
```

To:

```c#
public partial class MvxFormsPage : MvxContentPage<MainViewModel>
```

- You need to change the XAML page attached to it as well:

```c#
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
     xmlns:local="clr-namespace:MvxForms.Core">
```

To:

```c#
<d:MvxContentPage x:TypeArguments="viewModels:MainViewModel"
     xmlns="http://xamarin.com/schemas/2014/forms"
     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
     xmlns:local="clr-namespace:MvxForms.Core"
     x:Class="MvxForms.Core.MvxFormsPage"
     xmlns:viewModels="clr-namespace:MvxForms.Core.ViewModels;assembly=MvxForms.Core"
     xmlns:d="clr-namespace:MvvmCross.Forms.Core;assembly=MvvmCross.Forms">
```

- Note, if you are using a SplashScreen on Android you need to add:

```c#
protected override void TriggerFirstNavigate()
{
    StartActivity(typeof(MainActivity));
    base.TriggerFirstNavigate();
}
```

Otherwise your Forms Activity wouldn't run and you'll stuck on the SplashScreen.

## Fire up the App and enjoy!

The result of this tutorial is available at: https://github.com/martijn00/MvxForms


## Windows UWP

Modify Windows project's MainPage.xaml.cs ctor. in the following way:
```c#
public MainPage()
{
    InitializeComponent();
    SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

    var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsWindowsUWPPagePresenter;
    LoadApplication(FormsApplication);
}
```

# Bindings

You can use the MvvmCross binding syntax just like you would do in a native Xamarin project. For more information see the [Bindings](https://www.mvvmcross.com/documentation/fundamentals/data-binding) documentation.

# MasterDetail support

Use the MasterDetail presenters to add support for it to your app.

# MvxImageView

Use the `MvxImageViewRenderer` for MvvmCross.Forms to have a compatible ImageView Renderer.
