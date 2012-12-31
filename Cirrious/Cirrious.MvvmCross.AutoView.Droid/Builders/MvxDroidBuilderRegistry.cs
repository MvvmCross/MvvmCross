#region Copyright

// <copyright file="MvxDroidBuilderRegistry.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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