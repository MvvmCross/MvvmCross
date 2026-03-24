// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Runtime;

namespace MvvmCross.Platforms.Android.Views;

/// <summary>
/// Base Android application class for MvvmCross.
/// Use <see cref="MvvmCross.Platforms.Android.Hosting.MvxAndroidHostBuilder"/> in your
/// <see cref="Application.OnCreate"/> override to initialize the framework.
/// </summary>
public abstract class MvxAndroidApplication : Application, IMvxAndroidApplication
{
    public static MvxAndroidApplication? Instance { get; private set; }

    protected MvxAndroidApplication()
    {
        Instance = this;
    }

    protected MvxAndroidApplication(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
        Instance = this;
    }
}

