// IMvxSourcePropertyPathParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace Cirrious.MvvmCross.Binding.Parse.PropertyPath
{
    public interface IMvxSourcePropertyPathParser
    {
        IList<MvxPropertyToken> Parse(string textToParse);
    }
}