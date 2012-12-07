using System.Globalization;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace CrossUI.Droid.Dialog.Elements
{
    public class FloatElement : ValueElement<float>, SeekBar.IOnSeekBarChangeListener
    {
        private const int precision = 10000000;

        private float _maxValue;
        public float MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        private float _minValue;
        public float MinValue
        {
            get { return _minValue; }
            set { _minValue = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        private bool _showCaption;
        public bool ShowCaption
        {
            get { return _showCaption; }
            set { _showCaption = value; ActOnCurrentAttachedCell(UpdateCaptionDisplay); }
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
                slider.Max = (int) ((_maxValue - _minValue)*precision);
                slider.Progress = (int) ((Value - _minValue)*precision);
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

        public FloatElement(string caption = null, Bitmap left = null, Bitmap right = null, float value = 0, string layoutName = null)
            : base(null, value, layoutName ?? "dialog_floatimage")
        {
            Left = left;
            Right = right;
            MinValue = 0;
            MaxValue = 1;
        }

        protected override View GetViewImpl(Context context, View convertView, ViewGroup parent)
        {
            View view = DroidResources.LoadFloatElementLayout(context, convertView, parent, LayoutName);

            if (view != null)
            {
                TextView label;
                SeekBar slider;
                ImageView left;
                ImageView right;
                DroidResources.DecodeFloatElementLayout(Context, view, out label, out slider, out left, out right);
                slider.SetOnSeekBarChangeListener(this);
            }
            else
            {
                Android.Util.Log.Error("FloatElement", "GetViewImpl failed to load template view");
            }

            return view;
        }

        public override string Summary()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        void SeekBar.IOnSeekBarChangeListener.OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            OnUserValueChanged(((float)progress / (float)precision) - _minValue);
        }

        void SeekBar.IOnSeekBarChangeListener.OnStartTrackingTouch(SeekBar seekBar)
        {
        }

        void SeekBar.IOnSeekBarChangeListener.OnStopTrackingTouch(SeekBar seekBar)
        {
        }
    }
}