// MvxIndexerPropertyToken.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens
{
    public class MvxIndexerPropertyToken : MvxPropertyToken
    {
        protected MvxIndexerPropertyToken(object key)
        {
            Key = key;
        }

        public object Key { get; private set; }
    }

    public class MvxIndexerPropertyToken<T> : MvxIndexerPropertyToken
    {
        protected MvxIndexerPropertyToken(T key)
            : base(key)
        {
        }

        public new T Key
        {
            get { return (T) base.Key; }
        }
    }
}