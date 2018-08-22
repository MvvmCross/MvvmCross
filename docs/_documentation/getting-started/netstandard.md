---
layout: documentation
title: .NET Standard & MvvmCross
category: Getting-started
order: 4
---

This document will briefly cover how you use .NET Standard in your core project, whilst still using the PCL based MvvmCross packages. This also works for other non-MvvmCross projects, where you want to have your core .NET Standard and still want to consume PCL projects in it.

This document will asume that you are using the new `csproj` files and not using `project.json` to define your .NET Standard project.

## .NET Standard 1.0 - 1.6

In order to consume PCL projects in a .NET Standard project targeting versions 1.0 through to 1.6, you need to add a package target fallback for the PCL profile you want to consume. So let us assume you want to add `MvvmCross.Bindings` to your project. This library uses the PCL profile which has a signature that looks like `portable-net45+win+wpa81+wp80`. To add this as a target fallback you simply edit your `csproj` file and add the following line to it.

```xml
<PackageTargetFallback>$(PackageTargetFallback);portable-net45+win8+wp8+wpa81;</PackageTargetFallback>
```

This line would normally belong in the `PropertyGroup` which contains the `TargetFramework`, as shown below. You can have multiple `;`-separated fallbacks here, which helps you consume different profiles.

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard1.4</TargetFramework>
    <PackageTargetFallback>$(PackageTargetFallback);portable-net45+win8+wp8+wpa81;</PackageTargetFallback>
  </PropertyGroup>

  <!-- other entries -->
</Project>
```

From here you should be able to add MvvmCross and other PCL based NuGet packages to your .NET Standard project.

## .NET Standard 2

When using .NET Standard 2 you do not need to specify a package target fallback. In .NET Standard 2 the `PackageTargetFallback` flag has been deprecated and instead defaults to net461 (.NET Framework 4.6.1) or higher. If however, this does not suit your use case you can override this behaviour with the `AssetTargetFallback`.

```xml
<AssetTargetFallback>$(AssetTargetFallback);portable-net45+win8+wp8+wpa81;</AssetTargetFallback>
```
