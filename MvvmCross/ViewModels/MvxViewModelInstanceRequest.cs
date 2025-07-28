// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.ViewModels
{
    public class MvxViewModelInstanceRequest(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType)
            : MvxViewModelRequest(viewModelType)
    {
        public MvxViewModelInstanceRequest(IMvxViewModel viewModelInstance)
            : this(viewModelInstance.GetType())
        {
            ViewModelInstance = viewModelInstance;
        }

        public IMvxViewModel? ViewModelInstance { get; set; }
    }
}
