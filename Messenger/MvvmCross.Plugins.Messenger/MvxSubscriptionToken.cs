// MvxSubscriptionToken.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Plugins.Messenger
{
    public sealed class MvxSubscriptionToken
        : IDisposable
    {
        public Guid Id { get; private set; }
#pragma warning disable 414 // 414 is that this private field is only set, not used
        private readonly object[] _dependentObjects;
#pragma warning restore 414
        private readonly Action _disposeMe;

        public MvxSubscriptionToken(Guid id, Action disposeMe,  params object[] dependentObjects)
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
}