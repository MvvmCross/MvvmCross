// MvxViewModelInstanceRequest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;

namespace MvvmCross.tvOS.Views
{
#warning Move to shared PCL code

    public class MvxViewModelInstanceRequest : MvxViewModelRequest
    {
        public MvxViewModelInstanceRequest(IMvxViewModel viewModelInstance)
            : base(viewModelInstance.GetType(), null, null)
        {
            ViewModelInstance = viewModelInstance;
        }

        public IMvxViewModel ViewModelInstance { get; }
    }
}