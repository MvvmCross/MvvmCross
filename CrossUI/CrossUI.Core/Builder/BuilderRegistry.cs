using System;
using System.Collections.Generic;

namespace CrossUI.Core.Builder
{
    public class BuilderRegistry : IBuilderRegistry
    {
        private readonly Dictionary<Type, TypedUserInterfaceBuilder> _builders = new Dictionary<Type, TypedUserInterfaceBuilder>();

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