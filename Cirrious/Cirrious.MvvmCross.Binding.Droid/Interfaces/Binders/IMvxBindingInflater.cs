using System;
using Android.Util;
using Android.Content;
using Android.Views;

namespace Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders
{
	/// <summary>
	/// Inflates elements from Android XML files and attaches binding descriptions to them. If the view already
    /// exist it can just attach the binding information to it.
	/// </summary>
	public interface IMvxBindingInflater
	{
		/// <summary>
		/// Parses the <paramref name="attrs"/> for binding information and attaches it to the <paramref name="view"/>.
		/// </summary>
		/// <param name="view">View to attach the information to.</param>
		/// <param name="attrs">XML attributes for the view.</param>
		void Attach(View view, IAttributeSet attrs);

		/// <summary>
		/// Parses the <paramref name="attrs"/> and if binding information present, tries to create the
		/// view from <paramref name="name"/> and attaches the information to it.
		/// </summary>
		/// <param name="name">Name of the XML tag for which to create the <see cref="View"/>.</param>
		/// <param name="context">Current context.</param>
		/// <param name="attrs">XML attributes for the view.</param>
		/// <returns>
        /// The newly created view. <c>null</c> if no binding information in <paramref name="attrs"/>
        /// or creation of the view failed.
		/// </returns>
		View Inflate(string name, Context context, IAttributeSet attrs);
	}
}