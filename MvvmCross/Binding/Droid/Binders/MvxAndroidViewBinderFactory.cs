// MvxAndroidViewBinderFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Binders
{
    public class MvxAndroidViewBinderFactory
        : IMvxAndroidViewBinderFactory
    {
        public IMvxAndroidViewBinder Create(object source)
        {
            return new MvxAndroidViewBinder(source);
        }
    }
}