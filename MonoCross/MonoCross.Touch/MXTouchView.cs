using System;

using MonoTouch.UIKit;
using MonoTouch.Dialog;

using MonoCross.Navigation;

namespace MonoCross.Touch
{
    /// <summary>
    /// View types used in marking views with navigational attributes
    /// </summary>
	public enum ViewType { Master, Detail, Modal, Popover };

    /// <summary>
    ///
    /// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class MXTouchViewType: System.Attribute
	{
		public ViewType ViewType { get; set; }

		public MXTouchViewType(ViewType viewtype)
		{
			ViewType = viewtype;
		}
	}

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
        
        public event ModelEventHandler ViewModelChanged;        
        public virtual void OnViewModelChanged(object model) { }
        public void NotifyModelChanged() { if (ViewModelChanged != null) ViewModelChanged(this.Model); }
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
        
        public event ModelEventHandler ViewModelChanged;        
        public virtual void OnViewModelChanged(object model) { }
        public void NotifyModelChanged() { if (ViewModelChanged != null) ViewModelChanged(this.Model); }
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
        
        public event ModelEventHandler ViewModelChanged;        
        public virtual void OnViewModelChanged(object model) { }
        public void NotifyModelChanged() { if (ViewModelChanged != null) ViewModelChanged(this.Model); }
	}
	
	public abstract class MXTouchTabBarController<T> : UITabBarController, IMXView
	{
		public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public abstract void Render();
        public void SetModel(object model)
        {
            Model = (T)model;
        }
        
        public event ModelEventHandler ViewModelChanged;        
        public virtual void OnViewModelChanged(object model) { }
        public void NotifyModelChanged() { if (ViewModelChanged != null) ViewModelChanged(this.Model); }
	}
}