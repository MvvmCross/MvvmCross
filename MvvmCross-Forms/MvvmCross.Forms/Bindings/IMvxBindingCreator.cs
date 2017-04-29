// IMvxBindingCreator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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