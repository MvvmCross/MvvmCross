// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Exceptions;

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxSourceStepFactory : IMvxSourceStepFactoryRegistry
    {
        private readonly Dictionary<Type, IMvxSourceStepFactory> _subFactories =
            new Dictionary<Type, IMvxSourceStepFactory>();

        public void AddOrOverwrite(Type type, IMvxSourceStepFactory factory)
        {
            _subFactories[type] = factory;
        }

        public IMvxSourceStep Create(MvxSourceStepDescription description)
        {
            IMvxSourceStepFactory subFactory;
            if (!_subFactories.TryGetValue(description.GetType(), out subFactory))
            {
                throw new MvxException("Failed to get factory for step type {0}", description.GetType().Name);
            }

            return subFactory.Create(description);
        }
    }
}
