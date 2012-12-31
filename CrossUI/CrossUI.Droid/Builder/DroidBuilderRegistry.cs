#region Copyright

// <copyright file="DroidBuilderRegistry.cs" company="Cirrious">
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
using CrossUI.Droid.Dialog.Elements;

namespace CrossUI.Droid.Builder
{
    public class DroidBuilderRegistry : BuilderRegistry
    {
        public DroidBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof (Element), new DroidElementBuilder(registerDefaultElements));
            this.AddBuilder(typeof (Group), new DroidGroupBuilder(registerDefaultElements));
            this.AddBuilder(typeof (Section), new DroidSectionBuilder(registerDefaultElements));
            this.AddBuilder(typeof (IMenu), new DroidMenuBuilder(registerDefaultElements));
        }
    }
}