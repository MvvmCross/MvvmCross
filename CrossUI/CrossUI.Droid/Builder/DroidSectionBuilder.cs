// DroidSectionBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Builder;
using CrossUI.Droid.Dialog.Elements;

namespace CrossUI.Droid.Builder
{
    public class DroidSectionBuilder : TypedUserInterfaceBuilder
    {
        public DroidSectionBuilder(bool registerDefaults)
            : base(typeof(Section), "Section", "")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}