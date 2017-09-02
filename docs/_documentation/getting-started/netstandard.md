---
layout: documentation
title: .NET Standard & MvvmCross
category: Getting-started
order: 4
---

This document will briefly cover how you use .NET Standard in your core project, whilst still using the PCL based MvvmCross packages. This also works for other non-MvvmCross projects, where you want to have your core .NET Standard and still want to consume PCL projects in it.

This document will asume that you are using the new `csproj` files and not using `project.json` to define your .NET Standard project.

In order to consume PCL projects in any .NET Standard project you need to add a package target fallback for the PCL profile you want to consume. So let us assume you want to add `MvvmCross.Bindings` to your project. This library uses the PCL profile which has a signature that looks like `portable-net45+win+wpa81+wp80`. To add this as a target fallback you simply edit your `csproj` file and add the following line to it.

```xml
<PackageTargetFallback>portable-net45+win+wpa81+wp80</PackageTargetFallback>
```

This line would normaly belong in the `PropertyGroup` which contains the `TargetFramework`, so it would look like. You can have multiple `;`-separated fallbacks here, which helps you consume different profiles.

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <PackageTargetFallback>portable-net45+win+wpa81+wp80</PackageTargetFallback>
  </PropertyGroup>

  <!-- other entries -->
</Project>
```

From here you should be able to add MvvmCross and other PCL based NuGet packages to your .NET Standard project.