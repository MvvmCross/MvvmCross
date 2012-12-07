namespace CrossUI.Droid.Dialog.Elements
{
    public class MultilineEntryElement : EntryElement
    {
        public int MaxLength { get; set; }
        public MultilineEntryElement(string caption = null, string value = null, string layoutName = null)
            : base(caption, value, layoutName ?? "dialog_textfieldbelow")
        {
            Lines = 3;
        }

        public override void OnTextChanged(string newText)
        {
            if (MaxLength > 0 && newText.Length > MaxLength)
                Value = newText.Substring(0, MaxLength);
        }
    }
}