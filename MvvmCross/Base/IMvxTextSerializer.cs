// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.Base
{
    public interface IMvxTextSerializer
    {
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        T? DeserializeObject<T>(string inputText);

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        object? DeserializeObject(Type type, string inputText);

        string SerializeObject(object toSerialise);
    }
}
