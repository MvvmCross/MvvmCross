// MvxViewModelRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Platform;
using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.ViewModels
{
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
            ViewModelType = viewModelType;
            ParameterValues = parameterBundle.SafeGetData();
            PresentationValues = presentationBundle.SafeGetData();
            RequestedBy = requestedBy;
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