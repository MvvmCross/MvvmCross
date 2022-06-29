// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using XFPage = Xamarin.Forms.Page;

namespace MvvmCross.Forms.Presenters
{
    public static class PageExtensions
    {
        public static void SetModalPagePresentationStyle(this XFPage page, MvxFormsModalPresentationStyle presentationStyle)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            page.On<iOS>().SetModalPresentationStyle((Xamarin.Forms.PlatformConfiguration.iOSSpecific.UIModalPresentationStyle)presentationStyle);
        }
    }
}
