using System;
using System.Collections;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Accessibility;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Cirrious.MvvmCross.Droid.RecyclerView
{
    [Register("cirrious.mvvmcross.droid.recyclerview.MvxRecyclerView")]
    public class MvxRecyclerView : Android.Support.V7.Widget.RecyclerView, Android.Support.V7.Widget.RecyclerView.IOnItemTouchListener
    {
        private readonly GestureDetector _gestureDetector;

        #region ctor

        protected MvxRecyclerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        public MvxRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, 0, new MvxRecyclerAdapter()) { }

        public MvxRecyclerView(Context context, IAttributeSet attrs, int defStyle) : this(context, attrs, defStyle, new MvxRecyclerAdapter()) { }

        public MvxRecyclerView(Context context, IAttributeSet attrs, int defStyle, IMvxRecyclerAdapter adapter)
            : base(context, attrs, defStyle)
        {
            _gestureDetector = new GestureDetector(context, new GestureListener());
            this.AddOnItemTouchListener(this);

            // Note: Any calling derived class passing a null adapter is responsible for setting
            // it's own itemTemplateId
            if (adapter == null)
                return;

            SetLayoutManager(new LinearLayoutManager(context));

            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;
            Adapter = adapter;
        }

        #endregion

        public sealed override void AddOnItemTouchListener(IOnItemTouchListener listener)
        {
            base.AddOnItemTouchListener(listener);
        }
        public sealed override void SetLayoutManager(LayoutManager layout)
        {
            base.SetLayoutManager(layout);
        }

        public new IMvxRecyclerAdapter Adapter
        {
            get { return base.GetAdapter() as IMvxRecyclerAdapter; }
            set
            {
                var existing = Adapter;

                if (existing == value)
                    return;

                // Support lib doesn't seem to have anything similar to IListAdapter yet
                // hence cast to Adapter.

                if (value != null && existing != null)
                {
                    value.ItemsSource = existing.ItemsSource;
                    value.ItemTemplateId = existing.ItemTemplateId;

                    SwapAdapter((Adapter)value, false);
                }
                else
                {
                    SetAdapter((Adapter)value);
                }
            }
        }

        [MvxSetToNullAfterBinding]
        public IEnumerable ItemsSource
        {
            get { return Adapter.ItemsSource; }
            set { Adapter.ItemsSource = value; }
        }

        public int ItemTemplateId
        {
            get { return Adapter.ItemTemplateId; }
            set { Adapter.ItemTemplateId = value; }
        }

        public ICommand ItemClick { get; set; }

        protected virtual void ExecuteCommandOnItem(ICommand command, int position)
        {
            if (command == null)
                return;

            var item = Adapter.GetItem(position);

            if (item == null || !command.CanExecute(item))
                return;

            command.Execute(item);
        }

        #region IOnItemTouchListener

        public virtual bool OnInterceptTouchEvent(Android.Support.V7.Widget.RecyclerView view, MotionEvent e)
        {
            View childView = view.FindChildViewUnder(e.GetX(), e.GetY());
            if (childView != null && _gestureDetector.OnTouchEvent(e))
            {
                PlaySoundEffect(SoundEffects.Click);
                ExecuteCommandOnItem(this.ItemClick, view.GetChildPosition(childView));
                childView.SendAccessibilityEvent(EventTypes.ViewClicked);
                return true;
            }
            return false;
        }

        public virtual void OnTouchEvent(Android.Support.V7.Widget.RecyclerView view, MotionEvent e) { }

        class GestureListener : GestureDetector.SimpleOnGestureListener
        {
            public override bool OnSingleTapUp(MotionEvent e)
            {
                return true;
            }
        }

        #endregion
    }
}
