// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

namespace MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

public record MvxPropertyNamePropertyToken(string PropertyName) : IMvxPropertyToken
{
    public override string ToString() => "Property:" + PropertyName;
}
