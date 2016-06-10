// CustomDataSetObserver.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Database;
using System;

namespace CrossUI.Droid.Dialog
{
    public class CustomDataSetObserver : DataSetObserver
    {
        public event EventHandler Changed;

        public event EventHandler Invalidated;

        public override void OnChanged()
        {
            base.OnChanged();
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public override void OnInvalidated()
        {
            base.OnInvalidated();
            Invalidated?.Invoke(this, EventArgs.Empty);
        }
    }
}