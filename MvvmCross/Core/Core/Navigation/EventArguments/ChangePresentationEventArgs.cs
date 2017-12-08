using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Core.Navigation.EventArguments
{
    public class ChangePresentationEventArgs : EventArgs
    {
        public ChangePresentationEventArgs()
        {
        }

        public ChangePresentationEventArgs(MvxPresentationHint hint)
        {
            Hint = hint;
        }

        public MvxPresentationHint Hint { get; set; }

        public bool? Result { get; set; }
    }
}
