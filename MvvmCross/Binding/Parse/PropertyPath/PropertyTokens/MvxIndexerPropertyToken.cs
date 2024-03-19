// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

namespace MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

public record MvxIndexerPropertyToken(object? Key) : IMvxPropertyToken
{
    public override string ToString()
    {
        return "IndexedProperty:" + (Key == null ? "null" : Key.ToString());
    }
}

public record MvxIndexerPropertyToken<T>(T? Key) : MvxIndexerPropertyToken(Key)
{
    public new T? Key => (T?)base.Key;
}
