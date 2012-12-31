#region Copyright

// <copyright file="KeyedAuto.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using CrossUI.Core.Descriptions;

namespace Cirrious.MvvmCross.AutoView.Auto
{
    public abstract class KeyedAuto : BaseAuto
    {
        public string Key { get; set; }

        protected KeyedAuto(string key, string onlyFor = null, string notFor = null)
        {
            Key = key;
        }

        public abstract KeyedDescription ToDescription();

        public void Fill(KeyedDescription keyedDescription)
        {
            base.Fill(keyedDescription);
            keyedDescription.Key = Key;
        }
    }
}