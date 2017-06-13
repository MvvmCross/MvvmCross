---
layout: post
title: 3.2.1 Released - One Windows support :)
date:   2014-09-18 11:37:35 +0100
categories: mvvmcross
---

I've pushed a final attempt at 3.2.1 to nuget :)

I've also made 3.2 the default branch on https://github.com/MvvmCross/MvvmCross 


This build is finally out of beta as several people have now reported success with using the new Universal Project support :)

If you do find issues, then please do report them as you find them... we'll get the updates out there...

The main feature of this 3.2.1 build includes some marvellous WindowsCommon support - for Jupiter WindowsPhone Xaml with Windows 8.1 Xaml.

This is especially thanks to:

- the lovely https://github.com/jonstoneman and https://github.com/steveydee82 working at https://twitter.com/sequenceagency who have been pioneering lots of amazing shared code Jupiter apps
- the fab https://twitter.com/pedrolamas who makes the brilliant http://cimbalino.org/ and the rest of the team who work on my music player of choice - https://twitter.com/NokiaMixRadio

The support means you now must use a "new profile" like Profile 259 or Profile 78 to get working... don't blame me for this... blame Microsoft ;)

If you want to try this Jupiter code, then you can now try building a Universal WindowsPhone/WindowsStore app - using the new Universal projects - using the new "WindowsCommon" assemblies inside native projects or inside a PCL of profile 32. 

I'm afraid we haven't updated all the N+1 and Tutorial samples to the new profiles - just haven't had the time to go through and do them all.

At a more detailed level, this build 3.2.1 includes:

- Universal WindowsCommon app support
- Fixes to Universal WindowsCommon PictureChooser and PhoneCall plugins
- a fix for Title bindings in UIButton in iOS
- some PictureChooser scaling and memory fixes (for iOS)
- an infinite exception loop fix in the debug output sample files
- nuget fixes for windowscommon
- a default parameter added to WithConversion in fluent bindings
- a null reference fix in the reflection code - when the linker has stripped out property getters/setters
- a fix to improve ReloadState finding across multiple inhertiance hierarchy layers
- ImeAction.Previous has been changed to match Xamarin's change of Android version
- Json now has `ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize` set by default
- A fix for double queryString escaping in WindowsPhone navigation
- A fix for empty cc lists in the email plugin in iOS
- An optimisation of resource image loading (fromBundle instead of fromFile) 
- A fix for UIDatePicker centering in MT.Dialog
- An attempted fix for weak ref issues with CanExecuteChanged in ICommand in iOS
- A fix for multiple file flushes  in WriteFile in the File plugin


This 3.2.1 update did also includes some attempts at getting Symbols uploaded for nuget too - but this isn't quite finished yet... seems like this nuget functionality doesn't work without a little effort for multiple assemblies in the same nupkg.


This 3.2.1 build doesn't include any direct Xam.Forms support - https://twitter.com/Cheesebaron has pushed a fab sample about that to https://github.com/Cheesebaron/Xam.Forms.Mvx/ - beyond that Xamarin have also said there are some Mvx/Forms combination samples coming, but I don't have any inside info on these.

https://twitter.com/Cheesebaron has also done some fabulous Fragment changes recently - https://github.com/MvvmCross/MvvmCross/pull/771 - these will be included in a 3.3 release as soon as we have them ready :)

OK... that's all from me for now... huge thanks to the devs from Sequence Agency and to Microsoft/Nokia MixMusic for contributing their considerable skill, talent, effort and code to getting these Universal project changes included - thanks :)

Stuart
