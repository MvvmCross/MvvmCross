// IMvxTypeCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cirrious.CrossCore.IoC
{
    public interface IMvxTypeCache<TType>
    {
        Dictionary<string, Type> LowerCaseFullNameCache { get; }
        Dictionary<string, Type> FullNameCache { get; }
        Dictionary<string, Type> NameCache { get; }

        void AddAssembly(Assembly assembly);
    }
}