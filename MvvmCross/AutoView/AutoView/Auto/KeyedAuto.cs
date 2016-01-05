// KeyedAuto.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Auto
{
    using CrossUI.Core.Descriptions;

    public abstract class KeyedAuto : BaseAuto
    {
        public string Key { get; set; }

        protected KeyedAuto(string key, string onlyFor = null, string notFor = null)
        {
            this.Key = key;
        }

        public abstract KeyedDescription ToDescription();

        public void Fill(KeyedDescription keyedDescription)
        {
            base.Fill(keyedDescription);
            keyedDescription.Key = this.Key;
        }
    }
}