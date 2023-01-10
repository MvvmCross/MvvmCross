// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml.Controls;
using MvvmCross.Presenters.Attributes;

namespace MvvmCross.Platforms.WinUi.Presenters.Attributes
{
    public class MvxDialogViewPresentationAttribute : MvxBasePresentationAttribute
    {
        public MvxDialogViewPresentationAttribute()
        {
        }

        public ContentDialogPlacement Placement { get; set; }
    }
}
