using System;

namespace MvvmCross.Core.ViewModels.Hints
{
    public class MvxPopToRootPresentationHint
        : MvxPresentationHint
    {
        public MvxPopToRootPresentationHint(bool animated = true)
        {
            Animated = animated;
        }

        public bool Animated { get; set; }
    }
}
