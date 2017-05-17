---
layout: documentation
title: Xamarin.Forms with MvvmCross
category: Platform specifics
---

With the introduction of MvvmCross 5.0 we have full support for Xamarin Forms!

# Installing and use MvvmCross.Forms

## Core

Start with adding the [nuget package](https://www.nuget.org/packages/MvvmCross.Forms/) for `MvvmCross.Forms` and the [MvvmCross Starterpack](https://www.nuget.org/packages/MvvmCross.StarterPack/) to all your platform projects.

Use `MvxContentPage` as base for your Xamarin.Forms pages.

## Android 
On Android inherit from `MvxFormsApplicationActivity` or `MvxFormsAppCompatActivity` on your main Activity.

Add the following piece of code to Setup.cs from Xamarin.Android project:
```c#
	protected override IMvxAndroidViewPresenter CreateViewPresenter()
    {
        var presenter = new MvxFormsDroidPagePresenter();
        Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

        return presenter;
    }
```

## iOS

On iOS inherit from `MvxFormsApplicationDelegate` on your App Delegate.

Add the following piece of code to Setup.cs from iOS project:

```c#
protected override IMvxIosViewPresenter CreatePresenter()
{
    Forms.Init();

    var xamarinFormsApp = new MvxFormsApp();
    return new MvxFormsIosPagePresenter(Window, xamarinFormsApp);
}
```

## Windows UWP

Modify Windows project's MainPage.xaml.cs ctor. in the following way:
```c#
public MainPage()
{
    InitializeComponent();
    SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

    var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsWindowsUWPPagePresenter;
    LoadApplication(presenter.MvxFormsApp);
}
```

Add the following piece of code to Setup.cs from Windows Phone project:
```c#
protected override IMvxPhoneViewPresenter CreateViewPresenter(PhoneApplicationFrame rootFrame)
{
    Forms.Init();

    var xamarinFormsApp = new MvxFormsApp();
    var presenter = new MvxFormsWindowsUWPPagePresenter(rootFrame, xamarinFormsApp);
    Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

    return presenter;
}
```

# Bindings

You can use the MvvmCross binding syntax just like you would do in a native Xamarin project. For more information see the [Bindings](https://www.mvvmcross.com/documentation/fundamentals/data-binding) documentation.

# MasterDetail support

Use the MasterDetail presenters to add support for it to your app.

# MvxImageView

Use the `MvxImageViewRenderer` for MvvmCross.Forms to have a compatible ImageView Renderer.
