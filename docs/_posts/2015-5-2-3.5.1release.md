---
layout: post
title: 3.5.1 release
date:   2015-05-02 11:37:35 +0100
categories: mvvmcross
---

We've finally pushed Mvx 3.5.1 on nuget

Some significant changes in this release are:

- changes to binding strong `target` storage - to try to address issues with the latest Xamarin GC releases on iOS
- changes to Android Java naming for all views
- changes to Android inflation to assist with changes in Android support packages


Additionally, we have included:

- addition of a FromStoryboard attribute
- Touch setup changes to ease XamForms integration
- a fix for dynamic image helper (when reused in some error situations)
- a Fragments suspension fix
- addition of open generic support for binding registration
- addition of MvxOwnerViewModelFragment - for "old" style fragments
- GroupTemplateId added to MvxExpandableListView
- support for SelectedSegment binding in UISegmentedControl
- additions to support suspension management in WinStore and WinCommon
- a fix for camera picker leaking on iOS
- additional protected constructor added for dialog controller
- typo fix in Android target bindings
- some packages now have symbols on nuget - but not all - this is still a (frustrating) work in progress
- IntPtr constructor added to MvxFrameControl
- improvements to serialization for fragments presenter
- CreateIoCOptions added for unit tests
- bindable ExpandableListView added for Android
- Seekbar binding becomes a better C# citizen
- setter only property fix (broken by universal project changes)
- simple addition of INPC to assist with linking
- addition of GetFoldersIn to File plugin
- some test changes - to ensure invariant culture used
- fix for recursive delete on WinPhone Silverlight
- switch to automatic nuget package restore
- fix for selecteditem being fired for empty lists (UIPickerView)
- addition of clear functionality for download cache
- additions and improvemets to file plugin on windows
- prevention of loading 0 id resource bitmaps on droid
- missing saveinstance hooks added to fragment wrappers
- additional Action hooks added to RaiseAndSetIfChanged
- mem caching added for local resource images
- file access improved on foreach loop
- Ioc supporting tests switched to invariant culture


You can see these changes in source on https://github.com/MvvmCross/MvvmCross/commits/3.5

As ever we're keen to get feedback on this :) Let us know if things are broken or could be even more awesome

Thanks to all the fab contributors :) This patch includes: Sylapse, geirsagberg, martijn00, azchohfi,, Paul Kapustin, brsolucoes, pazi146, kjeremy, vzsg, tal33, jamie94bc, David Schwegler, cclarke, Kevin Ford, Paul Leman, Eugene Berdnikov, Bognar, Seifer, Jihun Lee, Kerry Lothrop, Ben B, Tomasz, Daniel W, guillaume-fr, Mohib

There's an  issue open on GitHub about changing the way the project is managed - bringing more people into the team, enabling more development and stopping certain old dinosaurs (me) slowing the project down - see https://github.com/MvvmCross/MvvmCross/issues/841 - would you like to be involved?
