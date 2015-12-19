// LinearDialogScrollView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using CrossUI.Droid.Dialog.Elements;
using System;
using System.Linq;
using Orientation = Android.Widget.Orientation;

namespace CrossUI.Droid.Dialog
{
    [Register("crossui.droid.dialog.LinearDialogScrollView")]
    public class LinearDialogScrollView : ScrollView
    {
        private CustomDataSetObserver _observer;
        private const int _TAG_INDEX = 82171829;
        private DividerAwareLinearLayout _list;

        private DialogAdapter _dialogAdapter;

        protected LinearDialogScrollView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public LinearDialogScrollView(Context context, IAttributeSet attrs)
            : this(context, attrs, Android.Resource.Style.WidgetListView)
        {
        }

        public LinearDialogScrollView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Init(attrs, defStyle);
        }

        public LinearDialogScrollView(Context context)
            : this(context, null, Android.Resource.Style.WidgetListView)
        {
        }

        public RootElement Root
        {
            get { return _dialogAdapter?.Root; }
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Root != null)
                {
                    Root.ValueChanged -= HandleValueChangedEvent;
                }
                if (_observer != null)
                {
                    _observer.Changed -= ObserverOnChanged;
                    _observer.Invalidated -= ObserverOnChanged;
                    _observer.Dispose();
                    _observer = null;
                }
                if (_dialogAdapter != null)
                {
                    _dialogAdapter.DeregisterListView();
                    _dialogAdapter.Dispose();
                    _dialogAdapter = null;
                }
                if (_list != null)
                {
                    _list.Dispose();
                    _list = null;
                }

                //lists can take up a huge numer of gref's, force a cleanup here as soon as we don't need these any longer
                GC.Collect(0);
            }
            base.Dispose(disposing);
        }

        protected void Init(IAttributeSet attrs, int defStyleRes)
        {
            var @params = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            _list = new DividerAwareLinearLayout(this.Context, null);
            _list.LayoutParameters = @params;
            _list.Orientation = Orientation.Vertical;
            AddFocusable();
            AddView(_list);

            LinearDialogStyleableResource.Initialise();

            TypedArray a = Context.ObtainStyledAttributes(attrs, LinearDialogStyleableResource.LinearDialogScrollViewStylableGroupId, defStyleRes, 0);
            if (LinearDialogStyleableResource.LinearDialogScrollViewDivider > -1)
                _list.DividerDrawable = a.GetDrawable(LinearDialogStyleableResource.LinearDialogScrollViewDivider);
            if (LinearDialogStyleableResource.LinearDialogScrollViewShowDividers > -1)
                _list.ShowDividers = a.GetInt(LinearDialogStyleableResource.LinearDialogScrollViewShowDividers, DividerAwareLinearLayout.SHOW_DIVIDER_NONE);
            if (LinearDialogStyleableResource.LinearDialogScrollDividerPadding > -1)
                _list.DividerPadding = a.GetDimensionPixelSize(LinearDialogStyleableResource.LinearDialogScrollDividerPadding, 0);
            if (LinearDialogStyleableResource.LinearDialogScrollDividerHeight > -1)
                _list.DividerHeight = a.GetDimensionPixelSize(LinearDialogStyleableResource.LinearDialogScrollDividerHeight, 0);
            if (LinearDialogStyleableResource.LinearDialogScrollItemBackgroundDrawable > -1)
                ItemBackgroundDrawable = a.GetDrawable(LinearDialogStyleableResource.LinearDialogScrollItemBackgroundDrawable);

            a.Recycle();
        }

        private void ObserverOnChanged(object sender, EventArgs eventArgs)
        {
            AddViews();
        }

        public void AddViews()
        {
            if (_dialogAdapter == null)
                return;

            for (var i = 0; i < _dialogAdapter.Count; i++)
            {
                var currentView = _list.ChildCount >= i + 1 ? _list.GetChildAt(i + 1) : null;
                var view = _dialogAdapter.GetView(i, currentView, _list);
                if (view == null)//eg if the layout fails to inflate, on other places only a warning is ouputted, so just ignore it here and have a look at the warnings
                {
                    continue;
                }
                view.SetTag(_TAG_INDEX, i);
                EnsureClickEventsSubscribedToOnceAndOnlyOnce(view);

                view.SetBackgroundDrawable(ItemBackgroundDrawable ?? Resources.GetDrawable(Android.Resource.Drawable.ListSelectorBackground));

                if (currentView != null)
                {
                    if (currentView != view) // we had a view in here, but it should be the new guy
                    {
                        currentView.Click -= ListView_ItemClick;
                        currentView.LongClick -= ListView_ItemLongClick;
                        currentView.Dispose();
                        _list.RemoveViewAt(i + 1);
                        _list.AddView(view, i + 1);
                    }
                }
                else
                {
                    _list.AddView(view);
                }
            }

            //remove remaining
            for (var i = _list.ChildCount; i > _dialogAdapter.Count + 1; i--)
            {
                _list.RemoveViewAt(i - 1);
            }
        }

        private void EnsureClickEventsSubscribedToOnceAndOnlyOnce(View view)
        {
            // to ensure we don't subscribe to the event more than once, we first unsubscribe
            // C# will ignore these unsubscriptions if we haven't previous subscribed
            // see
            // - http://stackoverflow.com/questions/937181/c-sharp-pattern-to-prevent-an-event-handler-hooked-twice
            // - notes in https://github.com/MvvmCross/MvvmCross/pull/363
            view.Click -= ListView_ItemClick;
            view.LongClick -= ListView_ItemLongClick;

            view.Click += ListView_ItemClick;
            view.LongClick += ListView_ItemLongClick;
        }

        public virtual Drawable ItemBackgroundDrawable { get; set; }

        private void AddFocusable()
        {
            //add a layout to get first focus of the screen, somehow I can't get windowsoftinputmode=stateHidden to work
            //http://stackoverflow.com/questions/1555109/stop-edittext-from-gaining-focus-at-activity-startup
            var focusableLayout = new LinearLayout(this.Context, null);
            focusableLayout.SetTag(DividerAwareLinearLayout.IGNORE_DIVIDER_FOR_CHILD_TAG, true);
            focusableLayout.LayoutParameters = new RelativeLayout.LayoutParams(1, 1);
            focusableLayout.Focusable = true;
            focusableLayout.FocusableInTouchMode = true;
            _list.AddView(focusableLayout);
        }

        public void ListView_ItemClick(object sender, EventArgs eventArgs)
        {
            var senderView = ((View)sender);
            var position = (int)senderView.GetTag(_TAG_INDEX);
            _dialogAdapter.ListView_ItemClick(sender, new AdapterView.ItemClickEventArgs(null, senderView, position, senderView.Id));
        }

#warning Just checking this is always wanted - see @csteeg's question on https://github.com/slodge/MvvmCross/issues/281

        public void ListView_ItemLongClick(object sender, View.LongClickEventArgs longClickEventArgs)
        {
            var senderView = ((View)sender);
            var position = (int)senderView.GetTag(_TAG_INDEX);
            _dialogAdapter.ListView_ItemLongClick(sender, new AdapterView.ItemLongClickEventArgs(false, null, senderView, position, senderView.Id));
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
            handler?.Invoke(sender, args);
        }

        public void ReloadData()
        {
            if (Root == null) return;
            _dialogAdapter.ReloadData();
        }
    }
}