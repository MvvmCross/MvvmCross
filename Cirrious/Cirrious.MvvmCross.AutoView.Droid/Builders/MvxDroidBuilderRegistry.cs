// MvxDroidBuilderRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.AutoView.Droid.Builders.Lists;
using CrossUI.Core.Elements.Lists;
using CrossUI.Droid.Builder;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders
{
    public class MvxDroidBuilderRegistry : DroidBuilderRegistry
    {
        public MvxDroidBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof (IListLayout), new MvxDroidListLayoutBuilder(registerDefaultElements));
            this.AddBuilder(typeof (IListItemLayout), new MvxDroidListItemLayoutBuilder(registerDefaultElements));
        }
    }
}