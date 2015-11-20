// MvxAndroidListLayoutBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Builder;
using CrossUI.Core.Elements.Lists;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders.Lists
{
    public class MvxAndroidListLayoutBuilder : TypedUserInterfaceBuilder
    {
        public MvxAndroidListLayoutBuilder(bool registerDefaults)
            : base(typeof(IListLayout), "ListLayout", "General")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}