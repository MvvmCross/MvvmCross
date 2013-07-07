# MvvmCross v3

This project provides a cross-platform mvvm mobile development framework built on top of:

- Silverlight for WP7, WP8
- Mono for Android  (or Xamarin.Android)
- MonoTouch for iOS  (or Xamarin.iOS)
- the WinRT XAML framework for Windows 8 Store apps.
- WPF
- Mono for Mac (or Xamarin.Mac)

This project makes extensive use of Portable Class Libraries to provide maintainable cross platform C# native applications.

# Getting started
I have started a tutorial series entitled [N+1 Days of MvvmCross](http://mvvmcross.wordpress.com) starting with the basics of your first MvvmCross application and covering lists, commands, binding, and more with samples for each device. The code for N+1 Days of MvvmCross is available on [GitHub](https://github.com/slodge/NPlus1DaysOfMvvmCross)

For more info and articles - see the wiki: https://github.com/slodge/MvvmCross/wiki

For more samples, see: https://github.com/slodge/MvvmCross-Tutorials/

# About MvvmCross...

This project was born from:

- a fork once long ago of the MonoCross project, moving away from MVC and towards MVVM
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
- many more!

# v3

This is the third version of the MvvmCross platform - v3.

For more info on what this version specifically aims to provide, see:

- http://slodge.blogspot.co.uk/2013/02/mvvmcross-v3.html

If you are looking for the "older" version of MvvmCross, then it's still available and still receiving updates on the master branch at

- https://github.com/slodge/MvvmCross/tree/master
- https://github.com/slodge/MvvmCross/tree/vNext

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
- MonoTouch.Dialog - TODO
- MonoDroid.Dialog - TODO
- OpenNetCF IoC (used from wtihin Phone7.Fx?) - TODO
- To be documented: MonoTouch.NinePatch (not currently used), code from Xamarin Conference samples, code from PullToRefresh
- Messenger ideas from JonathanPeppers/XPlatUtils and from GrumpyDev/TinyMessenger

# Thanks

Thanks to McCannLondon for sponsoring the initial part of this work - http://blogs.mccannlondon.co.uk/

Thanks to lots of people for their input over time - will add to this list...

Thanks to JetBrains for a community Resharper license to use on this project.

Thanks to Infragistics for a control license for Nuclios and .Net.

If you want to support this project, please do get in touch!
