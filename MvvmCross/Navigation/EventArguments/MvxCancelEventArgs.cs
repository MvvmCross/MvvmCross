// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Threading;

namespace MvvmCross.Navigation.EventArguments
{
    public class MvxCancelEventArgs : CancelEventArgs
    {
        public MvxCancelEventArgs(CancellationToken cancellationToken = default)
        {
            CancellationToken = cancellationToken;
            if (CancellationToken != default)
                CancellationToken.Register(Canceled);
        }
        protected CancellationToken CancellationToken { get; }

        protected virtual void Canceled()
        {
            Cancel = true;
        }
    }
}
