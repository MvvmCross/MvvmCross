// MvxBundle.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using MvvmCross.Core.Platform;

    public class MvxBundle
        : IMvxBundle
    {
        public MvxBundle()
            : this(new Dictionary<string, string>())
        {
        }

        public MvxBundle(IDictionary<string, string> data)
        {
            this.Data = data ?? new Dictionary<string, string>();
        }

        public IDictionary<string, string> Data { get; private set; }

        public void Write(object toStore)
        {
            this.Data.Write(toStore);
        }

        public T Read<T>()
            where T : new()
        {
            return this.Data.Read<T>();
        }

        public object Read(Type type)
        {
            return this.Data.Read(type);
        }

        public IEnumerable<object> CreateArgumentList(IEnumerable<ParameterInfo> requiredParameters, string debugText)
        {
            return this.Data.CreateArgumentList(requiredParameters, debugText);
        }
    }
}