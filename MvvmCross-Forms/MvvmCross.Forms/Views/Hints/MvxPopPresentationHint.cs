using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Forms.Views.Hints
{
    public class MvxPopPresentationHint
        : MvxPresentationHint
    {
        public MvxPopPresentationHint(Type viewToPopTo, bool animated = false)
        {
            ViewToPopTo = viewToPopTo;
            Animated = animated;
        }

        public Type ViewToPopTo { get; private set; }

        public bool Animated { get; set; }
    }
}
