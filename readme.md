# MvvmCross vNext

This project provides a cross-platform mvvm mobile development framework built on top of Silverlight for WP7, Mono for Android and MonoTouch for iOS, and the WinRT XAML framework for Windows 8 Store applications.

This project makes extensive use of Portable Class Libraries to provide maintainable cross platform C# native applications.

# Getting started

For an overview of how I personally develop, see http://slodge.blogspot.co.uk/2012/11/azure-to-wpmonodroidmonotouch-video-of.html or:

![youTube](http://img.youtube.com/vi/0jGz__A_2pk/1.jpg) ![youTube](http://img.youtube.com/vi/0jGz__A_2pk/2.jpg) ![youTube](http://img.youtube.com/vi/0jGz__A_2pk/3.jpg)
http://www.youtube.com/watch?v=0jGz__A_2pk

For a list of all sorts of resources, questions and answers, see http://slodge.blogspot.co.uk/p/mvvmcross-quicklist.html

For setting up your development environment to support portable libraries, see the steps in http://slodge.blogspot.co.uk/2012/09/mvvmcross-vnext-portable-class.html

To learn about what MVVM is... please look at this introduction from Infragistics http://blogs.infragistics.com/blogs/anand_raja/archive/2012/02/20/the-model-view-viewmodel-101-part-1.aspx and http://blogs.infragistics.com/blogs/anand_raja/archive/2012/03/06/introduction-to-the-model-view-viewmodel-pattern-part-2.aspx.

For some background on Portable Libraries see http://blogs.msdn.com/b/dsplaisted/archive/2012/08/27/how-to-make-portable-class-libraries-work-for-you.aspx

http://vimeo.com/39019207 provides a video of me talking about this project (with poor noise quality - sorry...) 

For more presentations (without sound) see http://www.slideshare.net/cirrious/

MvvmCross made it briefly to Channel9 - see http://slodge.blogspot.co.uk/2012/06/mvvmcross-on-channel9.html

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
- Aviva Drive - http://www.aviva.co.uk/drive
- Origo Foci-Eb 2012 - http://slodge.blogspot.co.uk/2012/10/origo-foci-eb-2012-example-mvvmcross.html
- Various Conference apps - SQLBitsX, DDDSW, LondonAzure, .... e.g. https://github.com/slodge/MvvmCross/tree/vnext/Sample%20-%20CirriousConference
- The CrossBox DropBox client - https://github.com/runegri/CrossBox
- The Blooor shopping list app - https://github.com/Zoldeper/Blooor
- Have you used the app? Please send me your links and I'll add them here

# vNext

This is the second version of the MvvmCross platform - vNext.

This version specifically provides:

- the same databinding and viewmodel framework from the original MvvmCross release
- an increase of code sharing through the use of portable class libraries
- a new plugin structure which makes it easy to consume and to reuse platform-specific code within your views and viewmodels

The motivation for the changes between version 1 and vNext are summarised in: http://www.slideshare.net/cirrious/mvvm-cross-going-portable
 
If you are looking for the "old" version of MvvmCross, then it's still available and still receiving updates on the master branch at https://github.com/slodge/MvvmCross/tree/master

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

There is a Jabbr room - on http://jabbr.net/#/rooms/mvvmcross - but please ask questions on StackOverflow if you can - this helps make the questions and the answers better!

If I haven't seen your question and no-one else has helped, then you can also contact me - me at slodge dot com - but please be patient with me if I'm busy with work.

If you need professional support, then please do contact me - I work freelance and can assist. Please do consider hiring me - everyone likes 'free', but I'm worth paying for :)

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
- Sqlite-net - custom license - see https://github.com/praeclarum/sqlite-net/blob/master/license.md

- To be documented: MonoTouch.Dialog, MonoDroid.Dialog (not currently used), MonoTouch.NinePatch (not currently used), OpenNetCF IoC (used from wtihin Phone7.Fx?), code from Xamarin samples, code from PullToRefresh

# Thanks

Thanks to McCannLondon for sponsoring the initial part of this work - http://blogs.mccannlondon.co.uk/

Thanks to lots of people for their input over time - will add to this list...

Thanks to JetBrains for a community Resharper license to use on this project.

Thanks to Infragistics for a control license for Nuclios and .Net.

If you want to support this project, please do get in touch!