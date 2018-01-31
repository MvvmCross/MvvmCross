// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Core.Navigation.EventArguments
{
    public class ChangePresentationEventArgs : EventArgs
    {
        public ChangePresentationEventArgs()
        {
        }

        public ChangePresentationEventArgs(MvxPresentationHint hint)
        {
            Hint = hint;
        }

        public MvxPresentationHint Hint { get; set; }

        public bool? Result { get; set; }
    }
}
