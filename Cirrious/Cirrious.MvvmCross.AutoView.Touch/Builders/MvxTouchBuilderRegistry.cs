#region Copyright

// <copyright file="MvxTouchBuilderRegistry.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Cirrious.MvvmCross.AutoView.Touch.Builders.Lists;
using Cirrious.MvvmCross.AutoView.Touch.Builders.Menus;
using CrossUI.Core.Elements.Lists;
using CrossUI.Core.Elements.Menu;
using CrossUI.Touch.Builder;

namespace Cirrious.MvvmCross.AutoView.Touch.Builders
{
    public class MvxTouchBuilderRegistry : TouchBuilderRegistry
    {
        public MvxTouchBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof (IListLayout), new MvxTouchListLayoutBuilder(registerDefaultElements));
            this.AddBuilder(typeof (IListItemLayout), new MvxTouchListItemLayoutBuilder(registerDefaultElements));
            this.AddBuilder(typeof (IMenu), new MvxTouchMenuBuilder(registerDefaultElements));
        }
    }
}