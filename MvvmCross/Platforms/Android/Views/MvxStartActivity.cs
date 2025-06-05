// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Android.Views;

[Register("mvvmcross.platforms.android.views.MvxStartActivity")]
public abstract class MvxStartActivity
    : MvxActivity<MvxStartViewModel>
{
    protected const int NoContent = 0;

    private readonly int _resourceId;

    private Bundle _bundle;

    public virtual bool SingleHostActivity => false;

    protected MvxStartActivity(int resourceId = NoContent)
    {
        RegisterSetup();
        _resourceId = resourceId;
    }

    protected MvxStartActivity(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    protected virtual void RequestWindowFeatures()
    {
        if (!SingleHostActivity)
            RequestWindowFeature(WindowFeatures.NoTitle);
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        RequestWindowFeatures();

        _bundle = savedInstanceState;

        base.OnCreate(savedInstanceState);

        if (_resourceId != NoContent)
        {
            var content = this.BindingInflate(_resourceId, null);
            SetContentView(content);
        }
    }

#pragma warning disable AsyncFixer01, AsyncFixer03
    protected override async void OnResume()
    {
        base.OnResume();
        await RunAppStartAsync(_bundle);
    }
#pragma warning restore AsyncFixer01, AsyncFixer03

    protected virtual async Task RunAppStartAsync(Bundle bundle)
    {
        if (Mvx.IoCProvider?.TryResolve(out IMvxAppStart startup) == true)
        {
            if (!startup.IsStarted)
            {
                await startup.StartAsync(GetAppStartHint(bundle));
            }
            else if (!SingleHostActivity)
            {
                Finish();
            }
        }
    }

    protected virtual object GetAppStartHint(object hint = null)
    {
        return hint;
    }

    protected virtual void RegisterSetup()
    {
    }
}

public class MvxStartViewModel : MvxNullViewModel {}
