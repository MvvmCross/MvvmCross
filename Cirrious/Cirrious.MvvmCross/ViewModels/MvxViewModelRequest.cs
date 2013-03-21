// MvxViewModelRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxViewModelRequest
    {
        public MvxViewModelRequest()
        {
        }

        public MvxViewModelRequest(Type viewModelType, IDictionary<string, string> parameterValues, bool clearTop,
                                       MvxRequestedBy requestedBy)
        {
            ViewModelType = viewModelType;
            ParameterValues = parameterValues;
            ClearTop = clearTop;
            RequestedBy = requestedBy;
        }

        public Type ViewModelType { get; set; }
        public IDictionary<string, string> ParameterValues { get; set; }
        public bool ClearTop { get; set; }
        public MvxRequestedBy RequestedBy { get; set; }

        public static MvxViewModelRequest GetDefaultRequest(Type viewModelType)
        {
            return new MvxViewModelRequest(viewModelType, null, false, MvxRequestedBy.Unknown);
        }
    }

    public class MvxViewModelRequest<TViewModel> : MvxViewModelRequest where TViewModel : IMvxViewModel
    {
        public MvxViewModelRequest(IDictionary<string, string> parameterValues, bool clearTop,
                                       MvxRequestedBy requestedBy)
            : base(typeof (TViewModel), parameterValues, clearTop, requestedBy)
        {
        }

        public static MvxViewModelRequest GetDefaultRequest()
        {
            return GetDefaultRequest(typeof (TViewModel));
        }
    }
}