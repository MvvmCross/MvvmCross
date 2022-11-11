// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace MvvmCross.ViewModels
{
#nullable enable
    public abstract class MvxPresentationHint
    {
        protected MvxPresentationHint(MvxBundle? body = default)
        {
            Body = body;
        }

        protected MvxPresentationHint(IDictionary<string, string> hints)
            : this(new MvxBundle(hints))
        {
        }

        public MvxBundle? Body { get; }
    }
#nullable restore
}
