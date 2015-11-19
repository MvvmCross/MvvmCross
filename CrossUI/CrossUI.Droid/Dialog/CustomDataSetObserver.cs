// CustomDataSetObserver.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Database;

namespace CrossUI.Droid.Dialog
{
    public class CustomDataSetObserver : DataSetObserver
    {
        public event EventHandler Changed;
        public event EventHandler Invalidated;

        public override void OnChanged()
        {
            base.OnChanged();
            var handler = Changed;
            handler?.Invoke(this, EventArgs.Empty);
        }
        public override void OnInvalidated()
        {
            base.OnInvalidated();
            var handler = Invalidated;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}