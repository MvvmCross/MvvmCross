using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Util;
using Android.Widget;

namespace MvvmCross.TestProjects.CustomBinding.Droid.Controls
{
    public class BinaryEdit : LinearLayout
    {
        private List<CheckBox> _boxes = new List<CheckBox>();

        public BinaryEdit(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize(context);
        }

        private void Initialize(Context context)
        {
            for (var i = 0; i < 4; i++)
            {
                var box = new CheckBox(context);
                AddView(box);
                _boxes.Add(box);
                box.CheckedChange += (sender, args) =>
                    {
                        UpdateCount();
                    };
            }
        }

        private bool _isUpdating;

        private void UpdateCount()
        {
            if (_isUpdating)
                return;

            var handler = MyCountChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler MyCountChanged;

        public int GetCount()
        {
            return _boxes.Count(b => b.Checked);
        }

        public void SetThat(int count)
        {
            _isUpdating = true;
            try
            {
                var currentCount = GetCount();

                if (count < 0 || count > 4)
                    return;
                
                while (count < currentCount)
                {
                    _boxes.First(b => b.Checked).Checked = false;
                    currentCount = GetCount();
                }
                while (count > currentCount)
                {
                    _boxes.First(b => !b.Checked).Checked = true;
                    currentCount = GetCount();
                }
            }
            finally
            {
                _isUpdating = false;
            }
        }
    }
}