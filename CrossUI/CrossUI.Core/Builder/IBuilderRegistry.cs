// IBuilderRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace CrossUI.Core.Builder
{
    public interface IBuilderRegistry
    {
        void AddBuilder(Type interfaceType, TypedUserInterfaceBuilder builder);

        bool TryGetValue(Type type, out TypedUserInterfaceBuilder typeBuilder);
    }
}