// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Core.Platform;

namespace MvvmCross.Core.ViewModels
{
    public class MvxBundle
        : IMvxBundle
    {
        public MvxBundle()
            : this(new Dictionary<string, string>())
        {
        }

        public MvxBundle(IDictionary<string, string> data)
        {
            Data = data ?? new Dictionary<string, string>();
        }

        public IDictionary<string, string> Data { get; private set; }

        public void Write(object toStore)
        {
            Data.Write(toStore);
        }

        public T Read<T>()
            where T : new()
        {
            return Data.Read<T>();
        }

        public object Read(Type type)
        {
            return Data.Read(type);
        }

        public IEnumerable<object> CreateArgumentList(IEnumerable<ParameterInfo> requiredParameters, string debugText)
        {
            return Data.CreateArgumentList(requiredParameters, debugText);
        }
    }
}