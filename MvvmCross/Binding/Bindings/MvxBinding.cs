// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Binding.Bindings
{
    public abstract class MvxBinding : IMvxBinding
    {
        ~MvxBinding()
        {
            Dispose(false);
        }

        #region IMvxBinding Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IMvxBinding Members

        protected virtual void Dispose(bool isDisposing)
        {
            // nothing to do in this base class
        }
    }
}
