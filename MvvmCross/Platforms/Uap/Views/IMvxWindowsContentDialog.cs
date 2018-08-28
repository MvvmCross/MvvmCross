// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Views;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace MvvmCross.Platforms.Uap.Views
{
    public interface IMvxWindowsContentDialog
        : IMvxView
    {
        IAsyncOperation<ContentDialogResult> ShowAsync(ContentDialogPlacement placement);
    }
}
