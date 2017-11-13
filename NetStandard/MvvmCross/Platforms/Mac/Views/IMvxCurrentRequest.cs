// IMvxCurrentRequest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
using MvvmCross.Core.ViewModels;

using MvvmCross.Core.ViewModels;

namespace MvvmCross.Mac.Views
{
    public interface IMvxCurrentRequest
    {
        MvxViewModelRequest CurrentRequest { get; }
    }
}