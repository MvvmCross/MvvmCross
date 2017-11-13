// IMvxAxmlNameViewTypeResolver.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace MvvmCross.Binding.Droid.Binders.ViewTypeResolvers
{ 
    public interface IMvxAxmlNameViewTypeResolver
    {
        IDictionary<string, string> ViewNamespaceAbbreviations { get; }
    }
}