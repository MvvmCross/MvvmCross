// IMvxViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Core.Views
{
    public interface IMvxViewPresenter
    {
        void Show(MvxViewModelRequest request);

        void ChangePresentation(MvxPresentationHint hint);

        void AddPresentationHintHandler<THint>(Func<THint, bool> action) where THint : MvxPresentationHint;

        void Close(IMvxViewModel toClose);
    }
}