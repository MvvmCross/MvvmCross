// BooleanElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Foundation;
using UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public class BooleanElement : ValueElement<bool>
    {
        private static readonly NSString Key = new NSString("BooleanElement");
        private UISwitch _switch;
        protected UISwitch Switch => _switch;

        public BooleanElement()
            : this("", false)
        {
        }

        public BooleanElement(string caption, bool value) : base(caption, value)
        {
        }

        public BooleanElement(string caption, bool value, string key) : base(caption, value)
        {
        }

        protected override NSString CellKey => Key;

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            if (_switch == null)
            {
                _switch = CreateSwitch();
                _switch.AddTarget(delegate { base.OnUserValueChanged(_switch.On); }, UIControlEvent.ValueChanged);
            }
            else
                _switch.On = Value;

            var cell = tv.DequeueReusableCell(CellKey);
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellKey)
                {
                    SelectionStyle = UITableViewCellSelectionStyle.None
                };
            }
            else
                cell.AccessoryView = null;

            var oldCell = _switch.Superview as UITableViewCell;
            if (oldCell != null)
                oldCell.AccessoryView = null;

            cell.TextLabel.Text = Caption;
            cell.AccessoryView = _switch;

            return cell;
        }

        protected virtual UISwitch CreateSwitch()
        {
            return new UISwitch
            {
                BackgroundColor = UIColor.Clear,
                Tag = 1,
                On = Value
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_switch != null)
                {
                    _switch.Dispose();
                    _switch = null;
                }
            }
        }

        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            if (_switch != null)
                _switch.On = Value;
        }
    }
}