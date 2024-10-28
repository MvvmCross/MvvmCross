// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

namespace MvvmCross.Platforms.Ios;

public class MvxIosTask
{
    protected Task<bool> DoUrlOpen(NSUrl url)
    {
        var sharedApp = UIApplication.SharedApplication;
        var options = new UIApplicationOpenUrlOptions { UniversalLinksOnly = false };
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        if (sharedApp.CanOpenUrl(url))
        {
            sharedApp.OpenUrl(url, options, ok => tcs.TrySetResult(ok));
        }
        else
        {
            tcs.TrySetResult(false);
        }

        return tcs.Task;
    }
}
