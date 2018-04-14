// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters
{
    public interface IMvxViewPresenter
    {
        void Show(MvxViewModelRequest request);

        void ChangePresentation(MvxPresentationHint hint);

        void AddPresentationHintHandler<THint>(Func<THint, bool> action) where THint : MvxPresentationHint;

        void Close(IMvxViewModel toClose);
    }
}
