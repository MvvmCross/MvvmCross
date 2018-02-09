// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxBundle
    {
        IDictionary<string, string> Data { get; }

        void Write(object toStore);

        T Read<T>() where T : new();

        object Read(Type type);

        IEnumerable<object> CreateArgumentList(IEnumerable<ParameterInfo> requiredParameters, string debugText);
    }
}