// IMvxChildViewModelCache.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Views
{
    using MvvmCross.Core.ViewModels;

    public interface IMvxChildViewModelCache
    {
        int Cache(IMvxViewModel viewModel);

        IMvxViewModel Get(int index);

        void Remove(int index);
    }
}