// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Binding.Bindings;

namespace MvvmCross.Forms.Bindings
{
    public interface IMvxBindingCreator
    {
        void CreateBindings(
            object sender,
            object oldValue,
            object newValue,
            Func<string, IEnumerable<MvxBindingDescription>> parseBindingDescriptions);
    }
}
