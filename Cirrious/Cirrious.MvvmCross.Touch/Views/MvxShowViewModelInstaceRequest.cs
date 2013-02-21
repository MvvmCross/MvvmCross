// MvxShowViewModelInstaceRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxShowViewModelInstaceRequest : MvxShowViewModelRequest
    {
        private readonly IMvxViewModel _viewModelInstance;

        public IMvxViewModel ViewModelInstance { get { return _viewModelInstance; } }

        public MvxShowViewModelInstaceRequest(IMvxViewModel viewModelInstance)
            : base(viewModelInstance.GetType(), null, false, MvxRequestedBy.Unknown)
        {
            _viewModelInstance = viewModelInstance;
        }
    }
}