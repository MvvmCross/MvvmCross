// MvxReflectionViewTypeResolver.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Views;
using MvvmCross.Platform.IoC;

namespace MvvmCross.Binding.Droid.Binders.ViewTypeResolvers
{
    public abstract class MvxReflectionViewTypeResolver : IMvxViewTypeResolver
    {
        protected IMvxTypeCache<View> TypeCache { get; }

        protected MvxReflectionViewTypeResolver(IMvxTypeCache<View> typeCache)
        {
            TypeCache = typeCache;
        }

        public abstract Type Resolve(string tagName);

        protected static bool IsFullyQualified(string tagName)
        {
            return tagName.Contains(".");
        }
    }
}