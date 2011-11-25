using System;

namespace MonoCross.Touch
{
	/// <summary>
    /// View types used in marking views with navigational attributes
    /// </summary>
	public enum ViewNavigationContext
	{
		/// <summary>
		/// decide in context whether to place the view in the master or detail pane following the view that
		/// this is the default behavior if a view isn't marked with a style
		/// </summary>
		InContext,

		/// <summary>
		/// always place in the master pone in a large form factor layout
		/// </summary>
		Master,

		/// <summary>
		/// always place in the detail pane in a large form factor layout, this is equivalent to Master for small
		/// form-factor platforms
		/// </summary>
		Detail,
		
		/// <summary>
		/// view is a modal popup 
		/// </summary>
		Modal,
	};
	
	/// <summary>
    ///
    /// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class MXTouchViewAttributes: System.Attribute
	{
		public ViewNavigationContext NavigationContext { get; set; }

		public MXTouchViewAttributes(ViewNavigationContext navigationContext)
		{
			NavigationContext = navigationContext;
		}
	}
}

