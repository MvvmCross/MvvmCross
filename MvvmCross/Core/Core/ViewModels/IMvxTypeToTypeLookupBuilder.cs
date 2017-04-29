// IMvxTypeToTypeLookupBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxTypeToTypeLookupBuilder
    {
        IDictionary<Type, Type> Build(IEnumerable<Assembly> sourceAssemblies);
    }
}