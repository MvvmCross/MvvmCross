using Foobar.Dialog.Core.Descriptions;

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