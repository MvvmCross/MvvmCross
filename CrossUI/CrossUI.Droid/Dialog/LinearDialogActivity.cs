// LinearDialogActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Database;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CrossUI.Droid.Dialog.Elements;
using Java.Lang;

namespace CrossUI.Droid.Dialog
{
    /// <summary>
    /// DialogActivity based on a linear view, this will solve all edittext related focus problems when using elements 
    /// suggestions at http://stackoverflow.com/questions/2679948/focusable-edittext-inside-listview doesn't help for example
    /// </summary>
    public class LinearDialogActivity : Activity
    {
        public LinearDialogActivity()
        {
        }

        public override void OnContentChanged()
        {
            base.OnContentChanged();
            var list = FindViewById<LinearDialogScrollView>(Android.Resource.Id.List);

            if (list == null)
            {
                throw new RuntimeException("Your content must have a ViewGroup whose id attribute is Android.Resource.Id.List and is of type LinearDialogScrollView");
            }

            list.AddViews();
        }


        public RootElement Root
        {
            get { return FindViewById<LinearDialogScrollView>(Android.Resource.Id.List).Root; }
            set { FindViewById<LinearDialogScrollView>(Android.Resource.Id.List).Root = value; }
        }
    }
}