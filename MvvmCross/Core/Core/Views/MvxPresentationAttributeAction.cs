using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Core.Views
{
    public class MvxPresentationAttributeAction
    {
        public Action<Type, MvxBasePresentationAttribute, MvxViewModelRequest> ShowAction { get; set; }

        public Func<IMvxViewModel, MvxBasePresentationAttribute, bool> CloseAction { get; set; }
    }
}
