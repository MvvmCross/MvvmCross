// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using MvvmCross.Exceptions;

namespace MvvmCross.Base
{
    public abstract class MvxSingleton
        : IDisposable
    {
        ~MvxSingleton()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool isDisposing);

        private static readonly ConcurrentBag<MvxSingleton> _singletons = new ConcurrentBag<MvxSingleton>();

        protected MvxSingleton()
        {
            _singletons.Add(this);
        }

        public static void ClearAllSingletons()
        {
            while (_singletons.TryTake(out var s))
            {
                s.Dispose();
            }
        }
    }

    public abstract class MvxSingleton<TInterface>
        : MvxSingleton
        where TInterface : class
    {
        protected MvxSingleton()
        {
            if (Instance != null)
                throw new MvxException("You cannot create more than one instance of MvxSingleton");

            Instance = this as TInterface;
        }

        public static TInterface Instance { get; private set; }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                Instance = null;
            }
        }
    }
}
