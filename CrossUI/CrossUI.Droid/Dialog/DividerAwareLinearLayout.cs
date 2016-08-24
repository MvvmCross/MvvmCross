using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;

namespace CrossUI.Droid.Dialog
{
    public class DividerAwareLinearLayout : LinearLayout
    {
        public const int IGNORE_DIVIDER_FOR_CHILD_TAG = 987452931;

        private Drawable _divider;
        private const int SHOW_DIVIDER_ALL = 7;
        private const int SHOW_DIVIDER_BEGINNING = 1;
        private const int SHOW_DIVIDER_END = 4;
        private const int SHOW_DIVIDER_MIDDLE = 2;
        public const int SHOW_DIVIDER_NONE = 0;
        private int _dividerHeight;
        private int _dividerPadding;
        private int _dividerWidth;
        private int _showDividers = SHOW_DIVIDER_NONE;

        protected DividerAwareLinearLayout(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public DividerAwareLinearLayout(Context context) : base(context)
        {
        }

        public DividerAwareLinearLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        #region Divider drawables

        protected bool HasDividerBeforeChildAt(int childIndex)
        {
            if (childIndex == 0)
            {
                return (_showDividers & SHOW_DIVIDER_BEGINNING) != 0;
            }
            else if (childIndex == ChildCount)
            {
                return (_showDividers & SHOW_DIVIDER_END) != 0;
            }
            else if ((_showDividers & SHOW_DIVIDER_MIDDLE) != 0)
            {
                for (int i = childIndex - 1; i >= 0; i--)
                {
                    var child = GetChildAt(i);
                    if (child.Visibility != ViewStates.Gone && child.GetTag(IGNORE_DIVIDER_FOR_CHILD_TAG) == null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected void DrawDividersHorizontal(Canvas canvas)
        {
            int count = ChildCount;
            for (int i = 0; i < count; i++)
            {
                View child = GetChildAt(i);
                if (child != null && child.Visibility != ViewStates.Gone)
                {
                    if (HasDividerBeforeChildAt(i))
                    {
                        var lp = (MarginLayoutParams)child.LayoutParameters;
                        int left = child.Left - lp.LeftMargin;
                        DrawVerticalDivider(canvas, left);
                    }
                }
            }
            if (HasDividerBeforeChildAt(count))
            {
                View child = GetChildAt(count - 1);
                int right = 0;
                if (child == null)
                {
                    right = Width - PaddingRight;
                }
                else
                {
                    var lp = (MarginLayoutParams)child.LayoutParameters;
                    right = child.Right + lp.RightMargin;
                }
                DrawVerticalDivider(canvas, right);
            }
        }

        protected void DrawDividersVertical(Canvas canvas)
        {
            int count = ChildCount;
            for (int i = 0; i < count; i++)
            {
                View child = GetChildAt(i);

                if (child != null && child.Visibility != ViewStates.Gone)
                {
                    if (HasDividerBeforeChildAt(i))
                    {
                        var lp = (MarginLayoutParams)child.LayoutParameters;
                        int top = child.Top - lp.TopMargin;
                        DrawHorizontalDivider(canvas, top);
                    }
                }
            }
            if (HasDividerBeforeChildAt(count))
            {
                View child = GetChildAt(count - 1);
                int bottom = 0;
                if (child == null)
                {
                    bottom = Height - PaddingBottom;
                }
                else
                {
                    var lp = (MarginLayoutParams)child.LayoutParameters;
                    bottom = child.Bottom + lp.BottomMargin;
                }
                DrawHorizontalDivider(canvas, bottom);
            }
        }

        protected void DrawHorizontalDivider(Canvas canvas, int top)
        {
            _divider.Bounds = new Rect(PaddingLeft + _dividerPadding, top, Width - PaddingRight - _dividerPadding, top + DividerHeight);
            _divider.Draw(canvas);
        }

        protected void DrawVerticalDivider(Canvas canvas, int left)
        {
            _divider.Bounds = new Rect(left, PaddingTop + _dividerPadding, left + _dividerWidth, Height - PaddingBottom - _dividerPadding);
            _divider.Draw(canvas);
        }

        public virtual Drawable DividerDrawable
        {
            get { return _divider; }
            set
            {
                if (value == _divider)
                {
                    return;
                }
                _divider = value;
                if (_divider != null)
                {
                    _dividerWidth = _divider.IntrinsicWidth;
                    DividerHeight = _divider.IntrinsicHeight;
                }
                else
                {
                    _dividerWidth = 0;
                    DividerHeight = 0;
                }
                SetWillNotDraw(_divider == null);
                RequestLayout();
            }
        }

        public virtual int DividerPadding
        {
            get { return _dividerPadding; }
            set { _dividerPadding = value; }
        }

        public virtual int ShowDividers
        {
            get { return _showDividers; }
            set { _showDividers = value; }
        }

        public int DividerHeight
        {
            get { return _dividerHeight; }
            set { _dividerHeight = value; }
        }

        protected override void OnDraw(Canvas canvas)
        {
            if (_divider != null)
            {
                if (Orientation == Orientation.Vertical)
                {
                    DrawDividersVertical(canvas);
                }
                else
                {
                    DrawDividersHorizontal(canvas);
                }
            }

            base.OnDraw(canvas);
        }

        protected override void MeasureChildWithMargins(View child, int parentWidthMeasureSpec, int widthUsed,
                                                        int parentHeightMeasureSpec, int heightUsed)
        {
            var index = IndexOfChild(child);
            var orientation = Orientation;
            var @params = (MarginLayoutParams)child.LayoutParameters;
            if (HasDividerBeforeChildAt(index))
            {
                if (orientation == Orientation.Vertical)
                {
                    @params.TopMargin = _dividerHeight;
                }
                else
                {
                    @params.LeftMargin = _dividerWidth;
                }
            }
            int count = ChildCount;
            if (index == count - 1)
            {
                if (HasDividerBeforeChildAt(count))
                {
                    if (orientation == Orientation.Vertical)
                    {
                        @params.BottomMargin = _dividerHeight;
                    }
                    else
                    {
                        @params.RightMargin = _dividerWidth;
                    }
                }
            }
            base.MeasureChildWithMargins(child, parentWidthMeasureSpec, widthUsed, parentHeightMeasureSpec, heightUsed);
        }

        #endregion Divider drawables
    }
}