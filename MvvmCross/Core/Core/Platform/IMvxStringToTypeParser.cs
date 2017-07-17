﻿// IMvxStringToTypeParser.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Core.Platform
{
    public interface IMvxStringToTypeParser
    {
        bool TypeSupported(Type targetType);

        object ReadValue(string rawValue, Type targetType, string fieldOrParameterName);
    }
}