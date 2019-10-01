// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Binding.Parse.PropertyPath.PropertyTokens
{
    public class MvxIndexerPropertyToken : MvxPropertyToken
    {
        protected MvxIndexerPropertyToken(object key)
        {
            Key = key;
        }

        public object Key { get; }

        public override string ToString()
        {
            return "IndexedProperty:" + (Key == null ? "null" : Key.ToString());
        }
    }

    public class MvxIndexerPropertyToken<T> : MvxIndexerPropertyToken
    {
        protected MvxIndexerPropertyToken(T key)
            : base(key)
        {
        }

        public new T Key => (T)base.Key;
    }
}