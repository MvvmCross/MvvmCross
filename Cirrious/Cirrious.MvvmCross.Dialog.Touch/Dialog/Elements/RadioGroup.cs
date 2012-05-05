namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    /// <summary>
    /// Captures the information about mutually exclusive elements in a RootElement
    /// </summary>
    public class RadioGroup : Group {
        int selected;
        public virtual int Selected {
            get { return selected; }
            set { selected = value; }
        }
		
        public RadioGroup (string key, int selected) : base (key)
        {
            this.selected = selected;
        }
		
        public RadioGroup (int selected) : base (null)
        {
            this.selected = selected;
        }
    }
}