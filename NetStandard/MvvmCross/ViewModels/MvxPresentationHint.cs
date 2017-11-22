// MvxPresentationHint.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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