#region Copyright

// <copyright file="MvxTouchListItemLayoutBuilder.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Cirrious.MvvmCross.AutoView.Touch.Interfaces.Lists;
using CrossUI.Core.Builder;

namespace Cirrious.MvvmCross.AutoView.Touch.Builders.Lists
{
    public class MvxTouchListItemLayoutBuilder : TypedUserInterfaceBuilder
    {
        public MvxTouchListItemLayoutBuilder(bool registerDefaults)
            : base(typeof (IMvxLayoutListItemViewFactory), "ListItemViewFactory", "General")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}