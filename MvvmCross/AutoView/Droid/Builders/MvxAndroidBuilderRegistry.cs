// MvxAndroidBuilderRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.Builders
{
    using CrossUI.Core.Elements.Lists;
    using CrossUI.Droid.Builder;

    using MvvmCross.AutoView.Droid.Builders.Lists;

    public class MvxAndroidBuilderRegistry : DroidBuilderRegistry
    {
        public MvxAndroidBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof(IListLayout), new MvxAndroidListLayoutBuilder(registerDefaultElements));
            this.AddBuilder(typeof(IListItemLayout), new MvxAndroidListItemLayoutBuilder(registerDefaultElements));
        }
    }
}