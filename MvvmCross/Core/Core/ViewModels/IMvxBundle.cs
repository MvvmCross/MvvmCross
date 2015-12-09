// IMvxBundle.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cirrious.MvvmCross.ViewModels
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