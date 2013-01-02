// MvxShowViewModelRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Views
{
    public class MvxShowViewModelRequest
    {
        public MvxShowViewModelRequest()
        {
        }

        public MvxShowViewModelRequest(Type viewModelType, IDictionary<string, string> parameterValues, bool clearTop,
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

        public static MvxShowViewModelRequest GetDefaultRequest(Type viewModelType)
        {
            return new MvxShowViewModelRequest(viewModelType, null, false, MvxRequestedBy.Unknown);
        }
    }

    public class MvxShowViewModelRequest<TViewModel> : MvxShowViewModelRequest where TViewModel : IMvxViewModel
    {
        public MvxShowViewModelRequest(IDictionary<string, string> parameterValues, bool clearTop,
                                       MvxRequestedBy requestedBy)
            : base(typeof (TViewModel), parameterValues, clearTop, requestedBy)
        {
        }

        public static MvxShowViewModelRequest GetDefaultRequest()
        {
            return GetDefaultRequest(typeof (TViewModel));
        }
    }
}