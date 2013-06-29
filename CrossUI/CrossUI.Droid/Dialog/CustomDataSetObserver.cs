using System;
using Android.Database;

namespace CrossUI.Droid.Dialog
{
    public class CustomDataSetObserver: DataSetObserver
    {

        public event EventHandler Changed;
        public event EventHandler Invalidated;

        public override void OnChanged()
        {
            base.OnChanged();
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }
        public override void OnInvalidated()
        {
            base.OnInvalidated();
            if (Invalidated != null)
                Invalidated(this, EventArgs.Empty);
        }
    }
}