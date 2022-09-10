// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    public abstract class MvxTypedSourceStepFactory<T>
        : IMvxSourceStepFactory
        where T : MvxSourceStepDescription
    {
        public IMvxSourceStep Create(MvxSourceStepDescription description)
        {
            return TypedCreate((T)description);
        }

        protected abstract IMvxSourceStep TypedCreate(T description);
    }
}
