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
	}

	public enum MvxTabPresentationMode
	{
		Root,
		Tab,
		Child,
		Modal
	}
}

