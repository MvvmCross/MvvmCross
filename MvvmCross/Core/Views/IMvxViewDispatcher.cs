﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base.Core;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Core.Views
{
    public interface IMvxViewDispatcher : IMvxMainThreadDispatcher
    {
        bool ShowViewModel(MvxViewModelRequest request);

        bool ChangePresentation(MvxPresentationHint hint);
    }
}