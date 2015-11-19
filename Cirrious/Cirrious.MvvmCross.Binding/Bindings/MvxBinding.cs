// MvxBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Binding.Bindings
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