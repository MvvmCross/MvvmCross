// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.Bindings;

namespace MvvmCross.Binding
{
    public class MvxBindingRequest
    {
        public MvxBindingRequest()
        {
        }

        public MvxBindingRequest(object source, object target, MvxBindingDescription description)
        {
            Target = target;
            Source = source;
            Description = description;
        }

        public object Target { get; set; }
        public object Source { get; set; }
        public MvxBindingDescription Description { get; set; }
    }
}
