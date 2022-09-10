// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.IO;

namespace MvvmCross.Base
{
#nullable enable
    public interface IMvxJsonConverter : IMvxTextSerializer
    {
        T DeserializeObject<T>(Stream stream);
    }
#nullable restore
}
