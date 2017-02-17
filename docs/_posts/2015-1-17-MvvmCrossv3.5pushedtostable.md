---
layout: post
title: MvvmCross v3.5 pushed to stable
date:   2015-01-17 11:37:35 +0100
categories: mvvmcross
---

MvvmCross v3.5 pushed to stable
Now that Xamarin Unified support has gone "stable" and since we've had such good positive feedback about 3.5.0-beta releases...

MvvmCross 3.5.0 is now also released as stable :)

This update was built with the stable channel - Xamarin iOS 8.6

The changes since 3.2 are:

- The inclusion of the new "Woah Fragments" architecture from Cheesebaron :)
- The switch to the Xamarin Unified iOS API - with lots of updates as Xamarin's changes have evolved
- The return of Generic base ViewController's and Activity's (No more Heisenbugs - hurrah!) + a recent fix to this change
- A new binding for UISegmentedControl's
- Improvements to the way RaisePropertyChanged can be called
- Improvement in Windows support for generic MvxCommand class
- Small updates to LinkerPleaseInclude for both Android and iOS
- WindowsPhone has Heading added to the geolocation plugin
- Some NSDate - DateTime auto-conversion fixes
- WPF branch now relies on Blend 4.5 Interactivity, not Blend 4.0
- Support for Xamarin.Mac Unified API (Classic API and MonoMac still supported - but not well documented!)

When updating Android projects from 3.2, I'm aware that some people may experience issues with the old "portable shim" assemblies (system.net, system.xml.serialization, etc) being left as references - the workaround for this seems to be to remove those references manually.

When updating iOS projects.... then the main steps seem to be "normal" Unified updates - see Kerry's blog for some help with these - http://kerry.lothrop.de/unified-api-for-xamarin-ios/


Thanks hugely to everyone who's contributed - especially:

- @Cheesebaron for the great work on "Woah Fragments!" - https://github.com/MvvmCross/MvvmCross/pull/771
- @kwlothrop for so much work on building with the Unified API - if you are porting your libraries and code, see http://kerry.lothrop.de/unified-api-for-xamarin-ios/
- @tofutim for pushing on with Mac libraries too!
- @gshackles for being our guinea-pig (canary?) on lots of the updates

Really - these guys are awesome! They've done the work on this! Thanks!

If you want to get involved, then please read https://github.com/MvvmCross/MvvmCross/issues/841 and please do join in :)
