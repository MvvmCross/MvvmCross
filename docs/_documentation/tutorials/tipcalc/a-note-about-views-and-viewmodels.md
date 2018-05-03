---
layout: documentation
title: Extra - Views and ViewModels
category: Tutorials
order: 9
---

## View <-> ViewModel

During this tutorial we have used generic parameters to link our Views / ViewModels. This is the recommended approach and it provides you with many advantages.

There is an alternative however. If you don't use generic parameters, then MvvmCross will make use of some built in "Convention over Configuration" to figure out how to link classes.

MvvmCross can locate a ViewModel for a View as long as both members have the same root and suffixes `View` & `ViewModel`. In our case, MvvmCross will look for a `TipViewModel` for `TipView`. 

More notes on this docs to be added! :)

[Next!](https://www.mvvmcross.com/documentation/tutorials/tipcalc/the-tip-calc-navigation)