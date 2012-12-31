#region Copyright

// <copyright file="RadioElement.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public class RadioElement : StringElement
    {
        public string Group { get; set; }
        public int RadioIdx { get; set; }

        public RadioElement(string caption, string group) : base(caption)
        {
            Group = group;
        }

        public RadioElement(string caption = "") : base(caption)
        {
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = base.GetCellImpl(tv);
            SubscribeToRoot();
            return cell;
        }

        private bool _alreadySubscribed;

        private void SubscribeToRoot()
        {
            if (_alreadySubscribed)
            {
                return;
            }

            var root = (RootElement) Parent.Parent;

            if (!(root.Group is RadioGroup))
                throw new Exception("The RootElement's Group is null or is not a RadioGroup");

            root.RadioSelectedChanged += RootOnRadioSelectedChanged;
            _alreadySubscribed = true;
        }

        private void RootOnRadioSelectedChanged(object sender, EventArgs eventArgs)
        {
            var cell = GetActiveCell();
            UpdateCellDisplay(cell);
        }

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath indexPath)
        {
            var root = (RootElement) Parent.Parent;
            root.RadioSelected = RadioIdx;

            base.Selected(dvc, tableView, indexPath);
        }

        protected override void UpdateCellDisplay(UITableViewCell cell)
        {
            base.UpdateCellDisplay(cell);
            UpdateAccessoryDisplay(cell);
        }

        protected virtual void UpdateAccessoryDisplay(UITableViewCell cell)
        {
            if (cell == null)
                return;

            var root = (RootElement) Parent.Parent;
            var selected = RadioIdx == ((RadioGroup) (root.Group)).Selected;
            cell.Accessory = selected ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
        }
    }
}