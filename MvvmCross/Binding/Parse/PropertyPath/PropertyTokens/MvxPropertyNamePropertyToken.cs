// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Binding.Parse.PropertyPath.PropertyTokens
{
    public class MvxPropertyNamePropertyToken : MvxPropertyToken
    {
        public MvxPropertyNamePropertyToken(string propertyText)
        {
            PropertyName = propertyText;
        }

        public string PropertyName { get; }

        public override string ToString()
        {
            return "Property:" + PropertyName;
        }
    }
}
