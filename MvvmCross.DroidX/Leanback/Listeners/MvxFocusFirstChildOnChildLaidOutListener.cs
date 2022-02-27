// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;
using AndroidX.Leanback.Widget;
using Java.Lang;

namespace MvvmCross.DroidX.Leanback.Listeners
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
