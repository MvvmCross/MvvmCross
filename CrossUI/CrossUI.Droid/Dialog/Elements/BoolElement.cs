namespace CrossUI.Droid.Dialog.Elements
{
    public abstract class BoolElement : ValueElement<bool>
    {
        public string TextOff { get; set; }
        public string TextOn { get; set; }

        protected BoolElement(string caption, bool value, string layoutName=null)
            : base(caption, value, layoutName)
        {
        }

        public override string Summary()
        {
            return Value ? TextOn : TextOff;
        }
    }
}