// MvxPropertyNamePropertyToken.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Parse.PropertyPath.PropertyTokens
{
    public class MvxPropertyNamePropertyToken : MvxPropertyToken
    {
        public MvxPropertyNamePropertyToken(string propertyText)
        {
            this.PropertyName = propertyText;
        }

        public string PropertyName { get; private set; }

        public override string ToString()
        {
            return "Property:" + this.PropertyName;
        }
    }
}