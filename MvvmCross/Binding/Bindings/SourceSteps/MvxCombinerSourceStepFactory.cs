// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxCombinerSourceStepFactory : MvxTypedSourceStepFactory<MvxCombinerSourceStepDescription>
    {
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        protected override IMvxSourceStep TypedCreate(MvxCombinerSourceStepDescription description)
        {
            var toReturn = new MvxCombinerSourceStep(description);
            return toReturn;
        }
    }
}
