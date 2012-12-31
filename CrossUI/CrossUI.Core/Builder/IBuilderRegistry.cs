#region Copyright

// <copyright file="IBuilderRegistry.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;

namespace CrossUI.Core.Builder
{
    public interface IBuilderRegistry
    {
        void AddBuilder(Type interfaceType, TypedUserInterfaceBuilder builder);
        bool TryGetValue(Type type, out TypedUserInterfaceBuilder typeBuilder);
    }
}