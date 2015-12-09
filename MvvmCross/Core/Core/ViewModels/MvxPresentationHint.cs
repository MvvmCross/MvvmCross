// MvxPresentationHint.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System.Collections.Generic;

    public abstract class MvxPresentationHint
    {
        protected MvxPresentationHint()
            : this(new MvxBundle())
        {
        }

        protected MvxPresentationHint(MvxBundle body)
        {
            this.Body = body;
        }

        protected MvxPresentationHint(IDictionary<string, string> hints)
            : this(new MvxBundle(hints))
        {
        }

        public MvxBundle Body { get; private set; }
    }
}