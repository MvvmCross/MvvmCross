using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    public class RadioElement : StringElement {
        public string Group { get; set; }
        public int RadioIdx { get; set; }
		
        public RadioElement (string caption, string group) : base (caption)
        {
            Group = group;
        }
				
        public RadioElement (string caption) : base (caption)
        {
        }

        protected override UITableViewCell GetCellImpl (UITableView tv)
        {
            var cell = base.GetCellImpl (tv);			
            var root = (RootElement) Parent.Parent;
			
            if (!(root.Group is RadioGroup))
                throw new Exception ("The RootElement's Group is null or is not a RadioGroup");
			
            bool selected = RadioIdx == ((RadioGroup)(root.Group)).Selected;
            cell.Accessory = selected ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;

            return cell;
        }

        public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath indexPath)
        {
            RootElement root = (RootElement) Parent.Parent;
            if (RadioIdx != root.RadioSelected){
                var cell = tableView.CellAt (root.PathForRadio (root.RadioSelected));
                if (cell != null)
                    cell.Accessory = UITableViewCellAccessory.None;
                cell = tableView.CellAt (indexPath);
                if (cell != null)
                    cell.Accessory = UITableViewCellAccessory.Checkmark;
                root.RadioSelected = RadioIdx;
            }
			
            base.Selected (dvc, tableView, indexPath);
        }
    }
}