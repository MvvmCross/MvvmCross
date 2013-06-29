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
    public class LinearDialogListView : ScrollView
    {
        private DialogAdapter _dialogAdapter;
        private CustomDataSetObserver _observer;

        protected LinearDialogListView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public LinearDialogListView(Context context, IAttributeSet attrs)
            : this(context, attrs, 0)
        {
        }

        public LinearDialogListView(Context context, IAttributeSet attrs, int defStyle)
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

            }

            if (_divider == null)
            {
                _divider = Resources.GetDrawable(Android.Resource.Drawable.DividerHorizontalBright);
            }
        }

        public LinearDialogListView(Context context)
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
                if (_divider != null)
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

        public void ListView_ItemLongClick(object sender, View.LongClickEventArgs longClickEventArgs)
        {
            var position = (int)((View)sender).GetTag(_TAG_INDEX);
            var elem = _dialogAdapter.ElementAtIndex(position);
            if (elem == null) return;
            if (elem.LongClick != null)
                elem.LongClick(this, EventArgs.Empty);
        }

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
            if (ValueChanged != null)
                ValueChanged(sender, args);
        }

        public void ReloadData()
        {
            if (Root == null) return;
            _dialogAdapter.ReloadData();
        }

    }

    [Register("android/R$styleable", DoNotGenerateAcw = true)]
    public class InternalStyleable : Java.Lang.Object
    {
        internal static IntPtr java_class_handle;
        private static IntPtr id_ctor;
        private static IntPtr scrollViewFieldId;
        private static IntPtr scrollView_fillViewportFieldId;

        internal static IntPtr class_ref
        {
            get { return JNIEnv.FindClass("android/R$styleable", ref java_class_handle); }
        }

        protected override IntPtr ThresholdClass
        {
            get { return class_ref; }
        }

        protected override Type ThresholdType
        {
            get { return typeof(InternalStyleable); }
        }

        internal InternalStyleable(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        [Register(".ctor", "()V", "")]
        public InternalStyleable()
            : base(IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
        {
            if (this.Handle != IntPtr.Zero)
                return;
            if (this.GetType() != typeof(InternalStyleable))
            {
                this.SetHandle(JNIEnv.CreateInstance(GetType(), "()V", new JValue[0]),
                                JniHandleOwnership.TransferLocalRef);
            }
            else
            {
                if (id_ctor == IntPtr.Zero)
                    id_ctor = JNIEnv.GetMethodID(class_ref, "<init>", "()V");
                this.SetHandle(JNIEnv.NewObject(class_ref, id_ctor), JniHandleOwnership.TransferLocalRef);
            }
        }

        public static int[] ScrollView
        {
            get
            {
                if (scrollViewFieldId == IntPtr.Zero)
                    scrollViewFieldId = JNIEnv.GetStaticFieldID(class_ref, "ScrollView", "[I");
                IntPtr ret = JNIEnv.GetStaticObjectField(class_ref, scrollViewFieldId);
                return GetObject<JavaArray<int>>(ret, JniHandleOwnership.TransferLocalRef).ToArray<int>();
            }
        }

        public static int[] ListView
        {
            get
            {
                if (scrollViewFieldId == IntPtr.Zero)
                    scrollViewFieldId = JNIEnv.GetStaticFieldID(class_ref, "ListView", "[I");
                IntPtr ret = JNIEnv.GetStaticObjectField(class_ref, scrollViewFieldId);
                return GetObject<JavaArray<int>>(ret, JniHandleOwnership.TransferLocalRef).ToArray<int>();
            }
        }

        public static int ListView_divider
        {
            get
            {
                if (scrollView_fillViewportFieldId == IntPtr.Zero)
                    scrollView_fillViewportFieldId = JNIEnv.GetStaticFieldID(class_ref, "ScrollView_fillViewport", "I");
                return JNIEnv.GetStaticIntField(class_ref, scrollView_fillViewportFieldId);
            }
        }
    }
}