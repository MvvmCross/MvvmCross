---
layout: documentation
title: Tip Calculator - A recap
category: Tutorials
---
Over the course of these articles, we've covered the complete `Tip Calc` app on 5 platforms from one shared PCL code library using Mvvm.

![Sketch](https://raw.github.com/slodge/MvvmCross/v3/v3Tutorial/Pictures/TipCalc_Summary.png)

I hope this was simple and easy to follow...

Just to recap what we did:

* For the core PCL library, we:
 * created a Profile 259 library
 * added some `MvvmCross` PCL libraries
 * added our services - the `Calculator`
 * added our `TipViewModel` which exposed properties
 * added our `App` which wired the services together and defined an `IMvxAppStart`
* For each platform, we generally went through a process like:
 * created a platform specific project
 * added some `MvvmCross` PCL libraries
 * added some `MvvmCross` platform specific libraries
 * added a `Setup` class which would initialise everything
 * modified the platform-specific Application to call `Setup`
 * created a `Views` folder
 * added a platform specific `View`
 * changed that `View` base class to something beginning with `Mvx`
 * added a `public new TipViewModel ViewModel` to link the `View` to the `ViewModel`
 * modified the XML for that `View` to add the UI fields
 * modified those UI fields to add databinding to the `ViewModel` properties
 * pressed 'Run' :)
 * considered some ways that the UI could be improved using platform-specific UI techniques

Generally, these same steps are what you'll follow for each MvvmCross application you want to make.

We'll cover more advanced topics in future articles.

Thanks for reading

Stuart