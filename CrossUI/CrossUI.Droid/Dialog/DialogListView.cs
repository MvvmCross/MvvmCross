// DialogListView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using CrossUI.Droid.Dialog.Elements;
using System;

namespace CrossUI.Droid.Dialog
{
    public class DialogListView : ListView
    {
        public RootElement Root
        {
            get { return _dialogAdapter?.Root; }
            set
            {
                value.ValueChanged -= HandleValueChangedEvent;
                value.ValueChanged += HandleValueChangedEvent;

                if (_dialogAdapter == null)
                    Adapter = _dialogAdapter = new DialogAdapter(Context, value, this);
                else
                    _dialogAdapter.Root = value;
            }
        }

        private DialogAdapter _dialogAdapter;

        public DialogListView(Context context) : base(context)
        {
            DescendantFocusability = DescendantFocusability.AfterDescendants;
            ItemsCanFocus = true;
        }

        public DialogListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            DescendantFocusability = DescendantFocusability.AfterDescendants;
            ItemsCanFocus = true;
        }

        public DialogListView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            DescendantFocusability = DescendantFocusability.AfterDescendants;
            ItemsCanFocus = true;
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            var currentFocus = ((Activity)Context).CurrentFocus;
            base.OnSizeChanged(w, h, oldw, oldh);
            //we have to place the focus request on the working-queueu, since by this time someone else has already requested the focus because of listactivity/listview's incompatiblity issues with edittext views
            new Handler().Post(() =>
                {
                    if (currentFocus != null && ((Activity)Context).CurrentFocus != currentFocus)
                    {
                        currentFocus.RequestFocus();
                        currentFocus.RequestFocusFromTouch();
                    }
                });
        }

        public event EventHandler ValueChanged;

        private void HandleValueChangedEvent(object sender, EventArgs args)
        {
            ValueChanged?.Invoke(sender, args);
        }

        public void ReloadData()
        {
            if (Root == null) return;
            _dialogAdapter.ReloadData();
        }
    }
}