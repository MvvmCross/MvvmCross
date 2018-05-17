// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platforms.Tvos.Views;
using MvvmCross.Presenters;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Presenters
{
    public interface IMvxTvosViewPresenter
        : IMvxViewPresenter
        , IMvxCanCreateTvosView
    {
        IUIApplicationDelegate ApplicationDelegate { get; set; }

        UIWindow Window { get; set; }
    }
}
