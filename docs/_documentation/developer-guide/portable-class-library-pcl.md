---
title: "Portable Class Library (PCL)"
excerpt: ""
---
**Why PCL and not Shared Project?**

With C# Views it is possible to write the MvvM either as a PCL or as a shared project. It is not possible to do that with XAML, you must use a PCL. You might think that this would be a disadvantage because in C# shared projects you can use "#ifdef" statements to conditionally compile code dependent on target platform. Thus you might reason that this would make maintaining and understanding the coding more productive.

However [Xamarin PCL](http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/platform-specifics/) achieves a similar outcome using a class known as the Device. This class although part of the Portable Class Library has methods that return different values depending on the native platform your App is installed on.

The Device static class is available in both C# and XAML coding if you know how to use it.

In our opinion then, the MvvM should always be coded in a PCL. In a later example though we will demonstrate how a solution based on PCL can also make use of a shared project to native platform interfacing.

Examples of Device function usage:

- **Device.OS** returns an enum so you know what Operating system you are on. enum TargetPlatform {Other, iOS, Android, WinPhone, Windows}

- **Device.OnPlatform** this allows you to choose an appropriate value dependent on platform. This function can be used directly in XAML coding whereas most Device functions will require you to interface to CSharp to access.

- **Device.Idiom** another enum that will tell you if you are on a Tablet or a Phone.

- **Device.GetNamedSize** used for getting fonts using device relative sizing.

With the exception of OnPlatform these static functions cannot be used directly in XAML. However you can write a small static class and then use them in XAML Resource dictionaries. We should demonstrate this later.

Shared Project though can be useful when writing platform level code that needs to be varied slightly depending on the target platform. For example if you need to create a file to hold a database or a secret each OS has a different file system location for that, and if you need to apply encryption while the device is code locked to protect a secret and exposure via Jailbreaking, again each OS is different. But all platforms can be coded in a shared project including "#ifdef" sections where the platforms differ. This makes the code more maintainable because you can then see all of your platforms at once.

Again we should demonstrate this later.