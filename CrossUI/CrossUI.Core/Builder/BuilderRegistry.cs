// BuilderRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace CrossUI.Core.Builder
{
    public class BuilderRegistry : IBuilderRegistry
    {
        private readonly Dictionary<Type, TypedUserInterfaceBuilder> _builders =
            new Dictionary<Type, TypedUserInterfaceBuilder>();

        public void AddBuilder(Type interfaceType, TypedUserInterfaceBuilder builder)
        {
            _builders[interfaceType] = builder;
        }

        public bool TryGetValue(Type type, out TypedUserInterfaceBuilder typeBuilder)
        {
            return _builders.TryGetValue(type, out typeBuilder);
        }
    }
}