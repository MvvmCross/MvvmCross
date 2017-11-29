using System;

namespace MvvmCross.Core.ViewModels.Hints
{
    public class MvxRemovePresentationHint
        : MvxPresentationHint
    {
        public MvxRemovePresentationHint(Type viewModelToRemove)
        {
            ViewModelToRemove = viewModelToRemove;
        }

        public Type ViewModelToRemove { get; private set; }
    }
}
