#region Copyright

// <copyright file="BuilderRegistry.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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