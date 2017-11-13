// IMvxViewDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Core;

namespace MvvmCross.Core.Views
{
    public interface IMvxViewDispatcher : IMvxMainThreadDispatcher
    {
        bool ShowViewModel(MvxViewModelRequest request);

        bool ChangePresentation(MvxPresentationHint hint);
    }
}