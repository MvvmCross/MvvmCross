// IMvxAndroidViewsContainer.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;

namespace MvvmCross.Uwp.Views
{
    public interface IMvxWindowsViewModelLoader
    {
        IMvxViewModel Load(string requestText, IMvxBundle savedState);
    }
}
