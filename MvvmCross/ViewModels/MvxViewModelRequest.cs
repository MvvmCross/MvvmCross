// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Core;

namespace MvvmCross.ViewModels
{
#nullable enable
    public class MvxViewModelRequest
    {
        public MvxViewModelRequest()
        {
        }

        public MvxViewModelRequest([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public MvxViewModelRequest([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
                                   IMvxBundle? parameterBundle,
                                   IMvxBundle? presentationBundle)
        {
            ViewModelType = viewModelType;
            ParameterValues = parameterBundle.SafeGetData();
            PresentationValues = presentationBundle.SafeGetData();
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)]
        public Type? ViewModelType { get; set; }
        public IDictionary<string, string>? ParameterValues { get; set; }
        public IDictionary<string, string>? PresentationValues { get; set; }

        public static MvxViewModelRequest GetDefaultRequest([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType)
        {
            return new MvxViewModelRequest(viewModelType, null, null);
        }
    }

    public class MvxViewModelRequest<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel> : MvxViewModelRequest where TViewModel : IMvxViewModel
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
#nullable restore
}
