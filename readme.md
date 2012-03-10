# MvvmCross

This project provides a cross-platform mvvm mobile development framework built on top of Silverlight for WP7, Mono for Android and MonoTouch for iOS.

# Getting started

To learn about what MVVM is... please look at this introduction from Infragistics http://blogs.infragistics.com/blogs/anand_raja/archive/2012/02/20/the-model-view-viewmodel-101-part-1.aspx and http://blogs.infragistics.com/blogs/anand_raja/archive/2012/03/06/introduction-to-the-model-view-viewmodel-pattern-part-2.aspx.

To see how MvvmCross provides ViewModels, Views and bindings, see the tutorial steps in the wiki - https://github.com/slodge/MvvmCross/wiki/_pages

# About MvvmCross...

This project was born from:

- a branch of the MonoCross project, moving away from MVC and towards MVVM
- an extension project from http://www.cirrious.com
- some ideas from MvvmLight
- some ideas from Phone7.Fx (OpenNetCF)

# Current state

Currently working:

- Android 
- Touch - fully working on iPhone - some iPad master/detail/popup/etc code working on the MWC sample
- WindowsPhone 
- Book sample
- CustomerManagement sample
- A Tutorial sample
- Console (partly working - needs to move to NUnit)

Will not be progressed:

- WebKit - we simply don't believe that a web server client belongs here in this mobile mvvm framework...

# Future direction

Under consideration

- More tablet support - for iPad, for Windows Metro and for Android
- Blendability
- Test integration
- Better Samples
- More cross platform services (accelerometer, contacts, sql, etc)

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