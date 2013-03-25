// MvxPresentationHint.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxPresentationHint
    {
        public MvxPresentationHint(MvxBundle body)
        {
            Body = body;
        }

        public MvxPresentationHint(IDictionary<string,string> hints)
            : this(new MvxBundle(hints))
        {
        }

        public MvxBundle Body { get; private set; }
    }
}