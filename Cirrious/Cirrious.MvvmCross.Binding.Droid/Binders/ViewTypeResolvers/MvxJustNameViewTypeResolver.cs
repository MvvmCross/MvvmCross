// MvxJustNameViewTypeResolver.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Views;
using Cirrious.CrossCore.IoC;

namespace Cirrious.MvvmCross.Binding.Droid.Binders.ViewTypeResolvers
{
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
            TypeCache.NameCache.TryGetValue(tagName, out toReturn);
            return toReturn;
        }
    }
}