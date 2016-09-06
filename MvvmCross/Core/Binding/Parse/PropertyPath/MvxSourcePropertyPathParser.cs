// MvxSourcePropertyPathParser.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Collections.Concurrent;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace MvvmCross.Binding.Parse.PropertyPath
{
    /// <summary>
    /// Stateless parser with global caching of tokens
    /// </summary>
    public class MvxSourcePropertyPathParser : IMvxSourcePropertyPathParser
    {
        private static readonly ConcurrentDictionary<int, IList<MvxPropertyToken>> ParseCache = 
            new ConcurrentDictionary<int, IList<MvxPropertyToken>>();

        public IList<MvxPropertyToken> Parse(string textToParse)
        {
            textToParse = MvxPropertyPathParser.MakeSafe(textToParse);
            var hash = textToParse.GetHashCode();
            IList<MvxPropertyToken> list;
            if (ParseCache.TryGetValue(hash, out list))
                return list;

            var parser = new MvxPropertyPathParser();
            var currentTokens = parser.Parse(textToParse);

            ParseCache.TryAdd(hash, currentTokens);
            return currentTokens;
        }
    }
}
