// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Views;
using MvvmCross.IoC;

namespace MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers
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
