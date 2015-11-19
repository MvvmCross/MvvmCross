// MvxPropertyNamePropertyToken.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens
{
    public class MvxPropertyNamePropertyToken : MvxPropertyToken
    {
        public MvxPropertyNamePropertyToken(string propertyText)
        {
            PropertyName = propertyText;
        }

        public string PropertyName { get; private set; }

        public override string ToString()
        {
            return "Property:" + PropertyName;
        }
    }
}