//
// PickerElement.cs: Defines UIPickerView element
// Based on DateTimeElement by Miguel de Icaza
//
// Author:
//   Tomasz Cielecki (tomasz@ostebaronen.dk)
//
// Code licensed under the MIT X11 license
//

using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    public interface IPickerComponentRow
    {
        string DisplayName { get; }
    }

    public interface IPickerComponent
    {
        IList<IPickerComponentRow> ComponentRows { get; }
        IPickerComponentRow SelectedRow { get; set; }
        float ComponentWidth { get; set; }
        float RowHeight { get; set; }
    }

    public class PickerElementComponent : IPickerComponent
    {
        public IList<IPickerComponentRow> ComponentRows { get; set; }
        public IPickerComponentRow SelectedRow { get; set; }
        public float ComponentWidth { get; set; }
        public float RowHeight { get; set; }
    }

    public class PickerElementComponentRow : IPickerComponentRow
    {
        public string DisplayName { get; set; }
    }

    public class PickerElement : ValueElement<IList<IPickerComponent>>
    {
        private static readonly NSString Key = new NSString("PickerElement");

        private UIPickerView _picker;

        public PickerElement(string caption, IList<IPickerComponent> data) 
            : base (caption, data)
        {
        }	
		
        protected override UITableViewCell GetCellImpl (UITableView tv)
        {
            var cell = tv.DequeueReusableCell(Key) ?? new UITableViewCell(UITableViewCellStyle.Value1, Key)
                                                          {Accessory = UITableViewCellAccessory.DisclosureIndicator};

            UpdateDetailDisplay(cell);
            return cell;
        }
 
        protected override void Dispose (bool disposing)
        {
            base.Dispose (disposing);
            if (disposing)
            {
                if (_picker != null)
                {
                    _picker.Model.Dispose();
                    _picker.Model = null;
                    _picker.Dispose();
                    _picker = null;
                }
            }
        }

        public virtual UIPickerView CreatePicker()
        {
            var picker = new UIPickerView(RectangleF.Empty)
                            {
                                AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
                                Model = new PickerViewModel(Value),
                                ShowSelectionIndicator = true,
                            };
            return picker;
        }
		                                                                                                                                
        static RectangleF PickerFrameWithSize (SizeF size)
        {                                                                                                                                    
            var screenRect = UIScreen.MainScreen.ApplicationFrame;
            float fY = 0, fX = 0;
			
            switch (UIApplication.SharedApplication.StatusBarOrientation){
                case UIInterfaceOrientation.LandscapeLeft:
                case UIInterfaceOrientation.LandscapeRight:
                    fX = (screenRect.Height - size.Width) /2;
                    fY = (screenRect.Width - size.Height) / 2 -17;
                    break;
				
                case UIInterfaceOrientation.Portrait:
                case UIInterfaceOrientation.PortraitUpsideDown:
                    fX = (screenRect.Width - size.Width) / 2;
                    fY = (screenRect.Height - size.Height) / 2 - 25;
                    break;
            }
			
            return new RectangleF (fX, fY, size.Width, size.Height);
        }
        
        class PickerViewModel : UIPickerViewModel
        {
            private readonly IList<IPickerComponent> _components;
            public IList<IPickerComponent> Components { get { return _components; } }

            public PickerViewModel(IList<IPickerComponent> components)
            {
                _components = components;
            }

            public override int GetComponentCount(UIPickerView picker)
            {
                return null != _components ? _components.Count : 0;
            }

            public override int GetRowsInComponent(UIPickerView picker, int component)
            {
                return null != _components ? _components[component].ComponentRows.Count : 0;
            }

            public override string GetTitle(UIPickerView picker, int row, int component)
            {
                return null != _components ? _components[component].ComponentRows[row].DisplayName : null;
            }

            public override float GetComponentWidth(UIPickerView picker, int component)
            {
                if (null != _components)
                {
                    if (_components[component].ComponentWidth > 0)
                        return _components[component].ComponentWidth;

                    if (null != picker)
                        return picker.Frame.Width/_components.Count;
                }
                return base.GetComponentWidth(picker, component);
            }

            public override float GetRowHeight(UIPickerView picker, int component)
            {
                if (null != _components)
                    if (_components[component].RowHeight > 0)
                        return _components[component].RowHeight;
                return base.GetRowHeight(picker, component);
            }

            public override void Selected(UIPickerView picker, int row, int component)
            {
                if (null != _components)
                    _components[component].SelectedRow = _components[component].ComponentRows[row];
            }
        }                                                                                                   

        class PickerViewController : UIViewController 
        {
            readonly PickerElement _container;

            public PickerViewController(PickerElement container)
            {
                _container = container;
            }
			
            public override void ViewWillDisappear (bool animated)
            {
                base.ViewWillDisappear (animated);
                _container.OnUserValueChanged(((PickerViewModel)_container._picker.Model).Components);
            }
			
            public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
            {
                base.DidRotate (fromInterfaceOrientation);
                _container._picker.Frame = PickerFrameWithSize (_container._picker.SizeThatFits (SizeF.Empty));
            }
			
            public bool Autorotate { get; set; }
			
            public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
            {
                return Autorotate;
            }
        }
		
        public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            var vc = new PickerViewController(this)
                        {
                            Autorotate = dvc.Autorotate
                        };
            _picker = CreatePicker ();
            _picker.Frame = PickerFrameWithSize (_picker.SizeThatFits (SizeF.Empty));

            var components = ((PickerViewModel) _picker.Model).Components;
            for (var i = 0; i < components.Count; i++)
            {
                _picker.Select(components[i].ComponentRows.IndexOf(components[i].SelectedRow), i, true);
            }

            vc.View.BackgroundColor = UIColor.Black;
            vc.View.AddSubview (_picker);
            dvc.ActivateController (vc);
        }

        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            if (cell == null)
            {
                return;
            }

            if (cell.DetailTextLabel != null)
            {
                var sb = new StringBuilder();
                foreach (var component in Value)
                {
                    sb.AppendFormat("{0}", component.SelectedRow.DisplayName);
                }
                cell.DetailTextLabel.Text = sb.ToString();
            }
        }
    }
}