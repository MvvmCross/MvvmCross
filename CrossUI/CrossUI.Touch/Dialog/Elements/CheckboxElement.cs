// CheckboxElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Foundation;
using UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public class CheckboxElement : ValueElement<bool>
    {
        private static readonly NSString Key = new NSString("CheckboxElement");

        public string Group { get; set; }

        public CheckboxElement() : this("")
        {
        }

        public CheckboxElement(string caption) : base(caption)
        {
        }

        public CheckboxElement(string caption, bool value) : base(caption)
        {
            Value = value;
        }

        public CheckboxElement(string caption, bool value, string group) : this(caption, value)
        {
            Group = group;
        }

        private void SetCellCheckmark(UITableViewCell cell)
        {
            if (cell != null)
            {
                cell.Accessory = Value ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
            }
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(Key) ?? new UITableViewCell(UITableViewCellStyle.Default, Key);
            SetCellCheckmark(cell);

            return cell;
        }

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            OnUserValueChanged(!Value);
            var cell = tableView.CellAt(path);
            UpdateDetailDisplay(cell);
            base.Selected(dvc, tableView, path);
        }

        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            SetCellCheckmark(cell);
        }
    }
}