#region Copyright

// <copyright file="MvxTouchListLayoutBuilder.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using CrossUI.Core.Builder;
using CrossUI.Core.Elements.Lists;

namespace Cirrious.MvvmCross.AutoView.Touch.Builders.Lists
{
    public class MvxTouchListLayoutBuilder : TypedUserInterfaceBuilder
    {
        public MvxTouchListLayoutBuilder(bool registerDefaults)
            : base(typeof (IListLayout), "ListLayout", "General")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}