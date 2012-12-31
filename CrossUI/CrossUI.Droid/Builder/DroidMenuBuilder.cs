#region Copyright

// <copyright file="DroidMenuBuilder.cs" company="Cirrious">
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

namespace CrossUI.Droid.Builder
{
    public class DroidMenuBuilder : TypedUserInterfaceBuilder
    {
        public DroidMenuBuilder(bool registerDefaults)
            : base(typeof (IMenu), "Menu", "CaptionAndIcon")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}