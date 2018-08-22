---
layout: post
title: Phasing out Silverlight
date:   2016-07-06 11:37:35 +0100
categories: mvvmcross
---
Some of you may have noticed that some projects in 4.2.1 have had their PCL profiles adjusted. We are gradually phasing out anything Silverlight.
This will also mean that Windows Phone 8 Silverlight projects will be removed from NuGet packages in the near future.

Abandoning Silverlight, allows us to use PCL profiles with a larger subset of the full .NET framework and increased compatibility with a lot of the Microsoft NuGet packages for collections etc.