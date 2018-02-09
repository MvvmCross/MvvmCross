// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform.Tvos.Views;
using MvvmCross.Views;

namespace MvvmCross.Platform.Tvos.Presenters
{
    public interface IMvxTvosViewPresenter
        : IMvxViewPresenter
        , IMvxCanCreateTvosView
    {
    }
}
