// MvxAndroidListItemLayoutBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.AutoView.Droid.Interfaces.Lists;
using CrossUI.Core.Builder;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders.Lists
{
    public class MvxAndroidListItemLayoutBuilder : TypedUserInterfaceBuilder
    {
        public MvxAndroidListItemLayoutBuilder(bool registerDefaults)
            : base(typeof(IMvxLayoutListItemViewFactory), "ListItemViewFactory", "General")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}