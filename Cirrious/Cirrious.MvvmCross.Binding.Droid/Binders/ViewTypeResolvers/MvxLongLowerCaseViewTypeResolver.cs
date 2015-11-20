// MvxLongLowerCaseViewTypeResolver.cs
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
    public abstract class MvxLongLowerCaseViewTypeResolver : MvxReflectionViewTypeResolver
    {
        protected MvxLongLowerCaseViewTypeResolver(IMvxTypeCache<View> typeCache)
            : base(typeCache)
        {
        }

        protected Type ResolveLowerCaseTypeName(string longLowerCaseName)
        {
            Type toReturn;
            TypeCache.LowerCaseFullNameCache.TryGetValue(longLowerCaseName, out toReturn);
            return toReturn;
        }
    }
}