---
layout: documentation
title: TipCalc - Summary
category: Tutorials
order: 8
---

Over the course of these articles, we've covered the complete `Tip Calc` app on many platforms, used a .NET Standard library to share code and took advantage of the MVVM pattern to structure our solution.

![Summary]({{ site.url }}/assets/img/tutorials/tipcalc/TipCalc_Sketch.png)

We really hope this tutorial was simple and easy to follow.

Just to recap what we did:

For the _Core_, we:
- Created a .NET Standard 2 library
- Added the `MvvmCross` package
- Added a service: - `ICalculatorService`
- Added our `TipViewModel` which exposed several properties to be consumed
- Added our `App` class, which wired the services together and defined an `IMvxAppStart`

For each platform, we generally went through a process like:
- Created a platform specific project
- Added the `MvvmCross` package    
- Modified the platform-specific 'Application' to let MvvmCross be initialized
- Used the default provided `Setup` class for that platform
- Created a `Views` folder
- Added a platform specific `TipView`, which inherited from something beginning with `Mvx`
- Modified the layout for that View and added some widgets
- Added data-binding to the platform view, targeting the `TipViewModel` properties
- Pressed 'Run' :)

Generally, these same steps are what you'll follow for each MvvmCross application you want to make.

This is the final article for the TipCalc app, we hope you have enjoyed the lectures and we hope you have a great time developing apps with MvvmCross.

In order to provide some follow-up content, we will talk about ViewModels and Navigation in the final two articles.

[Next!](https://www.mvvmcross.com/documentation/tutorials/tipcalc/a-note-about-views-and-viewmodels)

