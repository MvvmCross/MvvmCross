// LinearDialogScrollView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using CrossUI.Droid.Dialog.Elements;
using Orientation = Android.Widget.Orientation;

namespace CrossUI.Droid.Dialog
{
    public class LinearDialogScrollView : ScrollView
    {
        private DialogAdapter _dialogAdapter;
        private CustomDataSetObserver _observer;

        protected LinearDialogScrollView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

#warning Why does this use 0? The non attrs constructor uses default of Android.Resource.Style.WidgetListView
        public LinearDialogScrollView(Context context, IAttributeSet attrs)
            : this(context, attrs, 0)
        {
        }

        public LinearDialogScrollView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            try
            {
                TypedArray a = context.ObtainStyledAttributes(attrs, InternalStyleable.ListView, defStyle, 0);

                var d = a.GetDrawable(InternalStyleable.ListView_divider);
                if (d != null)
                {
                    // If a divider is specified use its intrinsic height for divider height
                    _divider = d;
                }
            }
            catch (System.Exception)
            {
#warning Is this pokemon needed? SHould there at least be a trace here?
            }

            if (_divider == null)
            {
                _divider = Resources.GetDrawable(Android.Resource.Drawable.DividerHorizontalBright);
            }
        }

        public LinearDialogScrollView(Context context)
            : this(context, null, Android.Resource.Style.WidgetListView)
        {
        }

        public RootElement Root
        {
            get { return _dialogAdapter == null ? null : _dialogAdapter.Root; }
            set
            {
                value.ValueChanged -= HandleValueChangedEvent;
                value.ValueChanged += HandleValueChangedEvent;

                if (_dialogAdapter != null)
                {
                    _dialogAdapter.DeregisterListView();
                    _dialogAdapter.UnregisterDataSetObserver(_observer);
                }

                _dialogAdapter = new DialogAdapter(this.Context, value);
                if (_observer == null)
                {
                    _observer = new CustomDataSetObserver();
                    _observer.Changed += ObserverOnChanged;
                    _observer.Invalidated += ObserverOnChanged;
                }
                _dialogAdapter.RegisterDataSetObserver(_observer);

                AddViews();
            }
        }

        private void ObserverOnChanged(object sender, EventArgs eventArgs)
        {
            AddViews();
        }

        private int _TAG_INDEX = 82171829;
        private LinearLayout _list;
        private Drawable _divider;

        public void AddViews()
        {
            if (_dialogAdapter == null)
                return;
            if (_list == null)
            {
                var @params = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent);
                _list = new LinearLayout(this.Context, null);
                _list.LayoutParameters = @params;
                _list.Orientation = Orientation.Vertical;
                AddView(_list);

            }
            _list.RemoveAllViews();
            AddFocusable();
            for (var i = 0; i < _dialogAdapter.Count; i++)
            {
                var view = _dialogAdapter.GetView(i, null, _list);
                view.SetTag(_TAG_INDEX, i);
                view.Click -= ListView_ItemClick;
                view.LongClick -= ListView_ItemLongClick;
                view.Click += ListView_ItemClick;
                view.LongClick += ListView_ItemLongClick;

                view.Focusable = false;
                view.FocusableInTouchMode = false;
                view.Clickable = true;
                view.LongClickable = true;
                //TODO: make this styleable
                view.SetBackgroundDrawable(Resources.GetDrawable(Android.Resource.Drawable.ListSelectorBackground));

                _list.AddView(view);
                if ((view.Visibility == ViewStates.Visible) && (_divider != null))
                {
                    var dividerImage = new ImageView(this.Context);
                    dividerImage.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.WrapContent);
                    dividerImage.SetPadding(5, 2, 5, 2);
                    dividerImage.SetScaleType(ImageView.ScaleType.FitXy);
                    dividerImage.SetImageDrawable(_divider);
                    _list.AddView(dividerImage);
                }

            }
        }

        private void AddFocusable()
        {
            //add a layout to get first focus of the screen, somehow I can't get windowsoftinputmode=stateHidden to work
            //http://stackoverflow.com/questions/1555109/stop-edittext-from-gaining-focus-at-activity-startup
            var focusableLayout = new LinearLayout(this.Context, null);
            focusableLayout.LayoutParameters = new RelativeLayout.LayoutParams(1, 1);
            focusableLayout.Focusable = true;
            focusableLayout.FocusableInTouchMode = true;
            _list.AddView(focusableLayout);
        }

        public void ListView_ItemClick(object sender, EventArgs eventArgs)
        {
            var position = (int)((View)sender).GetTag(_TAG_INDEX);
            var elem = _dialogAdapter.ElementAtIndex(position);
            if (elem == null) return;
            elem.Selected();
            if (elem.Click != null)
                elem.Click(this, EventArgs.Empty);
        }

#warning Just checking this is always wanted - see @csteeg's question on https://github.com/slodge/MvvmCross/issues/281
        public void ListView_ItemLongClick(object sender, View.LongClickEventArgs longClickEventArgs)
        {
            var position = (int)((View)sender).GetTag(_TAG_INDEX);
            var elem = _dialogAdapter.ElementAtIndex(position);
            if (elem == null) return;
            if (elem.LongClick != null)
                elem.LongClick(this, EventArgs.Empty);
        }

#warning The naming of this feels wrong ? Also feels like it might not work if any dynamic elements are ever used
        public void HandleValueChangedEvents(EventHandler eventHandler)
        {
            foreach (var element in Root.Sections.SelectMany(section => section))
            {
                if (element is ValueElement)
                    (element as ValueElement).ValueChanged += eventHandler;
            }
        }

        public event EventHandler ValueChanged;

        private void HandleValueChangedEvent(object sender, EventArgs args)
        {
            var handler = ValueChanged;
            if (handler != null)
                handler(sender, args);
        }

        public void ReloadData()
        {
            if (Root == null) return;
            _dialogAdapter.ReloadData();
        }
    }
}