// MvxBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Binding.Bindings
{
    public abstract class MvxBinding : IMvxBinding
    {
        #region IMvxBinding Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IMvxBinding Members

        ~MvxBinding()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            // nothing to do in this base class
        }
    }
}