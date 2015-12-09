// IMvxApplicableTo.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Core
{
    public interface IMvxApplicableTo
    {
        void ApplyTo(object what);
    }

    public interface IMvxApplicableTo<in T>
    {
        void ApplyTo(T what);
    }
}