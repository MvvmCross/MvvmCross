using System;

namespace MvvmCross.Core.ViewModels.Hints
{
    public class MvxPopPresentationHint
        : MvxPresentationHint
    {
        public MvxPopPresentationHint(Type viewModelToPopTo, bool animated = false)
        {
            ViewModelToPopTo = viewModelToPopTo;
            Animated = animated;
        }

        public Type ViewModelToPopTo { get; private set; }

        public bool Animated { get; set; }
    }
}
