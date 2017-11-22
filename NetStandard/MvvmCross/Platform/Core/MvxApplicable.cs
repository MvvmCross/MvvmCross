// MvxApplicable.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Platform.Core
{
    public abstract class MvxApplicable
        : IMvxApplicable
    {
        private bool _finalizerSuppressed;

        ~MvxApplicable()
        {
            MvxSingleton<IMvxLog>.Instance.Trace("Finaliser called on {0} - suggests that  Apply() was never called", GetType().Name);
        }

        protected void SuppressFinalizer()
        {
            if (_finalizerSuppressed)
                return;

            _finalizerSuppressed = true;
            GC.SuppressFinalize(this);
        }

        public virtual void Apply()
        {
            SuppressFinalizer();
        }
    }
}