// KeyedAuto.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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