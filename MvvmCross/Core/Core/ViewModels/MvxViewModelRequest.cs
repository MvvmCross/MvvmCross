// MvxViewModelRequest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Core.Platform;

    public class MvxViewModelRequest
    {
        public MvxViewModelRequest()
        {
        }

        public MvxViewModelRequest(Type viewModelType,
                                   IMvxBundle parameterBundle,
                                   IMvxBundle presentationBundle,
                                   MvxRequestedBy requestedBy)
        {
            this.ViewModelType = viewModelType;
            this.ParameterValues = parameterBundle.SafeGetData();
            this.PresentationValues = presentationBundle.SafeGetData();
            this.RequestedBy = requestedBy;
        }

        public Type ViewModelType { get; set; }
        public IDictionary<string, string> ParameterValues { get; set; }
        public IDictionary<string, string> PresentationValues { get; set; }
        public MvxRequestedBy RequestedBy { get; set; }

        public static MvxViewModelRequest GetDefaultRequest(Type viewModelType)
        {
            return new MvxViewModelRequest(viewModelType, null, null, MvxRequestedBy.Unknown);
        }
    }

    public class MvxViewModelRequest<TViewModel> : MvxViewModelRequest where TViewModel : IMvxViewModel
    {
        public MvxViewModelRequest(IMvxBundle parameterBundle, IMvxBundle presentationBundle,
                                   MvxRequestedBy requestedBy)
            : base(typeof(TViewModel), parameterBundle, presentationBundle, requestedBy)
        {
        }

        public static MvxViewModelRequest GetDefaultRequest()
        {
            return GetDefaultRequest(typeof(TViewModel));
        }
    }
}