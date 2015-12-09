// MvxSourceStepFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Platform.Exceptions;

    public class MvxSourceStepFactory : IMvxSourceStepFactoryRegistry
    {
        private readonly Dictionary<Type, IMvxSourceStepFactory> _subFactories =
            new Dictionary<Type, IMvxSourceStepFactory>();

        public void AddOrOverwrite(Type type, IMvxSourceStepFactory factory)
        {
            this._subFactories[type] = factory;
        }

        public IMvxSourceStep Create(MvxSourceStepDescription description)
        {
            IMvxSourceStepFactory subFactory;
            if (!this._subFactories.TryGetValue(description.GetType(), out subFactory))
            {
                throw new MvxException("Failed to get factory for step type {0}", description.GetType().Name);
            }

            return subFactory.Create(description);
        }
    }
}