// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Runtime;
using MvvmCross.Core;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Android.Views;

public abstract class MvxAndroidApplication : Application, IMvxAndroidApplication
{
    public static MvxAndroidApplication Instance { get; private set; }

    protected MvxAndroidApplication()
    {
        Instance = this;
        RegisterSetup();
    }

    protected MvxAndroidApplication(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
        Instance = this;
        RegisterSetup();
    }

    protected abstract void RegisterSetup();

    public override void OnCreate()
    {
        base.OnCreate();

        MvxAndroidSetupSingleton.EnsureSingletonAvailable(this).EnsureInitialized();
    }

    protected virtual void RunAppStart()
    {
        if (Mvx.IoCProvider?.TryResolve(out IMvxAppStart startup) == true && !startup.IsStarted)
        {
            startup.Start();
        }
    }
}

public abstract class MvxAndroidApplication<TMvxAndroidSetup, TApplication> : MvxAndroidApplication
  where TMvxAndroidSetup : MvxAndroidSetup<TApplication>, new()
  where TApplication : class, IMvxApplication, new()
{
    protected MvxAndroidApplication() : base()
    {
    }

    protected MvxAndroidApplication(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    protected override void RegisterSetup()
    {
        this.RegisterSetupType<TMvxAndroidSetup>();
    }
}
