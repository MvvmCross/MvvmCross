// MvxReflectionViewTypeResolver.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using Cirrious.CrossCore.IoC;
using System;

namespace Cirrious.MvvmCross.Binding.Droid.Binders.ViewTypeResolvers
{
    public abstract class MvxReflectionViewTypeResolver : IMvxViewTypeResolver
    {
        private readonly IMvxTypeCache<View> _typeCache;
        protected IMvxTypeCache<View> TypeCache => _typeCache;

        protected MvxReflectionViewTypeResolver(IMvxTypeCache<View> typeCache)
        {
            _typeCache = typeCache;
        }

        public abstract Type Resolve(string tagName);

        protected static bool IsFullyQualified(string tagName)
        {
            return tagName.Contains(".");
        }
    }
}