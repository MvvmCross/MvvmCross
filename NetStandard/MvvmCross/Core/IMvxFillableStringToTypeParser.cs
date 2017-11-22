// IMvxFillableStringToTypeParser.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace MvvmCross.Core.Platform
{
    public interface IMvxFillableStringToTypeParser
    {
        IDictionary<Type, MvxStringToTypeParser.IParser> TypeParsers { get; }
        IList<MvxStringToTypeParser.IExtraParser> ExtraParsers { get; }
    }
}