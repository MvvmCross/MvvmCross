using System;

namespace CrossUI.Core.Builder
{
    public interface IBuilderRegistry
    {
        void AddBuilder(Type interfaceType, TypedUserInterfaceBuilder builder);
        bool TryGetValue(Type type, out TypedUserInterfaceBuilder typeBuilder);
    }
}