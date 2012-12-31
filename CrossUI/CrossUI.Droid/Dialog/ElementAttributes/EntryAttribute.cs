#region Copyright

// <copyright file="EntryAttribute.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;

namespace CrossUI.Droid.Dialog.ElementAttributes
{
#warning Attributes not really used in our fork of Dialog
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class EntryAttribute : Attribute
    {
        public string Placeholder;

        public EntryAttribute()
            : this(null)
        {
        }

        public EntryAttribute(string placeholder)
        {
            Placeholder = placeholder;
        }
    }
}