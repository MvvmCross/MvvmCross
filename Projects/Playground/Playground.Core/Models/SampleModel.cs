// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Playground.Core.Models;

public record SampleModel(string Message, decimal Value)
{
    public override string ToString() => $"Message: {Message}, Value: {Value}";
}
