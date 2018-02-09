// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.ViewModels
{
    public class MvxViewModelInstanceRequest : MvxViewModelRequest
    {
        public IMvxViewModel ViewModelInstance { get; set; }

        public MvxViewModelInstanceRequest(Type viewModelType)
            : base(viewModelType)
        {
        }

        public MvxViewModelInstanceRequest(IMvxViewModel viewModelInstance)
            : base(viewModelInstance.GetType(), null, null)
        {
            ViewModelInstance = viewModelInstance;
        }
    }
}
