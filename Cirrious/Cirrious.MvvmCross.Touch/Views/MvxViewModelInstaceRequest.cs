// MvxViewModelInstaceRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxViewModelInstaceRequest : MvxViewModelRequest
    {
        private readonly IMvxViewModel _viewModelInstance;

        public IMvxViewModel ViewModelInstance
        {
            get { return _viewModelInstance; }
        }

        public MvxViewModelInstaceRequest(IMvxViewModel viewModelInstance)
            : base(viewModelInstance.GetType(), null, null, MvxRequestedBy.Unknown)
        {
            _viewModelInstance = viewModelInstance;
        }
    }
}