#region Copyright

// <copyright file="Group.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using CrossUI.Core.Elements.Dialog;

namespace CrossUI.Droid.Dialog.Elements
{
    /// <summary>
    /// Used by root elements to fetch information when they need to
    /// render a summary (Checkbox count or selected radio group).
    /// </summary>
    public class Group : IGroup
    {
        public string Key { get; set; }

        protected Group()
        {
        }

        public Group(string key)
        {
            Key = key;
        }
    }
}