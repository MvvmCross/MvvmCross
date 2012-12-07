namespace CrossUI.Droid.Dialog.Elements
{
    public class MultilineElement : StringElement
    {
        public MultilineElement(string caption) : base(caption) { }
        public MultilineElement(string caption, string value) : base(caption, value) { }
        public MultilineElement(string caption, string value, string layoutName) : base(caption, value, layoutName) { }
    }
}