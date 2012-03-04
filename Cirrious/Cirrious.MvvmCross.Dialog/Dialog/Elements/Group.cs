namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    /// <summary>
    /// Used by root elements to fetch information when they need to
    /// render a summary (Checkbox count or selected radio group).
    /// </summary>
    public class Group 
    {
        public string Key { get; set; }

        public Group (string key)
        {
            Key = key;
        }
    }
}