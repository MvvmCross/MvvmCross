// MvxClosePresentationHint.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    public class MvxClosePresentationHint
        : MvxPresentationHint
    {
        public MvxClosePresentationHint(IMvxViewModel viewModelToClose)
        {
            ViewModelToClose = viewModelToClose;
        }

        public IMvxViewModel ViewModelToClose { get; private set; }
    }
}