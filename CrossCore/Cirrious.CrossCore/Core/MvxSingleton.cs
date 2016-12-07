// MvxSingleton.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Exceptions;

namespace Cirrious.CrossCore.Core
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

        private static readonly List<MvxSingleton> Singletons = new List<MvxSingleton>();

        protected MvxSingleton()
        {
            lock (Singletons)
            {
                Singletons.Add(this);
            }
        }

        public static void ClearAllSingletons()
        {
            lock (Singletons)
            {
                foreach (var s in Singletons)
                {
                    s.Dispose();
                }

                Singletons.Clear();
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
                IDisposable disposable = Instance as IDisposable;
                if (disposable != null)
                    disposable.Dispose();

                Instance = null;
            }
        }
    }
}