// MvxJustNameViewTypeResolver.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Binders.ViewTypeResolvers
{
    using System;

    using Android.Views;

    using MvvmCross.Platform.IoC;

    public class MvxJustNameViewTypeResolver : MvxReflectionViewTypeResolver
    {
        public MvxJustNameViewTypeResolver(IMvxTypeCache<View> typeCache) : base(typeCache)
        {
        }

        public override Type Resolve(string tagName)
        {
            // this resolver can't handle fully qualified tag names
            if (IsFullyQualified(tagName))
                return null;

            Type toReturn;
            this.TypeCache.NameCache.TryGetValue(tagName, out toReturn);
            return toReturn;
        }
    }
}