// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Binding.Bindings.SourceSteps;

namespace MvvmCross.Binding.Combiners
{
    public interface IMvxValueCombiner
    {
        Type SourceType(IEnumerable<IMvxSourceStep> steps);

        void SetValue(IEnumerable<IMvxSourceStep> steps, object value);

        bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value);

        IEnumerable<Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps, Type overallTargetType);
    }
}
