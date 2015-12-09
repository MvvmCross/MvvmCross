// MvxBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings
{
    using System;

    public abstract class MvxBinding : IMvxBinding
    {
        ~MvxBinding()
        {
            this.Dispose(false);
        }

        #region IMvxBinding Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IMvxBinding Members

        protected virtual void Dispose(bool isDisposing)
        {
            // nothing to do in this base class
        }
    }
}