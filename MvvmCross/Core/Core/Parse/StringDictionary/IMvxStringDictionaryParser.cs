// IMvxStringDictionaryParser.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace MvvmCross.Core.Parse.StringDictionary
{
    public interface IMvxStringDictionaryParser
    {
        IDictionary<string, string> Parse(string textToParse);
    }
}