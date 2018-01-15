// IMvxAndroidViewsContainer.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;

namespace MvvmCross.Uwp.Views
{
    public interface IMvxWindowsViewModelRequestTranslator
    {
        string GetRequestTextFor(MvxViewModelRequest request);

        // Important: if calling GetRequestTextWithKeyFor then you must later call RemoveSubViewModelWithKey on the returned key
        string GetRequestTextWithKeyFor(IMvxViewModel existingViewModelToUse);

        void RemoveSubViewModelWithKey(int key);

        int RequestTextGetKey(string requestText);
    }
}
