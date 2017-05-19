// MvxViewModelInstanceRequest.cs
//
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt

namespace MvvmCross.Core.ViewModels
{
    public class MvxViewModelInstanceRequest : MvxViewModelRequest
    {
        private readonly IMvxViewModel _viewModelInstance;

        public IMvxViewModel ViewModelInstance => _viewModelInstance;

        public MvxViewModelInstanceRequest(IMvxViewModel viewModelInstance)
            : base(viewModelInstance.GetType(), null, null)
        {
            _viewModelInstance = viewModelInstance;
        }
    }
}
