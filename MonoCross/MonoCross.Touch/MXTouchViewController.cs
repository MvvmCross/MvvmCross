using System;

using MonoTouch.UIKit;
using MonoTouch.Dialog;

using MonoCross.Navigation;

namespace MonoCross.Touch
{
    /// <summary>
    ///
    /// </summary>
    public abstract class MXTouchViewController<T>: UIViewController, IMXView
    {
        public MXTouchViewController ()
        {
        }

        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public abstract void Render();
        public void SetModel(object model)
        {
            Model = (T)model;
        }
    }

    public abstract class MXTouchTableViewController<T>: UITableViewController, IMXView
    {
        public MXTouchTableViewController()
        {
        }

        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public abstract void Render();
        public void SetModel(object model)
        {
            Model = (T)model;
        }
    }
	
	public abstract class MXTouchDialogView<T>: DialogViewController, IMXView
	{
        public MXTouchDialogView(UITableViewStyle style, RootElement root, bool pushing):
			base(style, root, pushing)
        {
        }

        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public abstract void Render();
        public void SetModel(object model)
        {
            Model = (T)model;
        }
	}
}
