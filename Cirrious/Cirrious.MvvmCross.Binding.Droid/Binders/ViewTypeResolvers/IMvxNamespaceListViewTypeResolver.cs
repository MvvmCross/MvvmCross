// IMvxNamespaceListViewTypeResolver.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.Droid.Binders.ViewTypeResolvers
{
    public interface IMvxNamespaceListViewTypeResolver
    {
        void Add(string namespaceName);
    }
}