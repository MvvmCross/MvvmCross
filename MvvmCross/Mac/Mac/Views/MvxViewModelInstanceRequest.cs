// MvxViewModelInstanceRequest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Mac.Views
{
    using global::MvvmCross.Core.ViewModels;

#warning Move to shared PCL code

    public class MvxViewModelInstanceRequest : MvxViewModelRequest
    {
        private readonly IMvxViewModel _viewModelInstance;

        public IMvxViewModel ViewModelInstance
        {
            get { return this._viewModelInstance; }
        }

        public MvxViewModelInstanceRequest(IMvxViewModel viewModelInstance)
            : base(viewModelInstance.GetType(), null, null, MvxRequestedBy.Unknown)
        {
            this._viewModelInstance = viewModelInstance;
        }
    }
}