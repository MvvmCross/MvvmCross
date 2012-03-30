#region Copyright
// <copyright file="MvxSingleton.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Exceptions;

namespace Cirrious.MvvmCross.Core
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

        private readonly static List<MvxSingleton> Singletons = new List<MvxSingleton>();

        protected MvxSingleton()
        {
            lock (Singletons)
            {
                Singletons.Add(this);
            }
        }

        public static void ClearAllSingletons()
        {
            lock(Singletons)
            {
                foreach (var s in Singletons)
                {
                    // note that linq is not used because of winrt!
                    // Singletons.ForEach(s => s.Dispose());
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
                Instance = null;
            }
        }
    }
}