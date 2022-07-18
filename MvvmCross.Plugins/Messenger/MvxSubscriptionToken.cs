// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Plugin.Messenger
{
#nullable enable
    [Preserve(AllMembers = true)]
    public sealed class MvxSubscriptionToken
        : IDisposable
    {
        public Guid Id { get; private set; }
#pragma warning disable 414 // 414 is that this private field is only set, not used
        private readonly object[] _dependentObjects;
#pragma warning restore 414
        private readonly Action _disposeMe;

        public MvxSubscriptionToken(Guid id, Action disposeMe, params object[] dependentObjects)
        {
            Id = id;
            _disposeMe = disposeMe;
            _dependentObjects = dependentObjects;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _disposeMe();
            }
        }
    }
#nullable restore
}
