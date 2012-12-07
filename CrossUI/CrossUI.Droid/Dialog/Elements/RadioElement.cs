using System;
using Android.Content;
using Android.Views;

namespace CrossUI.Droid.Dialog.Elements
{
    public class RadioElement : StringElement
    {
        public string Group { get; set; }
        internal int RadioIdx;

        public RadioElement(string caption = null, string group = null)
            : base(caption)
        {
            Group = group;
        }

        protected override View GetViewImpl(Context context, View convertView, ViewGroup parent)
        {
            if (!(((RootElement)Parent.Parent).Group is RadioGroup))
                throw new Exception("The RootElement's Group is null or is not a RadioGroup");

            return base.GetViewImpl(context, convertView, parent);
        }

        public override string Summary()
        {
            return Caption;
        }
    }
}