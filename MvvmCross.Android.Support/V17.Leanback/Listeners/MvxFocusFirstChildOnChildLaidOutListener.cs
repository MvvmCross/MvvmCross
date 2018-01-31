// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Support.V17.Leanback.Widget;
using Android.Views;
using Java.Lang;

namespace MvvmCross.Droid.Support.V17.Leanback.Listeners
{
	/// <summary>
	/// Requests focus for first laid out child.
	/// </summary>
	public class MvxFocusFirstChildOnChildLaidOutListener 
        : Object, IOnChildLaidOutListener
	{
		public void OnChildLaidOut(ViewGroup parent, View view, int position, long id)
		{
			if (view != null && position == 0)
			{
				view.RequestFocus();
			}
		}
	}
}