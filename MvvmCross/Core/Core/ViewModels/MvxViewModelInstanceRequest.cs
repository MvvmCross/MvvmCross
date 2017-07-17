// MvxViewModelInstanceRequest.cs
//
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt

using System;

namespace MvvmCross.Core.ViewModels
{
    public class MvxViewModelInstanceRequest : MvxViewModelRequest
    {
        public IMvxViewModel ViewModelInstance { get; set; }

        public MvxViewModelInstanceRequest(Type viewModelType)
            : base(viewModelType)
        {
        }

        public MvxViewModelInstanceRequest(IMvxViewModel viewModelInstance)
            : base(viewModelInstance.GetType(), null, null)
        {
            ViewModelInstance = viewModelInstance;
        }
    }
}
