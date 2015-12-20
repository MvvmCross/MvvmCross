// MvxApplicableTo.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Core
{
    public abstract class MvxApplicableTo<T>
        : MvxApplicable,
          IMvxApplicableTo<T>
    {
        public virtual void ApplyTo(T what)
        {
            this.SuppressFinalizer();
        }
    }
}