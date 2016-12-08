using System;
namespace MvvmCross.iOS.Support.Presenters
{
	public class MvxTabPresentationAttribute : Attribute
	{
		/// <summary>
		/// The presentation mode.
		/// </summary>
		public readonly MvxTabPresentationMode Mode;

		/// <summary>
		/// Defines the image icon to be displayed in the TabBarItem
		/// </summary>
		public readonly string TabIconName;

		/// <summary>
		/// Defines the title to be displayed in the TabBarItem
		/// </summary>
		public readonly string TabTitle;

		/// <summary>
		/// Set to true if want to wrap the view inside a NavigationController
		/// </summary>
		public readonly bool WrapInNavigationController;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MvvmCross.iOS.Support.Presenters.MvxTabPresentationAttribute"/> class.
		/// </summary>
		/// <param name="mode">The presentation mode</param>
		/// <param name="wrapInNavigationController">If set to <c>true</c> if want to wrap in navigation controller.</param>
		public MvxTabPresentationAttribute(MvxTabPresentationMode mode, bool wrapInNavigationController = false)
		{
			Mode = mode;
			WrapInNavigationController = wrapInNavigationController;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MvvmCross.iOS.Support.Presenters.MvxTabPresentationAttribute"/> class.
		/// </summary>
		/// <param name="mode">The presentation mode</param>
		/// <param name="wrapInNavigationController">If set to <c>true</c> if want to wrap in navigation controller.</param>
		/// <param name="tabIconName">Tab icon name.</param>
		/// <param name="tabTitle">Tab title.</param>
		public MvxTabPresentationAttribute(MvxTabPresentationMode mode, string tabTitle, string tabIconName, bool wrapInNavigationController = false)
		{
			Mode = mode;
			TabIconName = tabIconName;
			TabTitle = tabTitle;
			WrapInNavigationController = wrapInNavigationController;
		}
	}

	public enum MvxTabPresentationMode
	{
		Root,
		Tab,
		Child,
		Modal
	}
}

