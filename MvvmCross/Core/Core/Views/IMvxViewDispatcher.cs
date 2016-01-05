﻿// IMvxViewDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.Views
{
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform.Core;

    public interface IMvxViewDispatcher : IMvxMainThreadDispatcher
    {
        bool ShowViewModel(MvxViewModelRequest request);

        bool ChangePresentation(MvxPresentationHint hint);
    }
}