namespace CrossUI.Droid.Dialog.Elements
{
    public class StyledMultilineElement : StringElement
    {
        public StyledMultilineElement() : base() { }
        public StyledMultilineElement(string caption) : base(caption) { }
        public StyledMultilineElement(string caption, string value) : base(caption, value) { }
        public StyledMultilineElement(string caption, string value, string layoutName) : base(caption, value, layoutName) { }
    }
}