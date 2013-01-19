// IMvxSourcePropertyTokeniser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction.PropertyTokens;

namespace Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source.Construction
{
    public interface IMvxSourcePropertyTokeniser
    {
        IList<MvxBasePropertyToken> Tokenise(string textToTokenise);
    }
}