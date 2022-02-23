// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Uap.Views
{
    public interface IMvxWindowsViewModelLoader
    {
        IMvxViewModel Load(string requestText, IMvxBundle savedState);
    }
}
