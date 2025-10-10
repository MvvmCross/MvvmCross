// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    public abstract class MvxTypedSourceStepFactory<T>
        : IMvxSourceStepFactory
        where T : MvxSourceStepDescription
    {
        [RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        public IMvxSourceStep Create(MvxSourceStepDescription description)
        {
            return TypedCreate((T)description);
        }

        [RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        protected abstract IMvxSourceStep TypedCreate(T description);
    }
}
