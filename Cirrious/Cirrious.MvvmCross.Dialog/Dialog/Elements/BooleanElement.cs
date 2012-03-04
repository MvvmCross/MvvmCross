using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    public class BooleanElement : ValueElement<bool> 
    {
        static readonly NSString Key = new NSString ("BooleanElement");
        UISwitch _switch;
		
        public BooleanElement (string caption, bool value) : base (caption, value)
        {  }
		
        public BooleanElement (string caption, bool value, string key) : base (caption, value)
        {  }
		
        protected override NSString CellKey {
            get {
                return Key;
            }
        }

        protected override UITableViewCell GetCellImpl (UITableView tv)
        {
            if (_switch == null){
                _switch = new UISwitch (){
                                        BackgroundColor = UIColor.Clear,
                                        Tag = 1,
                                        On = Value
                                    };
                _switch.AddTarget (delegate {
                    base.OnUserValueChanged(_switch.On);
                }, UIControlEvent.ValueChanged);
            } else
                _switch.On = Value;
			
            var cell = tv.DequeueReusableCell (CellKey);
            if (cell == null){
                cell = new UITableViewCell (UITableViewCellStyle.Default, CellKey);
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            } else
                RemoveTag (cell, 1);
		
            cell.TextLabel.Text = Caption;
            cell.AccessoryView = _switch;

            return cell;
        }
		
        protected override void Dispose (bool disposing)
        {
            if (disposing){
                if (_switch != null){
                    _switch.Dispose ();
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