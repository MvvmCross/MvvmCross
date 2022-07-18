// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxPathSourceStepDescription : MvxSourceStepDescription
    {
        public string SourcePropertyPath { get; set; }

        public override string ToString()
        {
            return SourcePropertyPath ?? "-empty-";
        }
    }
}
