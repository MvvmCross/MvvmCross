// MvxIntegerIndexerPropertyToken.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.Bindings.Source.Construction.PropertyTokens
{
    public class MvxIntegerIndexerPropertyToken : MvxIndexerPropertyToken<uint>
    {
        public MvxIntegerIndexerPropertyToken(uint key)
            : base(key)
        {
        }
    }
}