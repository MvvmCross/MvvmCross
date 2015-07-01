MvvmCross-Forms
============
### [MvvmCross](https://github.com/MvvmCross/MvvmCross) support for the [Xamarin Forms](http://http://xamarin.com/forms). ###

This Repo is currently under development. The objective is to greatly enhance the pioneering work of Cheesebaron (Tomasz Cielecki)
in getting MvvMCross to work with Xamarin.Forms.

### Objectives ###
- Sample Projects for both c# and XAML Xamarin.Forms
- Sample projects to be configured to use NuGet for linkages to: Xamarin.Forms, Cheesebaron.MvxPlugins, MvvmCross.HotTuna.CrossCore
- Forms Presenter and sample application for VS 2015 Windows Store Application
- This work creates a Portable Class Library that must be upgraded to PCL Profile 259 to target additional targets introduced in VS 2015: Window 8+ Windows Phone 8+ (no silverlight also known as Windows Store Apps)
- This work uses PCL components like MvvmCross.HotTuna.CrossCore that must also be upgraded to PCL 259
- NuSpec strings will conform to the Xamarin.Forms standard
- Some dependencies may need to be bundled into this packaged temporarily while we resolve issues with dependencies
- Sample applications will be used for testing, and we will create VSIX file so that the Sample can be installed into Visual Studio


Contributions
---------

Pull requests welcome!

Licensing
---------

This project is developed and distributed under the [Microsoft Public License (MS-PL)](http://opensource.org/licenses/ms-pl.html).
