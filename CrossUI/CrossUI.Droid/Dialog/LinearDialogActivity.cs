// LinearDialogActivity.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
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
            set
            {
                var list = EnsureListView();

                if (list == null)
                {
                    throw new RuntimeException("Your content must have a ViewGroup whose id attribute is Android.Resource.Id.List and is of type LinearDialogScrollView");
                }

                list.Root = value;
            }
        }

        private LinearDialogScrollView EnsureListView()
        {
            //if no content has manually been set, ensure our own layout:
            var list = FindViewById<LinearDialogScrollView>(Android.Resource.Id.List);
            if (list == null)
            {
                //create a default content/layout:
                var linearLayout = new LinearLayout(this);
                list = new LinearDialogScrollView(this);
                list.Id = Android.Resource.Id.List;
                linearLayout.AddView(list, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
                this.AddContentView(linearLayout, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            }

            return list;
        }
    }
}