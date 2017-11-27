using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Forms.Views.Hints
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
