using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    /// <summary>
    ///   This element can be used to insert an arbitrary UIView
    /// </summary>
    /// <remarks>
    ///   There is no cell reuse here as we have a 1:1 mapping
    ///   in this case from the UIViewElement to the cell that
    ///   holds our view.
    /// </remarks>
    public class UIViewElement : Element, IElementSizing {
        static int _count;
        readonly NSString key;
        protected UIView View { get; set; }
        public CellFlags Flags { get; set; }
		
        public enum CellFlags {
            Transparent = 1,
            DisableSelection = 2
        }
		
        /// <summary>
        ///   Constructor
        /// </summary>
        /// <param name="caption">
        /// The caption, only used for RootElements that might want to summarize results
        /// </param>
        /// <param name="view">
        /// The view to display
        /// </param>
        /// <param name="transparent">
        /// If this is set, then the view is responsible for painting the entire area,
        /// otherwise the default cell paint code will be used.
        /// </param>
        public UIViewElement (string caption, UIView view, bool transparent) : base (caption) 
        {
            this.View = view;
            this.Flags = transparent ? CellFlags.Transparent : 0;
            key = new NSString ("UIViewElement" + _count++);
        }
		
        protected override NSString CellKey {
            get {
                return key;
            }
        }
        protected override UITableViewCell GetCellImpl (UITableView tv)
        {
            var cell = tv.DequeueReusableCell (CellKey);
            if (cell == null){
                cell = new UITableViewCell (UITableViewCellStyle.Default, CellKey);
                if ((Flags & CellFlags.Transparent) != 0){
                    cell.BackgroundColor = UIColor.Clear;
					
                    // 
                    // This trick is necessary to keep the background clear, otherwise
                    // it gets painted as black
                    //
                    cell.BackgroundView = new UIView (RectangleF.Empty) { 
                                                                            BackgroundColor = UIColor.Clear 
                                                                        };
                }
                if ((Flags & CellFlags.DisableSelection) != 0)
                    cell.SelectionStyle = UITableViewCellSelectionStyle.None;

                if (Caption != null)
                    cell.TextLabel.Text = Caption;
                cell.ContentView.AddSubview (View);
            } 
            return cell;
        }
		
        public float GetHeight (UITableView tableView, NSIndexPath indexPath)
        {
            return View.Bounds.Height;
        }
		
        protected override void Dispose (bool disposing)
        {
            base.Dispose (disposing);
            if (disposing){
                if (View != null){
                    View.Dispose ();
                    View = null;
                }
            }
        }
    }
}