// FloatElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Globalization;

namespace CrossUI.Droid.Dialog.Elements
{
    public class FloatElement : ValueElement<float>, SeekBar.IOnSeekBarChangeListener
    {
        private int _precision = 10000000;

        public int Precision
        {
            get { return _precision; }
            set
            {
                _precision = value;
                ActOnCurrentAttachedCell(UpdateDetailDisplay);
            }
        }

        private float _maxValue;

        public float MaxValue
        {
            get { return _maxValue; }
            set
            {
                if (value < _minValue)
                {
                    // this protects the situation where the user sets the MaxValue before the MinValue
                    MinValue = value - 0.0001f;
                }

                if (Value > value)
                    Value = value;

                _maxValue = value;
                ActOnCurrentAttachedCell(UpdateDetailDisplay);
            }
        }

        private float _minValue;

        public float MinValue
        {
            get { return _minValue; }
            set
            {
                if (value > _maxValue)
                {
                    // this protects the situation where the user sets the MaxValue before the MinValue
                    MaxValue = value + 0.0001f;
                }

                if (Value < value)
                    Value = value;

                _minValue = value;
                ActOnCurrentAttachedCell(UpdateDetailDisplay);
            }
        }

        private bool _showCaption;

        public bool ShowCaption
        {
            get { return _showCaption; }
            set
            {
                _showCaption = value;
                ActOnCurrentAttachedCell(UpdateCaptionDisplay);
            }
        }

        public Bitmap Left { get; set; }
        public Bitmap Right { get; set; }

        protected override void UpdateDetailDisplay(View cell)
        {
            if (cell == null)
                return;

            TextView label;
            SeekBar slider;
            ImageView left;
            ImageView right;
            DroidResources.DecodeFloatElementLayout(Context, cell, out label, out slider, out left, out right);
            if (left != null)
            {
                if (Left != null)
                    left.SetImageBitmap(Left);
                else
                    left.Visibility = ViewStates.Gone;
            }
            if (right != null)
            {
                if (Right != null)
                    right.SetImageBitmap(Right);
                else
                    right.Visibility = ViewStates.Gone;
            }
            if (slider != null)
            {
                // Setting the two slider properties should be an atomic operation with no side effects.
                // However, setting Max may trigger OnProgressChanged, which will modify Value,
                // which will then be wrong when setting Progress.
                // Temporarily remove the listener to get over this hump.
                slider.SetOnSeekBarChangeListener(null);
                try
                {
                    slider.Max = (int)((_maxValue - _minValue) * Precision);
                    slider.Progress = (int)((Value - _minValue) * Precision);
                }
                finally
                {
                    slider.SetOnSeekBarChangeListener(this);
                }
            }
        }

        protected override void UpdateCellDisplay(View cell)
        {
            UpdateDetailDisplay(cell);
            base.UpdateCellDisplay(cell);
        }

        protected override void UpdateCaptionDisplay(View cell)
        {
            if (cell == null)
                return;

            TextView label;
            SeekBar slider;
            ImageView left;
            ImageView right;
            DroidResources.DecodeFloatElementLayout(Context, cell, out label, out slider, out left, out right);
            if (label != null)
            {
                if (ShowCaption)
                    label.Text = Caption;
                else
                    label.Visibility = ViewStates.Gone;
            }
        }

        public FloatElement(string caption = null, Bitmap left = null, Bitmap right = null, float value = 0,
                            string layoutName = null)
            : base(null, value, layoutName ?? "dialog_floatimage")
        {
            Left = left;
            Right = right;
            MinValue = 0;
            MaxValue = 1;
        }

        protected override View GetViewImpl(Context context, ViewGroup parent)
        {
            View view = DroidResources.LoadFloatElementLayout(context, parent, LayoutName);

            if (view == null)
            {
                Android.Util.Log.Error("FloatElement", "GetViewImpl failed to load template view");
            }

            return view;
        }

        public override string Summary()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public virtual void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            OnUserValueChanged((progress / (float)Precision) + _minValue);
        }

        public virtual void OnStartTrackingTouch(SeekBar seekBar)
        {
        }

        public virtual void OnStopTrackingTouch(SeekBar seekBar)
        {
        }
    }
}