// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Binding.Parse.Binding.Lang
{
    public interface IMvxLanguageBindingParser
        : IMvxBindingParser
    {
        string DefaultConverterName { get; set; }
        string DefaultTextSourceName { get; set; }
    }
}