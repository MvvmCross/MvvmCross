// MvxBundle.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Platform;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cirrious.MvvmCross.ViewModels
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