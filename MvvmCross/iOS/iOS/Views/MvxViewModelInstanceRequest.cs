// MvxViewModelInstanceRequest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.iOS.Views
{
    using MvvmCross.Core.ViewModels;

#warning Move to shared PCL code

    public class MvxViewModelInstanceRequest : MvxViewModelRequest
    {
        private readonly IMvxViewModel _viewModelInstance;

        public IMvxViewModel ViewModelInstance => this._viewModelInstance;

        public MvxViewModelInstanceRequest(IMvxViewModel viewModelInstance)
            : base(viewModelInstance.GetType(), null, null)
        {
            this._viewModelInstance = viewModelInstance;
        }
    }
}