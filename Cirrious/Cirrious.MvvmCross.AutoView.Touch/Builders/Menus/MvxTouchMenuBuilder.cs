#region Copyright

// <copyright file="MvxTouchMenuBuilder.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using CrossUI.Core.Builder;
using CrossUI.Core.Elements.Menu;

namespace Cirrious.MvvmCross.AutoView.Touch.Builders.Menus
{
    public class MvxTouchMenuBuilder : TypedUserInterfaceBuilder
    {
        public MvxTouchMenuBuilder(bool registerDefaults)
            : base(typeof (IMenu), "Menu", "CaptionAndIcon")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}