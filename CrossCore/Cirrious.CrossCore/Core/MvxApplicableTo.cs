// MvxApplicableTo.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.CrossCore.Core
{
    public abstract class MvxApplicableTo<T>
        : MvxApplicable,
          IMvxApplicableTo<T>
    {
        public virtual void ApplyTo(T what)
        {
            SuppressFinalizer();
        }
    }
}