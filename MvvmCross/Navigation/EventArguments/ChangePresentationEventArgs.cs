// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using MvvmCross.ViewModels;

namespace MvvmCross.Navigation.EventArguments
{
    public class ChangePresentationEventArgs : MvxCancelEventArgs
    {
        public ChangePresentationEventArgs(CancellationToken cancellationToken = default) : base(cancellationToken)
        {
        }

        public ChangePresentationEventArgs(MvxPresentationHint hint, CancellationToken cancellationToken = default) : this(cancellationToken)
        {
            Hint = hint;
        }

        public MvxPresentationHint Hint { get; set; }

        public bool? Result { get; set; }
    }
}
