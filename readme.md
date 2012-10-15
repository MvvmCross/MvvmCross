# MvvmCross vNext

This project provides a cross-platform mvvm mobile development framework built on top of Silverlight for WP7, Mono for Android and MonoTouch for iOS, and the WinRT XAML framework for Windows 8 Store applications.

This project makes extensive use of Portable Class Libraries to provide maintainable cross platform C# native applications.

# Getting started

For setting up your development environment to support portable libraries, see the steps in http://slodge.blogspot.co.uk/2012/09/mvvmcross-vnext-portable-class.html

To learn about what MVVM is... please look at this introduction from Infragistics http://blogs.infragistics.com/blogs/anand_raja/archive/2012/02/20/the-model-view-viewmodel-101-part-1.aspx and http://blogs.infragistics.com/blogs/anand_raja/archive/2012/03/06/introduction-to-the-model-view-viewmodel-pattern-part-2.aspx.

http://vimeo.com/39019207 provides a video of me talking about this project (with poor noise quality - sorry...) 

For more presentations (without sound) see http://www.slideshare.net/cirrious/

To see how MvvmCross provides ViewModels, Views and bindings, see the samples within this project. This really is the best "Getting Started" information available.

For International inspiration, see:

- http://www.slideshare.net/Runegri/kryssplatform-mobilutvikling
- http://www.slideshare.net/dan_ardelean/mvvmcross-da-windows-phone-a-windows-8-passando-per-android-e-ios
- http://www.e-naxos.com/Blog/post/Strategie-de-developpement-Cross-Platform-Partie-2.aspx

# About MvvmCross...

This project was born from:

- a branch of the MonoCross project, moving away from MVC and towards MVVM
- an extension project from http://www.cirrious.com
- some ideas from MvvmLight
- some ideas from ASP.NET MVC
- some ideas from OpenNetCF (Phone7.Fx)
- lots of my own ideas - http://www.cirrious.com and http://slodge.blogspot.co.uk/

Here is the framework in action for the sqlbits conference app:
![sql bits](http://i.imgur.com/lVPv1.png)
<!-- http://i.imgur.com/vfWen.png -->

Public projects that have used this framework include:

- Kinect Star Wars - http://www.youtube.com/watch?v=MXPE2iTvlWg
- Aviva Drive - http://www.aviva.co.uk/drive/
- Various Conference apps - SQLBitsX, DDDSW, LondonAzure, ....
- The CrossBox DropBox client - https://github.com/runegri/CrossBox
- Have you used the app? Please send me your links and I'll add them here

# vNext

This is the second version of the MvvmCross platform - vNext.

This version specifically provides:

- the same databinding and viewmodel framework from the original MvvmCross release
- an increase of code sharing through the use of portable class libraries
- a new plugin structure which makes it easy to consume and to reuse platform-specific code within your views and viewmodels

The motivation for the changes between version 1 and vNext are summarised in: http://www.slideshare.net/cirrious/mvvm-cross-going-portable
 
If you are looking for the "old" version of MvvmCross, then it's still available and still receiving updates at https://github.com/slodge/MvvmCross/tree/master

# What's here...

Currently included:

- Mono for Android 
- MonoTouch 
- WindowsPhone (mainly WP7)
- WinRT
- some Console/NUnit work (although this is not supported as a full deployment target currently)

Samples:

- Book sample
- Conference sample
- CustomerManagement sample
- Tutorial sample
- TwitterSearch sample

With:

- Some empty project templates to use in VisualStudio (see http://slodge.blogspot.co.uk/2012/10/some-project-templates-for-vnext.html)

# How to get involved

If you have questions, then please ask them on StackOverflow with tag mvvmcross - http://stackoverflow.com/questions/tagged/mvvmcross

Alternatively, people on Xamarin forums can be very helpful - http://forums.xamarin.com/

If I haven't seen your question and no-one else has helped, then you can also contact me - me at slodge dot com - but please be patient with me if I'm busy with work.

If you need professional support, then please do contact me - I work freelance and can assist.

If you find bugs or have feature requests, then please report them through GitHub - https://github.com/slodge/MvvmCross/issues

If you would like to help make MvvmCross even better, then please do:

- new code - including pull requests via GitHub - or you can fork the project and build your own extensions
- new plugins - can be hosted in your own repositories
- please do blog about your adventures with MvvmCross - we're currently very light on documentation!
- if you use the framework, then please let me know - we love to see what people are doing with it.

# Licensing

This project is developed and distributed under Ms-Pl - see http://opensource.org/licenses/ms-pl.html

- MonoCross was the original starting point for this project, and was used as a reference under MIT - please see http://code.google.com/p/monocross/ for original details.
- Phone7.Fx is redistributed and modified here under Ms-PL - please see http://phone7.codeplex.com for original details.
- Tiny bits of MvvmLight are redistributed and modified here under MIT - please see http://mvvmlight.codeplex.com/ for original details.
- NewtonSoft.Json is redistributed and modified here under MIT - please see http://json.codeplex.com for original details. 
- The original work on the JSON.Net port to MonoTouch and MonoDroid was done by @ChrisNTR - https://github.com/chrisntr/Newtonsoft.Json

- To be documented: MonoTouch.Dialog, MonoDroid.Dialog (not currently used), MonoTouch.NinePatch (not currently used), OpenNetCF IoC (used from wtihin Phone7.Fx?), code from Xamarin samples, code from PullToRefresh

# Thanks

Thanks to McCannLondon for sponsoring the initial part of this work - http://blogs.mccannlondon.co.uk/