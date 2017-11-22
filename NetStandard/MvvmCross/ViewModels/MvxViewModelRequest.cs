// MvxViewModelRequest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using MvvmCross.Core.Platform;

namespace MvvmCross.Core.ViewModels
{
    public class MvxViewModelRequest
    {
        public MvxViewModelRequest()
        {
        }

        public MvxViewModelRequest(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public MvxViewModelRequest(Type viewModelType,
                                   IMvxBundle parameterBundle,
                                   IMvxBundle presentationBundle)
        {
            ViewModelType = viewModelType;
            ParameterValues = parameterBundle.SafeGetData();
            PresentationValues = presentationBundle.SafeGetData();
        }

        public Type ViewModelType { get; set; }
        public IDictionary<string, string> ParameterValues { get; set; }
        public IDictionary<string, string> PresentationValues { get; set; }

        public static MvxViewModelRequest GetDefaultRequest(Type viewModelType)
        {
            return new MvxViewModelRequest(viewModelType, null, null);
        }
    }

    public class MvxViewModelRequest<TViewModel> : MvxViewModelRequest where TViewModel : IMvxViewModel
    {
        public MvxViewModelRequest() : base(typeof(TViewModel))
        {
        }

        public MvxViewModelRequest(IMvxBundle parameterBundle, IMvxBundle presentationBundle)
            : base(typeof(TViewModel), parameterBundle, presentationBundle)
        {
        }

        public static MvxViewModelRequest GetDefaultRequest()
        {
            return GetDefaultRequest(typeof(TViewModel));
        }
    }
}