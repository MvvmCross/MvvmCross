// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace MvvmCross.Core.ViewModels
{
    public abstract class MvxPresentationHint
    {
        protected MvxPresentationHint()
            : this(new MvxBundle())
        {
        }

        protected MvxPresentationHint(MvxBundle body)
        {
            Body = body;
        }

        protected MvxPresentationHint(IDictionary<string, string> hints)
            : this(new MvxBundle(hints))
        {
        }

        public MvxBundle Body { get; private set; }
    }
}