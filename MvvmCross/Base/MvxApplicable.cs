// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;

namespace MvvmCross.Base
{
#nullable enable
    public abstract class MvxApplicable
        : IMvxApplicable
    {
        private bool _finalizerSuppressed;

        ~MvxApplicable()
        {
            MvxLogHost.Default?.Log(LogLevel.Trace, "Finaliser called on {0} - suggests that  Apply() was never called", GetType().Name);
        }

        protected void SuppressFinalizer()
        {
            if (_finalizerSuppressed)
                return;

            _finalizerSuppressed = true;
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
#pragma warning disable S3971 // "GC.SuppressFinalize" should not be called
            GC.SuppressFinalize(this);
#pragma warning restore S3971 // "GC.SuppressFinalize" should not be called
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        }

        public virtual void Apply()
        {
            SuppressFinalizer();
        }
    }
#nullable restore
}
