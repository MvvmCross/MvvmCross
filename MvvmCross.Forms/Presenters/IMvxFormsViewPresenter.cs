// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Forms.Core;
using MvvmCross.Presenters;
using Xamarin.Forms;

namespace MvvmCross.Forms.Presenters
{
    public interface IMvxFormsViewPresenter : IMvxAttributeViewPresenter
    {
        Application FormsApplication { get; set; }
        IMvxFormsPagePresenter FormsPagePresenter { get; set; }

        bool ClosePlatformViews();
        bool ShowPlatformHost(Type hostViewModel = null);
    }
}
